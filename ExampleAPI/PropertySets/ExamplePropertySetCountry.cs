using RDHATEOAS.PropertySets;
using RDHATEOAS.Rulesets;
using System;
using System.Collections.Generic;

namespace ExampleAPI.PropertySets
{
    /// <summary>
    /// An example PropertySet that can be attached to an API method to indicate it
    /// should add links to any objects under the "Country" key of any objects in the root
    /// of the object hierarchy, using the ExampleRulesetCountry ruleset.
    /// </summary>
    public class ExamplePropertySetCountry : IHateoasPropertySet
    {
        public Type Ruleset { get; set; } = typeof(ExampleRulesetCountry);
        public List<string> Path { get; set; } = new List<string>() { "Country" };
        public List<string> Parameters { get; set; } = new List<string>() { "skip", "take" };
    }
}
