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
        Dictionary<string, Object> Parameters { get; set; }

        /// <summary>
        /// Determines whether this ruleset will apply to each item in a list or to the list itself.
        /// </summary>
        bool AppliesToEachListItem { get; set; }

        void AddLinksToRef(ref IsHateoasEnabled item, ResultExecutingContext context);
    }
}
