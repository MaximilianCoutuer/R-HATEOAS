using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Builders;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RDHATEOAS.Rulesets
{
    public class HateoasRulesetFullLinks : HateoasRulesetBase
    {
        protected override HateoasLink[] GetLinks(IsHateoasEnabled item, ResultExecutingContext context) {

            // TODO: sugar for self link
            // TODO: automatic first/last
            return new HateoasLink[] {
                new HateoasLinkBuilder(_urlHelper).Build(context, "Testroute", "person", "self", HttpMethod.Get, int.Parse((string)Parameter)),
                new HateoasLinkBuilder(_urlHelper).Build(context, "Testroute", "person", "list", HttpMethod.Get),
                new HateoasLinkBuilder(_urlHelper).Build(context, "Testroute", "person", "edit", HttpMethod.Post, null, "be-nl"),
                new HateoasLinkBuilder(_urlHelper).Build(context, "Testroute", "person", "delete", HttpMethod.Delete),
            };
        }


    }
}
