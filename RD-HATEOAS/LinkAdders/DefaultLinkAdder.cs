using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RDHATEOAS.Builders;
using RDHATEOAS.Models;
using RDHATEOAS.Rulesets;

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

            var val = (context.Result as OkObjectResult).Value;
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
            };
            var valSerialized = JsonConvert.SerializeObject(JToken.FromObject(val), settings);
            dynamic valToProcess = JsonConvert.DeserializeObject(valSerialized);

            RecursiveSearchAndProcessObject(valToProcess, context, pathId, arrayId);

            (context.Result as OkObjectResult).Value = valToProcess;
        }

        private void RecursiveSearchAndProcessObject(JToken currentObjectValue, ResultExecutingContext context, int pathId, int arrayId)
        {
            if (pathId < (_path[arrayId] ?? new string[] { }).Length)
            {
                // run through path to find relevant object
                var currentObjectType = currentObjectValue.GetType();
                if (currentObjectValue.GetType() == typeof(JArray))
                {
                    foreach (JToken currentObjectListitem in currentObjectValue as IList)
                    {
                        var key = _path[arrayId][pathId];
                        var itemProperties = currentObjectListitem.Children<JProperty>();
                        var nestedElement = itemProperties.FirstOrDefault(x => x.Name == key);
                        if (nestedElement != null)
                        {
                            var nestedObjectValue = nestedElement.Value;
                            RecursiveSearchAndProcessObject(nestedObjectValue, context, pathId + 1, arrayId);
                        }
                    }
                }
                else
                {
                    var key = _path[arrayId][pathId];
                    var itemProperties = currentObjectValue.Children<JProperty>();
                    var nestedElement = itemProperties.FirstOrDefault(x => x.Name == key);
                    if (nestedElement != null)
                    {
                        var nestedObjectValue = nestedElement.Value;
                        RecursiveSearchAndProcessObject(nestedObjectValue, context, pathId + 1, arrayId);
                    }
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

                if (currentObjectValue.GetType() == typeof(JArray))
                {
                    AddLinksToList(context, currentObjectValue, arrayId);
                }
                else
                {
                    AddLinksToObject(context, currentObjectValue as JObject, arrayId);
                }
            }
        }

        private void AddLinksToObject(ResultExecutingContext context, JObject item, int arrayId)
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
                    item.SetPropertyContent("_links", link);
                }
            }
        }

        private void AddLinksToList(ResultExecutingContext context, JToken unformattedList, int arrayId) // Must be a JToken even though it's a JArray because we're replacing it with a JObject later
        {
            var list = unformattedList as JArray;
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
                    if (list[i] is JObject listitem)
                    {
                        // apply links from ruleset
                        foreach (HateoasLink link in ruleset.GetLinks(listitem))
                        {
                            listitem.SetPropertyContent("_links", link);
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
                    JArray temp = (JArray)unformattedList;
                    unformattedList = new JObject();
                    ((JObject)unformattedList).SetPropertyContent("value", temp);
                    ((JObject)unformattedList).SetPropertyContent("_links", link);
                }
            }
        }
    }

    public static partial class ExtensionMethods
    {
        public static JObject SetPropertyContent(this JObject source, string name, object content)
        {
            var prop = source.Property(name);

            if (prop == null)
            {
                prop = new JProperty(name, JContainer.FromObject(content));

                source.Add(prop);
            }
            else
            {
                if (prop.Value is JObject item)
                {
                    var array = new JArray
                    {
                        item,
                        JToken.FromObject(content),
                    };
                    prop.Value = array;
                }
                else if (prop.Value is JArray array)
                {
                    array.Add(JToken.FromObject(content));
                }
            }

            return source;
        }
    }
}
