using System;
using System.Collections.Generic;

namespace Rdhateoas.PropertySets
{
    public interface IHateoasPropertySet
    {
        Type Ruleset { get; set; }

        List<string> Path { get; set; }

        List<string> Parameters { get; set; }
    }
}
