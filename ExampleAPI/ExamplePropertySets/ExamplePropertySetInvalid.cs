using Rdhateoas.PropertySets;
using Rdhateoas.Rulesets;
using System;
using System.Collections.Generic;

namespace ExampleAPI.PropertySets
{
    /// <summary>
    /// <para>An example PropertySet that can be attached to an API method to indicate it
    /// should add links to any objects under the "InvalidKey" key of any objects under the "Country"
    /// key of any objects in the root of the returned object hierarchy,
    /// using the ExampleRulesetCountry ruleset.</para>
    /// <para>This is an example of a worst case scenario where neither the path nor the parameters
    /// are valid.</para>
    /// </summary>
    public class ExamplePropertySetInvalid : IHateoasPropertySet
    {
        public Type Ruleset { get; set; } = typeof(ExampleRulesetCountry);
        public List<string> Path { get; set; } = new List<string>() { "Country", "InvalidKey" };
        public List<string> Parameters { get; set; } = new List<string>() { "invalidQueryParameter" };
    }
}
