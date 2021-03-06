﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Rhateoas.Builders;
using Rhateoas.Models;
using Rhateoas.Rulesets;

namespace Rhateoas.LinkAdders
{
    /// <summary>
    /// This class adds HATEOAS links to a context that is passed in.
    /// </summary>
    public class DefaultLinkAdder : ILinkAdder
    {
        private readonly List<string> _parameterNames;
        private readonly List<string> _path;
        private readonly IHateoasRuleset _ruleset;
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        private UrlHelper _urlHelper;
        private HateoasLinkBuilder _hateoasLinkBuilder;

        public DefaultLinkAdder(List<string> parameterNames, List<string> path, IHateoasRuleset rulesets, Dictionary<string, object> parameters)
        {
            _parameterNames = parameterNames;
            _path = path;
            _ruleset = rulesets;
            _parameters = parameters;
       }

        public void AddLinks(ResultExecutingContext context)
        {
            var currentObjectValue = (context.Result as OkObjectResult).Value;
            _urlHelper = new UrlHelper(context);
            _hateoasLinkBuilder = new HateoasLinkBuilder(_urlHelper);

            var value = (context.Result as OkObjectResult).Value;
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
            };
            var valueSerialized = JsonConvert.SerializeObject(JToken.FromObject(value), settings);
            dynamic valueToProcess = JsonConvert.DeserializeObject(valueSerialized);

            (context.Result as OkObjectResult).Value = valueToProcess;

            var val = (JToken)(context.Result as OkObjectResult).Value;

            this.RecursiveSearchAndProcessObject(ref val, context, 0);

            (context.Result as OkObjectResult).Value = val;
        }

        private void RecursiveSearchAndProcessObject(ref JToken currentObjectValue, ResultExecutingContext context, int pathId)
        {
            if (pathId < _path.Count)
            {
                var currentObjectType = currentObjectValue.GetType();
                if (currentObjectValue.GetType() == typeof(JArray))
                {
                    foreach (JToken currentObjectListitem in currentObjectValue as IList)
                    {
                        var key = _path[pathId];
                        var itemProperties = currentObjectListitem.Children<JProperty>();
                        var nestedElement = itemProperties.FirstOrDefault(x => x.Name == key);
                        if (nestedElement != null)
                        {
                            var nestedObjectValue = nestedElement.Value;
                            this.RecursiveSearchAndProcessObject(ref nestedObjectValue, context, pathId + 1);
                        }
                    }
                }
                else
                {
                    var key = _path[pathId];
                    var itemProperties = currentObjectValue.Children<JProperty>();
                    var nestedElement = itemProperties.FirstOrDefault(x => x.Name == key);
                    if (nestedElement != null)
                    {
                        var nestedObjectValue = nestedElement.Value;
                        this.RecursiveSearchAndProcessObject(ref nestedObjectValue, context, pathId + 1);
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
                    AddLinksToList(context, ref currentObjectValue);
                }
                else
                {
                    AddLinksToObject(context, ref currentObjectValue);
                }
            }
        }

        private void AddLinksToObject(ResultExecutingContext context, ref JToken item)
        {
            if (item != null && item.HasValues)
            {
                // set fields in ruleset to help rulesets make the correct decisions
                _ruleset.SetHelpers(context);
                _ruleset.Parameters = _parameters;

                // apply links from ruleset
                foreach (HateoasLink link in _ruleset.GetLinks(item))
                {
                    ((JObject)item).SetPropertyContent("_links", link);
                }
            }
        }

        private void AddLinksToList(ResultExecutingContext context, ref JToken tokenList) // Must be a JToken even though it's a JArray because we're replacing it with a JObject later
        {
            if (tokenList != null && tokenList.HasValues)
            {
                var list = tokenList as JArray;
                if (_ruleset.AppliesToEachListItem == true)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        // set fields in ruleset to help rulesets make the correct decisions
                        _ruleset.SetHelpers(context);
                        _ruleset.Parameters = _parameters;
                        _ruleset.Parameters["RD-ListId"] = i;
                        _ruleset.Parameters["RD-ListCount"] = list.Count;
                        if (list[i] is JObject listitem)
                        {
                            // apply links from ruleset
                            foreach (HateoasLink link in _ruleset.GetLinks(listitem))
                            {
                                listitem.SetPropertyContent("_links", link);
                            }
                        }
                    }
                }

                if (_ruleset.AppliesToEachListItem == false)
                {
                    // set fields in ruleset to help rulesets make the correct decisions
                    _ruleset.SetHelpers(context);
                    _ruleset.Parameters = _parameters;
                    _ruleset.Parameters["RD-ListCount"] = list.Count;

                    // apply links from ruleset
                    JArray temp = (JArray)tokenList.DeepClone();
                    tokenList = new JObject();
                    ((JObject)tokenList).SetPropertyContent("values", temp);
                    foreach (HateoasLink link in _ruleset.GetLinks(tokenList))
                    {
                        ((JObject)tokenList).SetPropertyContent("_links", link);
                    }
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
