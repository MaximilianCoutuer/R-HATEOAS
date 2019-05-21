namespace RDHATEOAS.Rulesets
{
    using System.Collections.Generic;
    using System.Net.Http;
    using ExampleAPI.Models;
    using RDHATEOAS.Models;

    public class ExampleRulesetFullLinksCountry : HateoasRulesetBase<Country>
    {
        public override bool AppliesToEachListItem { get; set; } = true;

        public override List<HateoasLink> GetLinks(Country country)
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
                HateoasLinkBuilder.Build(Context, "default", "Country", "Edit", "edit", HttpMethod.Post, country.Id)
                    .AddHreflang("be-nl")
                    .AddTitle("Edit this person")
                    .AddType("json"),
                HateoasLinkBuilder.Build(Context, "default", "Country", "Delete", "delete", HttpMethod.Delete)
                    .AddHreflang("be-nl")
                    .AddTitle("Delete this person")
                    .AddType("json")
                    .ExtendQueryString("id", country.Id.ToString()),
            };
            // Parameters.GetValueOrDefault("RD-ListId")
        }
    }
}
