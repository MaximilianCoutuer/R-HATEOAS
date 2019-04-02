using Microsoft.AspNetCore.Mvc;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Builders
{
    public sealed class HATEOASLinkObjectBuilder
    {

        private readonly IUrlHelper _urlHelper;

        public HATEOASLinkObjectBuilder(IUrlHelper urlHelper) {
            _urlHelper = urlHelper;
        }

        public HateoasLink Build(ActionContext response)
        {
            //IUrlHelper urlHelper = new urlHelper(response);   // DI not possible because the filter is an attribute
            var builtLink = response.HttpContext.Request.Host.ToUriComponent();
            builtLink += _urlHelper.RouteUrl("Testroute", new { controller = "person", id = 1 });

            return new HateoasLink("Testlink", "Testlink", HttpMethod.Get);
        }

    }
}
