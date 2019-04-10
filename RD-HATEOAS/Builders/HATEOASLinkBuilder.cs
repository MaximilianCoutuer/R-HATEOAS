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

        public HateoasLink Build(ActionContext response, string routeUrl, string linkController, string linkRel, HttpMethod linkMethod, int? linkId = null, string linkHreflang = null, string linkMedia = null, string linkTitle = null, string linkType = null)
        {
            var uri = response.HttpContext.Request.Host.ToUriComponent()
                + _urlHelper.RouteUrl(routeUrl, new {
                    controller = linkController,
                    id = linkId
                });
            var rel = linkRel;
            var hateoasLink = new HateoasLink(uri, linkRel, linkMethod)
            {
                Hreflang = linkHreflang,
                Media = linkMedia,
                Title = linkTitle,
                Type = linkType
            };

            // TODO: other params if relevant

            return hateoasLink;
        }

    }
}
