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
        private readonly string _parameterName;
        private readonly List<IHateoasRuleset> _rulesets = new List<IHateoasRuleset>();

        public AddHateoasLinksAttribute(string parameterName, Type[] rulesetNames)
        {
            _parameterName = parameterName;
            foreach (var type in rulesetNames)
            {
                _rulesets.Add((IHateoasRuleset)Activator.CreateInstance(type));
            }
        }

        public AddHateoasLinksAttribute(string parameterName, Type rulesetName) : this(parameterName, new Type[] { rulesetName }) { }

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


        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
            {
                var urlHelper = new UrlHelper(context); // DI not possible?
                var hateoasLinkBuilder = new HateoasLinkBuilder(urlHelper);
                var parameter = _parameterName != null ? context.RouteData.Values[_parameterName] : null;

                if (okObjectResult.Value.GetType().IsList())
                {
                    IList enumerable = okObjectResult.Value as IList;
                    for (int i = 0; i < enumerable.Count; i++)
                    //foreach (IsHateoasEnabled item in enumerable.OfType<object>())
                    {
                        foreach(IHateoasRuleset ruleset in _rulesets)
                        {
                            ruleset.Parameter = parameter;
                            var item = (IsHateoasEnabled)enumerable[i];
                            ruleset.AddLinksToRef(ref item, context);
                        }
                    }
                    //Parallel.ForEach((List<Object>)(okObjectResult.Value), (element) =>
                    // {
                    // });
                }
                else
                {
                    var item = (IsHateoasEnabled)okObjectResult.Value;
                    // loop through rulesets and add them to this dynamic object
                    foreach (IHateoasRuleset ruleset in _rulesets)
                    {
                        ruleset.Parameter = parameter;
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
    }
}