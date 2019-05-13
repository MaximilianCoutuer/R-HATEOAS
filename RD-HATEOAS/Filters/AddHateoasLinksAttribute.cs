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
    /// <summary>
    /// This filter is applied to a controller method via an attribute.
    /// It intercepts the response and adds links to it.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AddHateoasLinksAttribute : ResultFilterAttribute
    {
        #region fields

        private readonly string[] _parameterNames;
        private readonly List<IHateoasRuleset> _rulesets = new List<IHateoasRuleset>();
        private Dictionary<string, Object> parameters = new Dictionary<string, Object>();

        private UrlHelper urlHelper;
        private HateoasLinkBuilder hateoasLinkBuilder;

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
        /// This method is invoked whenever a result is sent from a controller method decorated with this attribute.
        /// </summary> 
        /// <param name="context">The result context from the result that caused this to be run.</param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
            {
                urlHelper = new UrlHelper(context);
                hateoasLinkBuilder = new HateoasLinkBuilder(urlHelper);

                if (_parameterNames != null)
                {
                    foreach (string parameterName in _parameterNames)
                    {
                        parameters[parameterName] = context.RouteData.Values[parameterName] ?? null;
                    }
                }

                if (okObjectResult.Value.GetType().IsList())
                {
                    var list = okObjectResult.Value as IList;

                    for (int i = 0; i < list.Count; i++)
                    {
                        foreach (IHateoasRuleset ruleset in _rulesets.Where(r => r.AppliesToEachListItem == true))
                        {
                            // set fields in ruleset
                            ruleset.SetHelpers(context);
                            ruleset.Parameters = parameters;
                            ruleset.Parameters["Id"] = i;
                            ruleset.Parameters["Count"] = list.Count;

                            var listitem = (IIsHateoasEnabled)list[i];
                            // get links from ruleset
                            foreach (HateoasLink link in ruleset.GetLinks(listitem))
                            {
                                listitem.Links.Add(link);
                            }
                        }
                    }

                    var objectList = new ListHateoasEnabled();
                    foreach (object listitem in list)
                    {
                        objectList.List.Add(listitem);
                    }

                    foreach (IHateoasRuleset ruleset in _rulesets.Where(r => r.AppliesToEachListItem == false))
                    {
                        // set fields in ruleset
                        ruleset.SetHelpers(context);
                        ruleset.Parameters = parameters;
                        ruleset.Parameters["Count"] = list.Count;
                        // get links from ruleset
                        foreach (HateoasLink link in ruleset.GetLinks(objectList))
                        {
                            objectList.Links.Add(link);
                        }
                    }
                    okObjectResult.Value = objectList;
                }
                else
                {
                    var item = (IIsHateoasEnabled)okObjectResult.Value;
                    foreach (IHateoasRuleset ruleset in _rulesets)
                    {
                        // set fields in ruleset
                        ruleset.SetHelpers(context);
                        ruleset.Parameters = parameters;
                        // get links from ruleset
                        foreach (HateoasLink link in ruleset.GetLinks(item))
                        {
                            item.Links.Add(link);
                        }
                    }
                }
            }
            base.OnResultExecuting(context);
        }

        #endregion
    }
}