using RDHATEOAS.PropertySets;
using RDHATEOAS.Rulesets;
using System;
using System.Collections.Generic;

namespace ExampleAPI.PropertySets
{
    public class ExampleHateoasPropertySet : IHateoasPropertySet
    {
        public Type Ruleset { get; set; } = typeof(ExampleRulesetFullLinksCountry);
        public List<string> Path { get; set; } = new List<string>() { "Country" };
        public List<string> Parameters { get; set; } = new List<string>() { "skip", "take" };
    }
}
