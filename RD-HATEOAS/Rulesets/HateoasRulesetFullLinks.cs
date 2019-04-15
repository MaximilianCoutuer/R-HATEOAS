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
            int id;
            return new HateoasLink[] {
                new HateoasLinkBuilder(_urlHelper).Build(context, "HateoasRoute", "person", "self", HttpMethod.Get, int.TryParse((string)Parameter, out id) ? id : default(int?)),
                new HateoasLinkBuilder(_urlHelper).Build(context, "HateoasRoute", "person", "list", HttpMethod.Get),
                new HateoasLinkBuilder(_urlHelper).Build(context, "HateoasRoute", "person", "edit", HttpMethod.Post).AddHreflang("be-nl").AddMedia("doctype/jpg").AddTitle("Photo of a duck").AddType("image"),
                new HateoasLinkBuilder(_urlHelper).Build(context, "HateoasRoute", "person", "delete", HttpMethod.Delete),
            };
        }


    }
}
