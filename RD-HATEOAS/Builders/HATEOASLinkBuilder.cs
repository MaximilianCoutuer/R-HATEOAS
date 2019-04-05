using Microsoft.AspNetCore.Mvc;
using RDHATEOAS.Models;
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
        public HateoasLink Build(ActionContext response)
        {
            var uri = response.HttpContext.Request.Host.ToUriComponent()
                + _urlHelper.RouteUrl("Testroute", new {
                    controller = "person",
                    id = 1
                });
            var rel = "next";
            // TODO: fill out rel, etc based on response
            return new HateoasLink(uri, rel, HttpMethod.Get);
        }

    }
}
