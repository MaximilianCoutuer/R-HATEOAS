using Rdhateoas.PropertySets;
using Rdhateoas.Rulesets;
using System;
using System.Collections.Generic;

namespace ExampleAPI.PropertySets
{
    /// <summary>
    /// <para>An example PropertySet that can be attached to an API method to indicate it
    /// should add links to any objects under the "Capital" key of any objects under the "Country"
    /// key of any objects in the root of the returned object hierarchy,
    /// using the ExampleRulesetCity ruleset.</para>
    /// <para>For demonstration purposes, an invalid query string parameter is added.</para>
    /// </summary>
    public class ExamplePropertySetCity : IHateoasPropertySet
    {
        public Type Ruleset { get; set; } = typeof(ExampleRulesetCity);
        public List<string> Path { get; set; } = new List<string>() { "Country", "Capital" };
        public List<string> Parameters { get; set; } = new List<string>() { "skip", "take", "invalid" };
    }
}
