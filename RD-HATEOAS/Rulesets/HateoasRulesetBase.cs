using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Builders;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;

namespace RDHATEOAS.Rulesets
{
    /// <summary>
    /// The base class for a HATEOAS ruleset.
    /// Rulesets determine which links should be added to a HATEOAS enabled object.
    /// </summary>
    public abstract class HateoasRulesetBase : IHateoasRuleset
    {
        public object Parameter { get; set; }
        protected UrlHelper _urlHelper { get; set; }
        protected HateoasLinkBuilder hateoasLinkBuilder { get; set; }

        /// <summary>
        /// Accepts a reference to an instance of a HATEOAS enabled object and adds links to it.
        /// The logic determining which links are to be added is in GetLinks(), which should be overridden in subclasses.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="context"></param>
        public void AddLinksToRef(ref IsHateoasEnabled item, ResultExecutingContext context) {
            _urlHelper = new UrlHelper(context);
            hateoasLinkBuilder = new HateoasLinkBuilder(_urlHelper);
            item._links = GetLinks(item, context);
        }

        protected virtual HateoasLink[] GetLinks(IsHateoasEnabled item, ResultExecutingContext context)
        {
            // default null implementation yields no links
            return null;
        }
    }
}
