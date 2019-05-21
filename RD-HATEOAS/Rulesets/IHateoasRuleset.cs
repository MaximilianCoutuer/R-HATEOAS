using Microsoft.AspNetCore.Mvc.Filters;
using RDHATEOAS.Models;
using System.Collections.Generic;

namespace RDHATEOAS.Rulesets
{
    /// <summary>
    /// The interface that defines a ruleset.
    /// Individual rulesets should inherit from HateoasRulesetBase instead.
    /// </summary>
    public interface IHateoasRuleset<T> where T : IIsHateoasEnabled
    {
        /// <summary>
        /// Gets or sets the parameters from the method attribute.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this ruleset will apply to each item in a list or to the list itself.
        /// </summary>
        bool AppliesToEachListItem { get; set; }

        void SetHelpers(ResultExecutingContext context);

        List<HateoasLink> GetLinks(T item);
    }
}
