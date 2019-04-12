using Microsoft.AspNetCore.Mvc.Filters;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;

namespace RDHATEOAS.Rulesets
{
    public abstract class HateoasRulesetBase : IHateoasRuleset
    {
        public HateoasRulesetBase() { }

        public object Parameter { get; set; }

        public void AddLinksToRef(ref IsHateoasEnabled item, ResultExecutingContext context, dynamic data) {
            item._links = GetLinks(item, context, data);
        }

        protected virtual HateoasLink[] GetLinks(IsHateoasEnabled item, ResultExecutingContext context, dynamic data)
        {
            // default null implementation
            return null;
        }
    }
}
