using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RDHATEOAS.Models;

namespace RDHATEOAS.Rulesets
{
    /// <summary>
    /// An example Ruleset that adds a self link.
    /// <list type="bullet">
    /// <item>
    /// <term>Self link</term>
    /// <description>A link to self. This uses the BuildSelfLink shorthand.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class ExampleRulesetSelf : HateoasRulesetBase
    {
        public override bool AppliesToEachListItem { get; set; } = false;   // this prevents it from applying to every item

        public override List<HateoasLink> GetLinks(JToken item)
        {
            return new List<HateoasLink>
            {
                HateoasLinkBuilder.BuildSelfLink(Context)
                    .AddHreflang("be-nl")
                    .AddTitle("Link to self")
                    .AddType("json"),
            };
        }
    }
}
