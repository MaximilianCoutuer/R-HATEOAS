using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Builders;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Rulesets
{
    public class HateoasRulesetFullLinks : HateoasRulesetBase
    {
        
        public override void AddDescribedLink(ref IDictionary<string, Object> itemDynamic, ResultExecutingContext context, dynamic data) {
            // TODO: Full link implementation


            // TODO: refactor to eliminate this line?
            //AddLinkObjectToRef(ref itemDynamic, new Models.HateoasLink("test", "test", HttpMethod.Get));
            AddLinkObjectToRef(ref itemDynamic, (new HateoasLinkBuilder(new UrlHelper(context)).Build(context)));
        }


    }
}
