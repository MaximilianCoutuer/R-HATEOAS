using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Builders;
using RDHATEOAS.Extensions;
using RDHATEOAS.Models;
using RDHATEOAS.Rulesets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        private UrlHelper urlHelper;
        private HateoasLinkBuilder hateoasLinkBuilder;

        #endregion

        #region constructors

        /// <summary>
        /// AddHateoasLinksAttribute constructor.
        /// </summary>
        /// <param name="parameterNames">Any parameters in the result you wish to pass on to the ruleset.</param>
        /// <param name="rulesetNames">Names of the rulesets you wish to apply to the object.</param>
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
                            if (list[i] is IIsHateoasEnabled listitem)
                            {
                                // apply links from ruleset
                                foreach (HateoasLink link in ruleset.GetLinks(listitem))
                                {
                                    listitem.Links.Add(link);
                                }
                            }
                        }
                    }

                    // replace the list with a ListHateoasEnabled which contains it as well as a links property
                    // TODO: simplify this?
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
                        // apply links from ruleset
                        foreach (HateoasLink link in ruleset.GetLinks(objectList))
                        {
                            objectList.Links.Add(link);
                        }
                    }
                    okObjectResult.Value = objectList;
                }
                else
                {
                    if (okObjectResult.Value is IIsHateoasEnabled item)
                    {
                        foreach (IHateoasRuleset ruleset in _rulesets)
                        {
                            // set fields in ruleset
                            ruleset.SetHelpers(context);
                            ruleset.Parameters = parameters;
                            // apply links from ruleset
                            foreach (HateoasLink link in ruleset.GetLinks(item))
                            {
                                item.Links.Add(link);
                            }
                        }
                    }
                }
            }
            base.OnResultExecuting(context);
        }

        #endregion
    }
}