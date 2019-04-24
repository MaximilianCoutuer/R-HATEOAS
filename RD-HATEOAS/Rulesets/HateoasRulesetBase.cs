using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Builders;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;

namespace RDHATEOAS.Rulesets
{
    /// <summary>
    /// The base class for a HATEOAS ruleset.
    /// Rulesets determine which links should be added to a HATEOAS enabled object.
    /// </summary>
    public abstract class HateoasRulesetBase : IHateoasRuleset
    {
        public Dictionary<string, Object> Parameters { get; set; }
        protected UrlHelper urlHelper { get; set; }
        protected ResultExecutingContext context { get; set; }
        protected HateoasLinkBuilder hateoasLinkBuilder { get; set; }
        public virtual bool AppliesToEachListItem { get; set; } = false;


        public void SetHelpers(ResultExecutingContext context)
        {
            this.context = context;
            urlHelper = new UrlHelper(context);
            hateoasLinkBuilder = new HateoasLinkBuilder(urlHelper);
        }

        //public IsHateoasEnabled AddLinksToItem(IsHateoasEnabled item, ResultExecutingContext context) {
        //    _urlHelper = new UrlHelper(context);
        //    hateoasLinkBuilder = new HateoasLinkBuilder(_urlHelper);

        //    var links = GetLinks(item, context);
        //    foreach (HateoasLink link in links)
        //    {
        //        item.Links.Add(link);
        //    }
        //    return item;
        //}

        public virtual List<HateoasLink> GetLinks(IIsHateoasEnabled item)
        {
            // default null implementation yields no links
            return null;
        }
    }
}
