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

        public void AddDescribedLink(ref IDictionary<string, Object> itemDynamic, ResultExecutingContext context, dynamic data) {
            itemDynamic.Add("_links", AddLinkObjectToRef(itemDynamic, context, data));
        }

        protected virtual HateoasLink[] AddLinkObjectToRef(IDictionary<string, Object> itemDynamic, ResultExecutingContext context, dynamic data)
        {
            // default null implementation
            return null;
        }
    }
}
