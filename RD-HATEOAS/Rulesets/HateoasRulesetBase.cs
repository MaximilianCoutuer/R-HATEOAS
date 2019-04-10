using Microsoft.AspNetCore.Mvc.Filters;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Rulesets
{
    public abstract class HateoasRulesetBase : IHateoasRuleset
    {
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
