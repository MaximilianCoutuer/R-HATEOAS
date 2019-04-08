using Microsoft.AspNetCore.Mvc;
using RDHATEOAS.Models;
using RDHATEOAS.Rulesets;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Builders
{
    public sealed class HateoasLinkBuilder
    {
        private readonly IUrlHelper _urlHelper;

        public HateoasLinkBuilder(IUrlHelper urlHelper) {
            _urlHelper = urlHelper;
        }

        public HateoasLink Build(ActionContext response, IHateoasRuleset ruleset)
        {
            var uri = response.HttpContext.Request.Host.ToUriComponent()
                + _urlHelper.RouteUrl("Testroute", new {
                    controller = "person",
                    id = 1      // TODO: softcode
                });
            var rel = "next";   // TODO: softcode
            var httpMethod = HttpMethod.Get;    // TODO: softcode
            var hateoasLink = new HateoasLink(uri, rel, HttpMethod.Get);

            // TODO: other params

            return hateoasLink;
        }

    }
}
