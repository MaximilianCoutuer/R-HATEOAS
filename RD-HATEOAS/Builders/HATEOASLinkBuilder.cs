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

        public HateoasLink Build(ActionContext response, string routeUrl, string linkController, int linkId, string linkRel, HttpMethod linkMethod)
        {
            var uri = response.HttpContext.Request.Host.ToUriComponent()
                + _urlHelper.RouteUrl(routeUrl, new {
                    controller = linkController,
                    id = linkId
                });
            var rel = linkRel;
            var httpMethod = linkMethod;
            var hateoasLink = new HateoasLink(uri, rel, HttpMethod.Get);

            // TODO: other params if relevant

            return hateoasLink;
        }

    }
}
