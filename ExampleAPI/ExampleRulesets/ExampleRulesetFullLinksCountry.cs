namespace RDHATEOAS.Rulesets
{
    using System.Collections.Generic;
    using System.Net.Http;
    using ExampleAPI.Models;
    using RDHATEOAS.Models;

    public class ExampleRulesetFullLinksCountry : HateoasRulesetBase
    {
        public override bool AppliesToEachListItem { get; set; } = true;

        public override List<HateoasLink> GetLinks(IIsHateoasEnabled item)
        {
            return new List<HateoasLink>
            {
                HateoasLinkBuilder.BuildSelfLink(Context)
                    .AddHreflang("be-nl")
                    .AddTitle("Properties of this requested country")
                    .AddType("json"),
                HateoasLinkBuilder.Build(Context, "default", "Country", string.Empty, "list", HttpMethod.Get)
                    .AddHreflang("be-nl")
                    .AddTitle("List of persons")
                    .AddType("json"),
                HateoasLinkBuilder.Build(Context, "default", "Country", "Edit", "edit", HttpMethod.Post, ((Country)item).Id)
                    .AddHreflang("be-nl")
                    .AddTitle("Edit this person")
                    .AddType("json"),
                HateoasLinkBuilder.Build(Context, "default", "Country", "Delete", "delete", HttpMethod.Delete)
                    .AddHreflang("be-nl")
                    .AddTitle("Delete this person")
                    .AddType("json")
                    .ExtendQueryString("id", ((Country)item).Id.ToString()),
            };
            // Parameters.GetValueOrDefault("RD-ListId")
        }
    }
}
