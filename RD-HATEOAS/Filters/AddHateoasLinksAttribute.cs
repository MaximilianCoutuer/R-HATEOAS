using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Models;
using System.Collections;
using RDHATEOAS.Rulesets;
using System.Linq;
using RDHATEOAS.Extensions;
using RDHATEOAS.Builders;
using System.Threading.Tasks;

namespace RDHATEOAS.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AddHateoasLinksAttribute : ResultFilterAttribute
    {
        #region fields

        private readonly string[] _parameterNames;
        private readonly List<IHateoasRuleset> _rulesets = new List<IHateoasRuleset>();

        #endregion

        #region constructors

        public AddHateoasLinksAttribute(string[] parameterNames, Type[] rulesetNames)
        {
            _parameterNames = parameterNames;
            foreach (var type in rulesetNames)
            {
                _rulesets.Add((IHateoasRuleset)Activator.CreateInstance(type));
            }
        }

        public AddHateoasLinksAttribute(string[] parameterNames, Type rulesetName) : this(parameterNames, new Type[] { rulesetName }) { }

        #endregion

        #region methods

        /// <summary>
        /// This method is invoked whenever a result is sent from a controller method with this attribute attached to it.
        /// </summary>
        /// <param name="context">The result context.</param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
            {
                var urlHelper = new UrlHelper(context); // TODO: Is there no way to use DI?
                var hateoasLinkBuilder = new HateoasLinkBuilder(urlHelper);
                var parameters = new Dictionary<string, Object>();
                if (_parameterNames != null)
                {
                    foreach (string parameterName in _parameterNames)
                    {
                        parameters.Add(parameterName, context.RouteData.Values[parameterName] ?? null);
                    }
                }

                if (okObjectResult.Value.GetType().IsList())
                {
                    var list = okObjectResult.Value as IList;
                    for (int i = 0; i < list.Count; i++)  // Foreach doesn't allow modifying objects
                    {
                        foreach(IHateoasRuleset ruleset in _rulesets.Where(r => r.AppliesToEachListItem == true))
                        {
                            ruleset.Parameters = parameters;
                            var item = (IsHateoasEnabled)list[i];
                            ruleset.AddLinksToRef(ref item, context);
                        }
                    }

                    // TODO: "first" and "last" are hardcoded because they are standard options?
                    // TODO: "prev" and "next"?

                    // HACK: This is horrible and needs a rewrite, there HAS to be a better way
                    var objectList = new ListHateoasEnabled();
                    foreach (Object listitem in list)
                    {
                        objectList.list.Add((Object)listitem);
                    }
                    var hateoaslist = (IsHateoasEnabled)objectList;    // why do I need a cast if it inherits from it? oO
                    foreach (IHateoasRuleset ruleset in _rulesets.Where(r => r.AppliesToEachListItem == false))
                    {
                        ruleset.Parameters = parameters;
                        ruleset.AddLinksToRef(ref hateoaslist, context);
                    }
                    okObjectResult.Value = hateoaslist;
                    // /This is horrible and needs a rewrite
                }
                else
                {
                    var item = (IsHateoasEnabled)okObjectResult.Value;
                    foreach (IHateoasRuleset ruleset in _rulesets)
                    {
                        ruleset.Parameters = parameters;
                        ruleset.AddLinksToRef(ref item, context);
                    }
                }
                //else if (okObjectResult.Value is PagedSearchDTO<Object> pagedSearch)
                //{
                //    Parallel.ForEach(pagedSearch.List.ToList(), (element) =>
                //    {
                //        // result is a paged search??
                //        //test(element, urlHelper);
                //    });
                //}
            }
            base.OnResultExecuting(context);
        }

        #endregion
    }


    //private IHateoasRuleset[] GetControllerBasedLinkRulesets(ResultExecutingContext response)
    //{
    //    var controllerActionDescriptor = response.ActionDescriptor as ControllerActionDescriptor;
    //    if (controllerActionDescriptor != null)
    //    {
    //        Attribute[] attrs = (Attribute[])controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);
    //    }
    //    // TODO: do something with this
    //    // TODO: determine if one or more rulesets (probably one)
    //    return null;
    //}


    //private IHateoasRuleset[] GetModelBasedLinkRulesets(OkObjectResult okObjectResult) {
    //    if (IsList(okObjectResult))
    //    {
    //        // result is a list
    //        List<Object> list = (List<Object>)okObjectResult.Value;
    //        Attribute[] attrs = System.Attribute.GetCustomAttributes(list.First().GetType());
    //        // TODO: do something with this
    //    }
    //    else
    //    {
    //        // result is a model
    //        Object item = (Object)okObjectResult.Value;
    //        Attribute[] attrs = System.Attribute.GetCustomAttributes(item.GetType());
    //        // TODO: do something with this
    //    }
    //    return null;
    //}

    //private IHateoasRuleset[] GetGlobalLinkRulesets(OkObjectResult okObjectResult)
    //{
    //    return null;
    //}
}