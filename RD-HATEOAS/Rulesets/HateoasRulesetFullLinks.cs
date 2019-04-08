using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Rulesets
{
    public class HateoasRulesetFullLinks : HateoasRulesetBase
    {
        
        public override void AddDescribedLink(ref IDictionary<string, Object> itemDynamic) {
            // TODO: Full link implementation


            // TODO: refactor to eliminate this line?
            AddLinkObjectToRef(ref itemDynamic, null);
        }


    }
}
