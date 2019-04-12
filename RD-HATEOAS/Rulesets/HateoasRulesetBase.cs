using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;

namespace RDHATEOAS.Rulesets
{
    public abstract class HateoasRulesetBase : IHateoasRuleset
    {
        public object Parameter { get; set; }
        protected UrlHelper _urlHelper { get; set; }

        public void AddLinksToRef(ref IsHateoasEnabled item, ResultExecutingContext context) {
            _urlHelper = new UrlHelper(context);
            item._links = GetLinks(item, context);
        }

        protected virtual HateoasLink[] GetLinks(IsHateoasEnabled item, ResultExecutingContext context)
        {
            // default null implementation
            return null;
        }
    }
}
