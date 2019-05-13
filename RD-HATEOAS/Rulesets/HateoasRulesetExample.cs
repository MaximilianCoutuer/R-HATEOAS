using RDHATEOAS.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace RDHATEOAS.Rulesets
{
    public class HateoasRulesetExample : HateoasRulesetBase
    {
        public override bool AppliesToEachListItem { get; set; } = true;

        public  override List<HateoasLink> GetLinks(IIsHateoasEnabled item) {
            var test = Parameters;
            return new List<HateoasLink> {
                hateoasLinkBuilder.BuildSelfLink(context, "default", "person"),
                hateoasLinkBuilder.Build(context, "default", "person", "list", HttpMethod.Get, Parameters.GetValueOrDefault("Id")),
                hateoasLinkBuilder.Build(context, "default", "person", "edit", HttpMethod.Post).AddHreflang("be-nl").AddMedia("doctype/jpg").AddTitle("Photo of a duck").AddType("image"),
                hateoasLinkBuilder.Build(context, "default", "person", "delete", HttpMethod.Delete),
            };
        }


    }
}
