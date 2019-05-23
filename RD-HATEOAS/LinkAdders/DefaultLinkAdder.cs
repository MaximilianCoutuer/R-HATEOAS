using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RDHATEOAS.Builders;
using RDHATEOAS.Extensions;
using RDHATEOAS.Models;
using RDHATEOAS.Rulesets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.LinkAdders
{
    public class DefaultLinkAdder : ILinkAdder
    {
        private readonly List<string> _parameterNames;
        private readonly List<string[]> _path;
        private readonly List<IHateoasRuleset> _rulesets = new List<IHateoasRuleset>();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        private UrlHelper urlHelper;
        private HateoasLinkBuilder hateoasLinkBuilder;

        private dynamic lol = null;

        public DefaultLinkAdder(List<string> parameterNames, List<string[]> path, List<IHateoasRuleset> rulesets, Dictionary<string, object> parameters)
        {
            _parameterNames = parameterNames;
            _path = path;
            _rulesets = rulesets;
            _parameters = parameters;
       }

        public void AddLinks(object currentObjectValue, ResultExecutingContext context, int pathId, int arrayId)
        {
            urlHelper = new UrlHelper(context);
            hateoasLinkBuilder = new HateoasLinkBuilder(urlHelper);

            //var val = (context.Result as OkObjectResult).Value;
            //var jo = JToken.FromObject(val);
            ////var grrrrrrr = new JObject(new JProperty("lol", "rofl"));
            ////grrrrrrr.Add("argh", jo);
            //////jo.Add("lol", "rofl");

            //var settings = new JsonSerializerSettings
            //{
            //    ContractResolver = new DefaultContractResolver()
            //};
            //var help = JsonConvert.SerializeObject(jo, settings);

            RecursiveSearchAndProcessObject(currentObjectValue, context, pathId, arrayId);

            //(context.Result as OkObjectResult).Value = help;

        }

        private void RecursiveSearchAndProcessObject(object currentObjectValue, ResultExecutingContext context, int pathId, int arrayId)
        {
            if (pathId < (_path[arrayId] ?? new string[] { }).Length) // TODO: test if not always 1
            {
                // run through path to find relevant object
                var currentObjectType = currentObjectValue.GetType();
                if (currentObjectType.IsList())
                {
                    foreach (object currentObjectListitem in currentObjectValue as IList)
                    {
                        currentObjectType = currentObjectListitem.GetType(); // TODO: error handling
                        var key = currentObjectType.GetProperty(_path[arrayId][pathId]);
                        var nestedObjectValue = key.GetValue(currentObjectListitem);
                        RecursiveSearchAndProcessObject(nestedObjectValue, context, pathId + 1, arrayId);
                    }
                }
                else
                {
                    var key = currentObjectType.GetProperty(_path[arrayId][pathId]);
                    var nestedObjectValue = key.GetValue(currentObjectValue);
                    RecursiveSearchAndProcessObject(nestedObjectValue, context, pathId + 1, arrayId);
                }
            }
            else
            {
                // add links depending on whether the item is an object or list
                if (_parameterNames != null)
                {
                    foreach (string parameterName in _parameterNames)
                    {
                        _parameters[parameterName] = context.RouteData.Values[parameterName] ?? null;
                    }
                }

                if (currentObjectValue.GetType().IsList())
                {
                    // TODO: simplify this?
                    var objectList = new ListHateoasEnabled();
                    var list = currentObjectValue as IList;
                    foreach (object listitem in list)
                    {
                        objectList.List.Add(listitem);
                    }
                    AddLinksToList(context, objectList, arrayId);
                }
                else
                {
                    AddLinksToObject(context, currentObjectValue as IIsHateoasEnabled, arrayId);
                }
            }
        }

        private void AddLinksToObject(ResultExecutingContext context, IIsHateoasEnabled item, int arrayId)
        {
            var ruleset = _rulesets[arrayId];
            if (ruleset.AppliesToEachListItem == true)
            {
                // set fields in ruleset to help rulesets make the correct decisions
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
            var ruleset = _rulesets[arrayId];
            if (ruleset.AppliesToEachListItem == true)
            {
                for (int i = 0; i < list.Count; i++)
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

            if (ruleset.AppliesToEachListItem == false)
            {
                // set fields in ruleset to help rulesets make the correct decisions
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

    }
}
