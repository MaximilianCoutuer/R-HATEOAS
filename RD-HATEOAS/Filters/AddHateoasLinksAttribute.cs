namespace RDHATEOAS.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using RDHATEOAS.Builders;
    using RDHATEOAS.LinkAdders;
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
        private readonly ILinkAdder _linkAdder;

        #endregion

        #region constructors

        public AddHateoasLinksAttribute(string[] parameterNames, Type[] rulesetNames, string[] path)
        {
            _parameterNames = new List<string>(parameterNames ?? new string[] { });
            _path = new List<string[]>();
            var pathUnsplit = new List<string>(path ?? new string[] { null });
            foreach (string pathCode in pathUnsplit)
            {
                if (pathCode != null)
                {
                    _path.Add(pathCode.Split("|"));
                } else
                {
                    _path.Add(null);
                }
            }

            foreach (var type in rulesetNames)
            {
                _rulesets.Add((IHateoasRuleset)Activator.CreateInstance(type));
            }

            _linkAdder = new DefaultLinkAdder(_parameterNames, _path, _rulesets, _parameters);


        }

        public AddHateoasLinksAttribute(string[] parameterNames, Type rulesetName, string path)
            : this(parameterNames, new Type[] { rulesetName }, new string[] { path })
        {
        }

        #endregion

        #region methods

        public override void OnResultExecuting(ResultExecutingContext context)
        {

            if (context.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
            {
                for (int i = 0; i < _rulesets.Count; i++)
                {
                    _linkAdder.AddLinks(okObjectResult.Value, context, 0, i);
                }
            }




            //var val = (context.Result as OkObjectResult).Value;
            //var jo = JArray.FromObject(val);
            //var grrrrrrr = new JObject(new JProperty("lol", "rofl"));
            //grrrrrrr.Add("argh", jo);
            ////jo.Add("lol", "rofl");

            //var settings = new JsonSerializerSettings
            //{
            //    ContractResolver = new DefaultContractResolver()
            //};
            //var help = JsonConvert.SerializeObject(grrrrrrr, settings);

            //var expConverter = new ExpandoObjectConverter();
            //dynamic rev = JsonConvert.DeserializeObject<ExpandoObject>(help, expConverter);

            //(context.Result as OkObjectResult).Value = rev;




            base.OnResultExecuting(context);
        }

        #endregion
    }
}