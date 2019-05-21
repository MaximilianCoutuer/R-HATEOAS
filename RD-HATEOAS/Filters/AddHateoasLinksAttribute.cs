namespace RDHATEOAS.Filters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Routing;
    using RDHATEOAS.Builders;
    using RDHATEOAS.Extensions;
    using RDHATEOAS.Models;
    using RDHATEOAS.Rulesets;

    /// <summary>
    /// This filter is applied to a controller method via an attribute.
    /// It intercepts the response and adds links to it.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AddHateoasLinksAttribute : ResultFilterAttribute
    {
        #region fields

        private readonly List<string> _parameterNames;
        private readonly List<string[]> _path;
        private readonly List<IHateoasRuleset> _rulesets = new List<IHateoasRuleset>();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        private UrlHelper urlHelper;
        private HateoasLinkBuilder hateoasLinkBuilder;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddHateoasLinksAttribute"/> class.
        /// </summary>
        /// <param name="parameterNames">Any parameters in the result you wish to pass on to the ruleset.</param>
        /// <param name="rulesetNames">Names of the rulesets you wish to apply to the object.</param>
        /// <param name="path">Path to the object to add links for, as a sequence of keys.</param>
        public AddHateoasLinksAttribute(string[] parameterNames, Type[] rulesetNames, string[] path)
        {
            _parameterNames = new List<string>(parameterNames);

            // split strings in path parameter and add them as arrays to the path
            var pathUnsplit = new List<string>(path);
            foreach (string pathCode in pathUnsplit)
            {
                _path.Add(pathCode.Split("|"));
            }

            foreach (var type in rulesetNames)
            {
                _rulesets.Add((IHateoasRuleset)Activator.CreateInstance(type));
            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddHateoasLinksAttribute"/> class.
        /// </summary>
        /// <param name="parameterNames"></param>
        /// <param name="rulesetName"></param>
        public AddHateoasLinksAttribute(string[] parameterNames, Type rulesetName, string path)
            : this(parameterNames, new Type[] { rulesetName }, new string[] { path })
        {
        }

        #endregion

        #region methods

        /// <summary>
        /// This method is invoked whenever a result is sent from a controller method decorated with this attribute.
        /// It processes the result if it is a 200.
        /// </summary>
        /// <param name="context">The result context from the result that caused this to be run.</param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
            {
                for (int i = 0; i < _path.Count; i++)
                {
                    RecursiveFindObjectAndAddLinks(okObjectResult.Value, context, 0, i);
                }
            }

            base.OnResultExecuting(context);
        }

        /// <summary>
        /// Drills down into the path tree of the result object until it reaches the destination object or list.
        /// </summary>
        /// <param name="currentObjectValue"></param>
        /// <param name="context"></param>
        /// <param name="pathId"></param>
        /// <param name="arrayId"></param>
        private void RecursiveFindObjectAndAddLinks(object currentObjectValue, ResultExecutingContext context, int pathId, int arrayId)
        {
            if (pathId < _path[arrayId].Length) // TODO: test if not always 1
            {
                var currentObjectType = currentObjectValue.GetType();
                if (currentObjectType.IsList())
                {
                    foreach (object currentObjectListitem in currentObjectValue as IList)
                    {
                        currentObjectType = currentObjectListitem.GetType(); // TODO: error handling
                        var key = currentObjectType.GetProperty(_path[arrayId][pathId]);
                        var nestedObjectValue = key.GetValue(currentObjectListitem);
                        RecursiveFindObjectAndAddLinks(nestedObjectValue, context, pathId + 1, arrayId);
                    }
                }
                else
                {
                    var key = currentObjectType.GetProperty(_path[arrayId][pathId]);
                    var nestedObjectValue = key.GetValue(currentObjectValue);
                    RecursiveFindObjectAndAddLinks(nestedObjectValue, context, pathId + 1, arrayId);
                }

            }
            else
            {
                AddLinks(context, currentObjectValue, arrayId);
            }

        }

        private void AddLinks(ResultExecutingContext context, object item, int arrayId)
        {
            urlHelper = new UrlHelper(context);
            hateoasLinkBuilder = new HateoasLinkBuilder(urlHelper);

            if (_parameterNames != null)
            {
                foreach (string parameterName in _parameterNames)
                {
                    _parameters[parameterName] = context.RouteData.Values[parameterName] ?? null;
                }
            }

            if (item.GetType().IsList())
            {
                // TODO: simplify this?
                var objectList = new ListHateoasEnabled();
                var list = item as IList;
                foreach (object listitem in list)
                {
                    objectList.List.Add(listitem);
                }
                AddLinksToList(context, objectList, arrayId);
            }
            else
            {
                AddLinksToObject(context, item as IIsHateoasEnabled, arrayId);
            }
        }

        private void AddLinksToObject(ResultExecutingContext context, IIsHateoasEnabled item, int arrayId)
        {
            foreach (IHateoasRuleset ruleset in _rulesets)
            {
                // set fields in ruleset
                ruleset.SetHelpers(context);
                ruleset.Parameters = _parameters;

                // apply links from ruleset
                foreach (HateoasLink link in ruleset.GetLinks(item))
                {
                    item.Links.Add(link);
                }
            }
        }

        private void AddLinksToList(ResultExecutingContext context, ListHateoasEnabled unformattedList, int arrayId)
        {
            var list = unformattedList.List as IList;
            var rulesets = _rulesets[arrayId];
            for (int i = 0; i < list.Count; i++)
            {
                foreach (IHateoasRuleset ruleset in rulesets.Where(r => r.AppliesToEachListItem == true))
                {
                    // set fields in ruleset to help rulesets make the correct decisions
                    ruleset.SetHelpers(context);
                    ruleset.Parameters = _parameters;
                    ruleset.Parameters["RD-ListId"] = i;
                    ruleset.Parameters["RD-ListCount"] = list.Count;
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

            foreach (IHateoasRuleset ruleset in rulesets.Where(r => r.AppliesToEachListItem == false))
            {
                // set fields in ruleset
                ruleset.SetHelpers(context);
                ruleset.Parameters = _parameters;
                ruleset.Parameters["RD-ListCount"] = list.Count;

                // apply links from ruleset
                foreach (HateoasLink link in ruleset.GetLinks(unformattedList))
                {
                    unformattedList.Links.Add(link);
                }
            }

        }

        #endregion
    }
}