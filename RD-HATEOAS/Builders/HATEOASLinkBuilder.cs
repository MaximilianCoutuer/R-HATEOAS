using Microsoft.AspNetCore.Mvc;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Builders
{
    public sealed class HATEOASLinkBuilder
    {

        private readonly IUrlHelper _urlHelper;

        public HATEOASLinkBuilder(IUrlHelper urlHelper) {
            _urlHelper = urlHelper;
        }

        public HateoasLink Build(ActionContext response)
        {
            var builtLink = response.HttpContext.Request.Host.ToUriComponent();
            builtLink += _urlHelper.RouteUrl("Testroute", new { controller = "person", id = 1 });

            return new HateoasLink("Testlink", "Testlink", HttpMethod.Get);
        }

    }
}
