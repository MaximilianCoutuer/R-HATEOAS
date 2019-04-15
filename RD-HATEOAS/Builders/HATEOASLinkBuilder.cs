using Microsoft.AspNetCore.Mvc;
using RDHATEOAS.Models;
using RDHATEOAS.Rulesets;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Builders
{
    //
    //
    public sealed class HateoasLinkBuilder
    {
        private IUrlHelper _urlHelper;

        public HateoasLinkBuilder(IUrlHelper urlHelper) {
            _urlHelper = urlHelper;
        }

        // <summary>
        //
        // </summary>
        public HateoasLink Build(ActionContext response, string routeUrl, string linkController, string linkRel, HttpMethod linkMethod, Object linkId = null)
        {
            int id;
            var uri = response.HttpContext.Request.Host.ToUriComponent()
                + _urlHelper.RouteUrl(routeUrl, new {
                    controller = linkController,
                    id = (int.TryParse((string)(linkId ?? ""), out id) ? id : default(int?))
                });
            var hateoasLink = new HateoasLink(uri, linkRel, linkMethod);
            return hateoasLink;
        }

    }
}
