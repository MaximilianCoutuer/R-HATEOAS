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
        
        public virtual void AddDescribedLink(ref IDictionary<string, Object> itemDynamic) {
            // default null implementation
        }

        private void AddLinkObjectToRef(ref IDictionary<string, Object> itemDynamic, HateoasLink link)
        {
            itemDynamic.Add("links", link);
        }


    }
}
