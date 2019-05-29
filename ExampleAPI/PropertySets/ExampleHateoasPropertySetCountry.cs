using RDHATEOAS.PropertySets;
using RDHATEOAS.Rulesets;
using System;
using System.Collections.Generic;

namespace ExampleAPI.PropertySets
{
    /// <summary>
    /// An example PropertySet that can be attached to an API method to indicate it
    /// should add links to any objects under the "Country" key of any objects in the root
    /// of the object hierarchy, using the ExampleRulesetFullLinksCountry ruleset.
    /// </summary>
    public class ExampleHateoasPropertySetCountry : IHateoasPropertySet
    {
        public Type Ruleset { get; set; } = typeof(ExampleRulesetFullLinksCountry);
        public List<string> Path { get; set; } = new List<string>() { "Country" };
        public List<string> Parameters { get; set; } = new List<string>() { "skip", "take" };
    }
}
