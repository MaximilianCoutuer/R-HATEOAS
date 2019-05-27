using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RDHATEOAS.LinkAdders;
using RDHATEOAS.Rulesets;

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

        private readonly List<string> _parameterNames;
        private readonly List<string[]> _path;
        private readonly List<IHateoasRuleset> _rulesets = new List<IHateoasRuleset>();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();
        private readonly ILinkAdder _linkAdder;

        #endregion

        #region constructors

        public AddHateoasLinksAttribute(string[] parameterNames, Type[] rulesetNames, string[] path)
        {
            if (rulesetNames == null)
            {
                throw new ArgumentException("Ruleset array can't be null.");
            }

            if (path != null && rulesetNames.Length < path.Length)
            {
                throw new ArgumentException("Number of rulesets must be equal to or greater than the number of paths.");
            }

            _parameterNames = new List<string>(parameterNames ?? new string[] { });
            _path = new List<string[]>();
            var pathUnsplit = new List<string>(path ?? new string[] { null });
            foreach (string pathCode in pathUnsplit)
            {
                if (pathCode != null)
                {
                    _path.Add(pathCode.Split("|"));
                }
                else
                {
                    _path.Add(null);
                }
            }

            foreach (var type in rulesetNames)
            {
                if (typeof(IHateoasRuleset).IsAssignableFrom(type))
                {
                    _rulesets.Add((IHateoasRuleset)Activator.CreateInstance(type));
                } else
                {
                    throw new ArgumentException("Type " + type + " is not a valid HATEOAS ruleset (it does not implement IHateoasRuleset).");
                }
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

            base.OnResultExecuting(context);
        }

        #endregion
    }
}