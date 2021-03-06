﻿using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Rhateoas.Models;

namespace Rhateoas.Rulesets
{
    /// <summary>
    /// An example Ruleset that adds a number of relevant links to a Person entity:
    /// <list type="bullet">
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
                HateoasLinkBuilder.Build(Context, "default", "Person", string.Empty, "list", HttpMethod.Get)
                    .AddHreflang("nl-be")
                    .AddTitle("List of persons")
                    .AddType("application/json+hal"),
                HateoasLinkBuilder.Build(Context, "default", "Person", "Edit", "edit", HttpMethod.Post, item["Id"])
                    .AddHreflang("nl-be")
                    .AddTitle("Edit this person")
                    .AddType("application/json+hal"),
                HateoasLinkBuilder.Build(Context, "default", "Person", "Delete", "delete", HttpMethod.Delete, item["Id"])
                    .AddHreflang("nl-be")
                    .AddTitle("Delete this person")
                    .AddType("application/json+hal"),
            };
            // Tip: if the API method returns a List<Person>, one could access the ID of the Person in said list via:
            // Parameters.GetValueOrDefault("RD-ListId");
        }
    }
}
