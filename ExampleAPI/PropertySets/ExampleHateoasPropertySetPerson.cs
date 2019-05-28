using RDHATEOAS.PropertySets;
using RDHATEOAS.Rulesets;
using System;
using System.Collections.Generic;

namespace ExampleAPI.PropertySets
{
    public class ExampleHateoasPropertySetPerson : IHateoasPropertySet
    {
        public Type Ruleset { get; set; } = typeof(ExampleRulesetFullLinksPerson);
        public List<string> Path { get; set; } = new List<string>();
        public List<string> Parameters { get; set; } = new List<string>() { "skip", "take" };
    }
}
