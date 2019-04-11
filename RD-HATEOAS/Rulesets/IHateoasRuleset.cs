using Microsoft.AspNetCore.Mvc.Filters;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;

namespace RDHATEOAS.Rulesets
{
    public interface IHateoasRuleset
    {
        // generates a link
        object Parameter { get; set; }
        void AddDescribedLink(ref IsHateoasEnabled item, ResultExecutingContext context, dynamic data);
        // ienumerable first()

    }
}
