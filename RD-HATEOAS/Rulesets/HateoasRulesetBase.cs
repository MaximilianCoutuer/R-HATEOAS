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
        
        public virtual void AddDescribedLink(ref IDictionary<string, Object> itemDynamic, ResultExecutingContext context, dynamic data) {
            // default null implementation
        }

        protected void AddLinkObjectToRef(ref IDictionary<string, Object> itemDynamic, HateoasLink link)
        {
            itemDynamic.Add("_links", link);
        }


    }
}
