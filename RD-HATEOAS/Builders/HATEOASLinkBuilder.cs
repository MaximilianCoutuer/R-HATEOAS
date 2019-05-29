using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Rdhateoas.Models;

namespace Rdhateoas.Builders
{
    /// <summary>
    /// This class is used to build a HATEOAS link.
    /// </summary>
    public sealed class HateoasLinkBuilder
    {
        #region fields

        private IUrlHelper _urlHelper;

        public HateoasLinkBuilder(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        #endregion

        #region methods

        /// <summary>
        /// Builds a HATEOAS link based on the provided parameters.
        /// </summary>
        /// <param name="response">The response for which links are generated.</param>
        /// <param name="routeUrl">The name of the route map.</param>
        /// <param name="routeUrlController">The name of the controller that handles the link.</param>
        /// <param name="routeUrlAction">The name of the action that handles the link.</param>
        /// <param name="linkRel">The link rel property.</param>
        /// <param name="linkMethod">The link HTTP method.</param>
        /// <param name="linkId">If the link has an ID number, use this.</param>
        /// <returns>A HATEOAS link object.</returns>
        public HateoasLink Build(ActionContext response, string routeUrl, string routeUrlController, string routeUrlAction, string linkRel, HttpMethod linkMethod, object linkId = null)
        {
            if (routeUrl == null || routeUrlController == null || routeUrlAction == null || linkRel == null || linkMethod == null)
            {
                throw new ArgumentNullException();
            }

            var uri = (response.HttpContext.Request.Host.Value == null ? "Localhost" : response.HttpContext.Request.Host.ToUriComponent())
                + _urlHelper.RouteUrl(routeUrl, new
                {
                    controller = routeUrlController,
                    action = routeUrlAction,
                    id = int.TryParse((linkId ?? string.Empty).ToString(), out int id) ? id : default(int?),
                });
            var hateoasLink = new HateoasLink(uri, linkRel, linkMethod);
            return hateoasLink;
        }

        /// <summary>
        /// Builds a HATEOAS self link based on the provided parameters.
        /// </summary>
        /// <remarks>
        /// This is based on the request URL.
        /// According to HATEOAS specs, only the top level item can have a self link.
        /// </remarks>
        /// <param name="response">The response for which links are generated.</param>
        /// <returns>A HATEOAS link object.</returns>
        public HateoasLink BuildSelfLink(ActionContext response)
        {
            var request = response.HttpContext.Request;
            var uri = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port.GetValueOrDefault(80), request.Path.ToString(), request.QueryString.ToString()).Uri.ToString();
            var hateoasLink = new HateoasLink(uri, "self", HttpMethod.Get);
            return hateoasLink;
        }

        #endregion

    }
}
