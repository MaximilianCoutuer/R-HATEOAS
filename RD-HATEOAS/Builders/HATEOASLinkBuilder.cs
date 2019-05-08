using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RDHATEOAS.Models;
using RDHATEOAS.Rulesets;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Builders
{
    /// <summary>
    /// This class is used to build a HATEOAS link.
    /// </summary>
    public sealed class HateoasLinkBuilder
    {
        #region fields

        private IUrlHelper _urlHelper;

        public HateoasLinkBuilder(IUrlHelper urlHelper) {
            _urlHelper = urlHelper;
        }

        #endregion

        #region methods

        /// <summary>
        /// Build a HATEOAS link (type HateoasLink) based on the provided parameters.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="routeUrl"></param>
        /// <param name="linkController"></param>
        /// <param name="linkRel"></param>
        /// <param name="linkMethod"></param>
        /// <param name="linkId"></param>
        /// <returns>A HATEOAS link object.</returns>
        public HateoasLink Build(ActionContext response, string routeUrl, string linkController, string linkRel, HttpMethod linkMethod, Object linkId = null)
        {
            if (routeUrl == null || linkController == null || linkRel == null || linkMethod == null)
            {
                throw new ArgumentNullException();
            }
            var uri = response.HttpContext.Request.Host.ToUriComponent() ?? "localhost"
                + _urlHelper.RouteUrl(routeUrl, new
                {
                    controller = linkController,
                    id = (int.TryParse((linkId ?? "").ToString(), out int id) ? id : default(int?))
                });
            var hateoasLink = new HateoasLink(uri, linkRel, linkMethod);
            return hateoasLink;
        }

        /// <summary>
        /// Build a HATEOAS link (type HateoasLink) based on the provided parameters.
        /// However, this is always a self link.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="routeUrl"></param>
        /// <param name="linkController"></param>
        /// <returns></returns>
        public HateoasLink BuildSelfLink(ActionContext response, string routeUrl, string linkController)
        {
            var request = response.HttpContext.Request;
            var uri = (new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port.GetValueOrDefault(80), request.Path.ToString(), request.QueryString.ToString())).Uri.ToString();
            var hateoasLink = new HateoasLink(uri, "self", HttpMethod.Get);
            return hateoasLink;
        }

        #endregion

    }
}
