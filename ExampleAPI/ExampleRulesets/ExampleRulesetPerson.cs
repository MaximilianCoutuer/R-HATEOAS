using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using RDHATEOAS.Models;

namespace RDHATEOAS.Rulesets
{
    /// <summary>
    /// An example Ruleset that adds a number of relevant links to a Person entity:
    /// <list type="bullet">
    /// <item>
    /// <term>Self link</term>
    /// <description>A link to self. This uses the BuildSelfLink shorthand.</description>
    /// </item>
    /// <item>
    /// <term>Index</term>
    /// <description>A link to the list of Persons.</description>
    /// </item>
    /// <item>
    /// <term>Edit</term>
    /// <description>A link to Edit this Person. Note the use of item["Id"] to add its ID to the URL.</description>
    /// </item>
    /// <item>
    /// <term>Delete</term>
    /// <description>A link to Delete this Person. Note the use of item["Id"] to add its ID to the URL.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class ExampleRulesetPerson : HateoasRulesetBase
    {
        public override bool AppliesToEachListItem { get; set; } = true;

        public override List<HateoasLink> GetLinks(JToken item)
        {
            return new List<HateoasLink>
            {
                HateoasLinkBuilder.BuildSelfLink(Context)
                    .AddHreflang("be-nl")
                    .AddTitle("Link to self")
                    .AddType("json"),
                HateoasLinkBuilder.Build(Context, "default", "Person", string.Empty, "list", HttpMethod.Get)
                    .AddHreflang("be-nl")
                    .AddTitle("List of persons")
                    .AddType("json"),
                HateoasLinkBuilder.Build(Context, "default", "Person", "Edit", "edit", HttpMethod.Post, item["Id"])
                    .AddHreflang("be-nl")
                    .AddTitle("Edit this person")
                    .AddType("json"),
                HateoasLinkBuilder.Build(Context, "default", "Person", "Delete", "delete", HttpMethod.Delete, item["Id"])
                    .AddHreflang("be-nl")
                    .AddTitle("Delete this person")
                    .AddType("json"),
            };
            // Tip: if the API method returns a List<Person>, one could access the ID of the Person in said list via:
            // Parameters.GetValueOrDefault("RD-ListId");
        }
    }
}
