namespace RDHATEOAS.Rulesets
{
    using System.Collections.Generic;
    using System.Net.Http;
    using RDHATEOAS.Models;

    public class HateoasRulesetExample : HateoasRulesetBase
    {
        public override bool AppliesToEachListItem { get; set; } = true;

        public override List<HateoasLink> GetLinks(IIsHateoasEnabled item)
        {
            var test = Parameters;
            return new List<HateoasLink>
            {
                HateoasLinkBuilder.BuildSelfLink(Context),
                HateoasLinkBuilder.Build(Context, "default", "person", null, "list", HttpMethod.Get, Parameters.GetValueOrDefault("Id")),
                HateoasLinkBuilder.Build(Context, "default", "person", null, "edit", HttpMethod.Post).AddHreflang("be-nl").AddMedia("doctype/jpg").AddTitle("Photo of a duck").AddType("image"),
                HateoasLinkBuilder.Build(Context, "default", "person", null, "delete", HttpMethod.Delete),
            };
        }
    }
}
