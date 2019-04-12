using Microsoft.AspNetCore.Mvc.Filters;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;

namespace RDHATEOAS.Rulesets
{
    /// <summary>
    /// The interface that defines a ruleset.
    /// Individual rulesets should inherit from HateoasRulesetBase instead.
    /// </summary>
    public interface IHateoasRuleset
    {
        object Parameter { get; set; }
        void AddLinksToRef(ref IsHateoasEnabled item, ResultExecutingContext context);
    }
}
