using ExampleAPI.Models;
using RDHATEOAS.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace RDHATEOAS.Rulesets
{
    public class DemoRulesetFullLinks : HateoasRulesetBase
    {
        public override bool AppliesToEachListItem { get; set; } = true;

        public override List<HateoasLink> GetLinks(IIsHateoasEnabled item) {

            return new List<HateoasLink> {
                hateoasLinkBuilder.BuildSelfLink(context, "HateoasRoute", "person"),
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "list", HttpMethod.Get, Parameters.GetValueOrDefault("Id") ?? ((Person)item).Id),    // custom rulesets can access the ID of an object via the last field
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "edit", HttpMethod.Post).AddHreflang("be-nl").AddMedia("doctype/jpg").AddTitle("Photo of a duck").AddType("image"),
                hateoasLinkBuilder.Build(context, "HateoasRoute", "person", "delete", HttpMethod.Delete),
            };
        }


    }
}
