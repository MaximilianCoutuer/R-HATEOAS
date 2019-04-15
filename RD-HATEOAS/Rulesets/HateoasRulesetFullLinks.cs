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
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "self", HttpMethod.Get, Parameter),
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "list", HttpMethod.Get),
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "edit", HttpMethod.Post).AddHreflang("be-nl").AddMedia("doctype/jpg").AddTitle("Photo of a duck").AddType("image"),
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "delete", HttpMethod.Delete),
            };
        }


    }
}
