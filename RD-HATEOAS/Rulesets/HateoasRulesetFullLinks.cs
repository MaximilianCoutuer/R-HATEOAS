using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Builders;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Rulesets
{
    public class HateoasRulesetFullLinks : HateoasRulesetBase
    {
        protected override HateoasLink[] AddLinkObjectToRef(IDictionary<string, Object> itemDynamic, ResultExecutingContext context, dynamic data) {

            return new HateoasLink[] {
                new HateoasLinkBuilder(new UrlHelper(context)).Build(context, "Testroute", "person", "self", HttpMethod.Get),
                new HateoasLinkBuilder(new UrlHelper(context)).Build(context, "Testroute", "person", "self", HttpMethod.Get, 1),
                new HateoasLinkBuilder(new UrlHelper(context)).Build(context, "Testroute", "person", "self", HttpMethod.Post, null, "be-nl"),
                new HateoasLinkBuilder(new UrlHelper(context)).Build(context, "Testroute", "person", "self", HttpMethod.Delete),
            };
        }


    }
}
