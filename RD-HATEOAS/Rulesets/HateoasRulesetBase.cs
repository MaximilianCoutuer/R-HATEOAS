using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json.Linq;
using Rhateoas.Builders;
using Rhateoas.Models;

namespace Rhateoas.Rulesets
{
    /// <summary>
    /// The base class for a HATEOAS ruleset.
    /// Rulesets determine which links should be added to a HATEOAS enabled object.
    /// </summary>
    public abstract class HateoasRulesetBase : IHateoasRuleset
    {
        public Dictionary<string, object> Parameters { get; set; }

        public virtual bool AppliesToEachListItem { get; set; } = true;

        protected UrlHelper UrlHelper { get; set; }

        protected ResultExecutingContext Context { get; set; }

        protected HateoasLinkBuilder HateoasLinkBuilder { get; set; }

        public void SetHelpers(ResultExecutingContext context)
        {
            Context = context;
            UrlHelper = new UrlHelper(context);
            HateoasLinkBuilder = new HateoasLinkBuilder(UrlHelper);
        }

        /// <summary>
        /// Returns a list of links to add to the item.
        /// </summary>
        /// <param name="item">The item to which we want to add links.</param>
        /// <returns>The links.</returns>
        public virtual List<HateoasLink> GetLinks(JToken item)
        {
            // default null implementation yields no links
            return null;
        }
    }
}
