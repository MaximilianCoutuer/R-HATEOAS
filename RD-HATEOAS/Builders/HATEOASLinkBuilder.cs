using Microsoft.AspNetCore.Mvc;
using RDHATEOAS.Models;
using System;
using System.Net.Http;

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
        /// Builds a HATEOAS link based on the provided parameters.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="routeUrl"></param>
        /// <param name="routeUrlController"></param>
        /// <param name="routeUrlAction"></param>
        /// <param name="linkRel"></param>
        /// <param name="linkMethod"></param>
        /// <param name="linkId"></param>
        /// <returns>A HATEOAS link object.</returns>
        public HateoasLink Build(ActionContext response, string routeUrl, string routeUrlController, string routeUrlAction, string linkRel, HttpMethod linkMethod, Object linkId = null)
        {
            if (routeUrl == null || routeUrlController == null || routeUrlAction == null || linkRel == null || linkMethod == null)
            {
                throw new ArgumentNullException();
            }

            var test = _urlHelper.RouteUrl("default", new { controller = "Person", action = "", id = "" });

            var uri = (response.HttpContext.Request.Host.ToUriComponent() ?? "localhost")
                + _urlHelper.RouteUrl(routeUrl, new
                {
                    controller = routeUrlController,
                    action = routeUrlAction,
                    id = (int.TryParse((linkId ?? "").ToString(), out int id) ? id : default(int?))
                });
            var hateoasLink = new HateoasLink(uri, linkRel, linkMethod);
            return hateoasLink;
        }

        /// <summary>
        /// Builds a HATEOAS self link based on the provided parameters.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="routeUrl"></param>
        /// <param name="linkController"></param>
        /// <returns>A HATEOAS link object.</returns>
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
