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
                new HateoasLinkBuilder(_urlHelper).Build(context, "HateoasRoute", "person", "self", HttpMethod.Get, (Parameter != null) ? (int?)int.Parse((string)Parameter) : null),   // HACK: cast to int? is required for code to compile
                new HateoasLinkBuilder(_urlHelper).Build(context, "HateoasRoute", "person", "list", HttpMethod.Get),
                new HateoasLinkBuilder(_urlHelper).Build(context, "HateoasRoute", "person", "edit", HttpMethod.Post, null, "be-nl"),
                new HateoasLinkBuilder(_urlHelper).Build(context, "HateoasRoute", "person", "delete", HttpMethod.Delete),
            };
        }


    }
}
