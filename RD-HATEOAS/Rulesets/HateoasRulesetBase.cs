﻿using Microsoft.AspNetCore.Mvc.Filters;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Rulesets
{
    public abstract class HateoasRulesetBase : IHateoasRuleset
    {

        public HateoasRulesetBase() { }

        public void AddDescribedLink(ref IsHateoasEnabled item, ResultExecutingContext context, dynamic data) {
            item._links = AddLinkObjectToRef(item, context, data);
        }

        protected virtual HateoasLink[] AddLinkObjectToRef(IsHateoasEnabled item, ResultExecutingContext context, dynamic data)
        {
            // default null implementation
            return null;
        }
    }
}