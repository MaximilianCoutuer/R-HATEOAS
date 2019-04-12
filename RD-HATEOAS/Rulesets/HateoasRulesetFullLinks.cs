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

            var test = Parameter;
            return new HateoasLink[] {
                // TODO: sugar for self link
                new HateoasLinkBuilder(new UrlHelper(context)).Build(context, "Testroute", "person", "self", HttpMethod.Get),
                new HateoasLinkBuilder(new UrlHelper(context)).Build(context, "Testroute", "person", "self", HttpMethod.Get, int.Parse((string)Parameter)),
                new HateoasLinkBuilder(new UrlHelper(context)).Build(context, "Testroute", "person", "self", HttpMethod.Post, null, "be-nl"),
                new HateoasLinkBuilder(new UrlHelper(context)).Build(context, "Testroute", "person", "self", HttpMethod.Delete),
            };
        }


    }
}
