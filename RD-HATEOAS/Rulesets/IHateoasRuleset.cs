using Microsoft.AspNetCore.Mvc.Filters;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Rulesets
{
    public interface IHateoasRuleset
    {
        // generates a link

        void AddDescribedLink(ref IsHateoasEnabled item, ResultExecutingContext context, dynamic data);
        // ienumerable first()

    }
}
