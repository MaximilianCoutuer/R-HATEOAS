using Rhateoas.PropertySets;
using Rhateoas.Rulesets;
using System;
using System.Collections.Generic;

namespace ExampleAPI.PropertySets
{
    /// <summary>
    /// An example PropertySet that can be attached to an API method to indicate it
    /// should add a self link, using the ExampleRulesetSelf ruleset.
    /// </summary>
    public class ExamplePropertySetSelf : IHateoasPropertySet
    {
        public Type Ruleset { get; set; } = typeof(ExampleRulesetSelf);
        public List<string> Path { get; set; } = new List<string>();
        public List<string> Parameters { get; set; } = new List<string>() { "skip", "take" };
    }
}
