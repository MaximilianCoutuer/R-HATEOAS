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
                hateoasLinkBuilder.BuildSelfLink(context, "default", "Person")
                    .AddHreflang("be-nl")
                    .AddTitle("Properties of this requested person")
                    .AddType("json"),
                hateoasLinkBuilder.Build(context, "default", "Person", "", "list", HttpMethod.Get)
                    .AddHreflang("be-nl")
                    .AddTitle("List of persons")
                    .AddType("json"),
                hateoasLinkBuilder.Build(context, "default", "Person", "Edit", "edit", HttpMethod.Post, Parameters.GetValueOrDefault("Id") ?? ((Person)item).Id)
                    .AddHrefLang("be-nl")
                    .AddTitle("Edit this person")
                    .AddType("json"),
                hateoasLinkBuilder.Build(context, "default", "Person", "Delete", "delete", HttpMethod.Delete, Parameters.GetValueOrDefault("Id") ?? ((Person)item).Id)
                    .AddHrefLang("be-nl")
                    .AddTitle("Delete this person")
                    .AddType("json"),
            };
        }


    }
}
