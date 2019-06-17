using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rhateoas.LinkAdders;
using Rhateoas.PropertySets;
using Rhateoas.Rulesets;

namespace Rhateoas.Filters
{
    /// <summary>
    /// This filter is applied to a controller method via an attribute.
    /// It intercepts the response and adds links to it.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AddHateoasLinksAttribute : ResultFilterAttribute
    {
        #region fields

        private readonly List<IHateoasPropertySet> _propertySets = new List<IHateoasPropertySet>();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        #endregion

        #region constructors

        public AddHateoasLinksAttribute(Type[] propertySetTypes)
        {
            foreach (Type propertySetType in propertySetTypes)
            {
                var propertySet = (IHateoasPropertySet)Activator.CreateInstance(propertySetType);
                if (propertySet == null)
                {
                    throw new ArgumentException("Type " + propertySetType + " is not a valid HATEOAS property set");
                }

                if (propertySet.Ruleset == null)
                {
                    throw new ArgumentException("Ruleset can't be null.");
                }

                if (typeof(IHateoasRuleset).IsAssignableFrom(propertySet.Ruleset) == false)
                {
                    throw new ArgumentException("Type " + propertySet.Ruleset + " is not a valid HATEOAS ruleset (it does not implement IHateoasRuleset).");
                }

                _propertySets.Add(propertySet);
            }
        }

        public AddHateoasLinksAttribute(Type propertySetType)
            : this(new[] { propertySetType })
        {
        }

        #endregion

        #region methods

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
            {
                foreach (IHateoasPropertySet propertySet in _propertySets)
                {
                    var parameterNames = propertySet.Parameters;
                    var ruleset = (IHateoasRuleset)Activator.CreateInstance(propertySet.Ruleset);
                    var path = propertySet.Path;
                    var linkAdder = new DefaultLinkAdder(parameterNames, path, ruleset, _parameters);
                    linkAdder.AddLinks(context);
                }
            }

            base.OnResultExecuting(context);
        }

        #endregion
    }
}