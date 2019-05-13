using ExampleAPI.Models;
using RDHATEOAS.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace RDHATEOAS.Rulesets
{
    public class ExampleRulesetFullLinks : HateoasRulesetBase
    {
        public override bool AppliesToEachListItem { get; set; } = true;

        public override List<HateoasLink> GetLinks(IIsHateoasEnabled item) {

            return new List<HateoasLink> {
                hateoasLinkBuilder.BuildSelfLink(context, "HateoasRoute", "person")
                    .AddHreflang("be-nl")
                    .AddTitle("Properties of this requested person")
                    .AddType("json"),
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "list", HttpMethod.Get)
                    .AddHreflang("be-nl")
                    .AddTitle("List of persons")
                    .AddType("json"),
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "edit", HttpMethod.Post, Parameters.GetValueOrDefault("Id") ?? ((Person)item).Id),
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "delete", HttpMethod.Delete, Parameters.GetValueOrDefault("Id") ?? ((Person)item).Id),
            };
        }


    }
}
