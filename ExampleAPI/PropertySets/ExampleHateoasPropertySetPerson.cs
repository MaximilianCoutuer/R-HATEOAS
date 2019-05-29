using RDHATEOAS.PropertySets;
using RDHATEOAS.Rulesets;
using System;
using System.Collections.Generic;

namespace ExampleAPI.PropertySets
{
    /// <summary>
    /// An example PropertySet that can be attached to an API method to indicate it
    /// should add links to any objects in the root of the returned object hierarchy,
    /// using the ExampleRulesetFullLinksPerson ruleset.
    /// </summary>
    public class ExampleHateoasPropertySetPerson : IHateoasPropertySet
    {
        public Type Ruleset { get; set; } = typeof(ExampleRulesetFullLinksPerson);
        public List<string> Path { get; set; } = new List<string>();
        public List<string> Parameters { get; set; } = new List<string>() { "skip", "take" };
    }
}
