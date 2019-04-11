using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace RDHATEOAS.Rulesets
{
    public interface IHateoasRuleset
    {
        // generates a link
        object Parameter { get; set; }
        void AddDescribedLink(ref IDictionary<string, Object> item, ResultExecutingContext context, dynamic data);
        // ienumerable first()

    }
}
