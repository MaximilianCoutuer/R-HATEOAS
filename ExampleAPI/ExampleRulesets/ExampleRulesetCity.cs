using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Rhateoas.Models;

namespace Rhateoas.Rulesets
{
    /// <summary>
    /// An example Ruleset that adds a number of relevant links to a City entity.
    /// As there is no City controller, we pretend for educational purposes that it uses query string parameters.
    /// <list type="bullet">
    /// <item>
    /// <term>Index</term>
    /// <description>A link to the list of Cities.</description>
    /// </item>
    /// <item>
    /// <term>Edit</term>
    /// <description>A Details link to this City. Note the use of ExtendQueryString() and item["Name"].</description>
    /// </item>
    /// </list>
    /// </summary>
    public class ExampleRulesetCity : HateoasRulesetBase
    {
        public override bool AppliesToEachListItem { get; set; } = true;

        public override List<HateoasLink> GetLinks(JToken item)
        {
            return new List<HateoasLink>
            {
                HateoasLinkBuilder.Build(Context, "default", "City", "List", "list", HttpMethod.Get)
                    .AddHreflang("nl-be")
                    .AddTitle("List of cities")
                    .AddType("application/json+hal"),
                HateoasLinkBuilder.Build(Context, "default", "City", "Details", "details", HttpMethod.Get)
                    .AddHreflang("nl-be")
                    .AddTitle("Details of this city")
                    .AddType("application/json+hal")
                    .ExtendQueryString("name", item["Name"].ToString()),
            };
        }
    }
}
