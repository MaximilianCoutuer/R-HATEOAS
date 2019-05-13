using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using RDHATEOAS.Builders;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xunit;

namespace RDHATEOAS.Tests.UnitTests.Builders
{
    public class LinkBuilderFixture : IDisposable
    {
        public readonly HateoasLinkBuilder _linkBuilder;
        public readonly ActionContext _mockContext;
        public readonly UrlHelper _urlHelper;
        public readonly RouteData _routeData;

        public LinkBuilderFixture()
        {
            // "We all know how painful it is to mock a HttpContext"
            _mockContext = new ActionContext();
            _mockContext.HttpContext = new DefaultHttpContext();
            _routeData = new RouteData();
            _mockContext.RouteData = _routeData;
            _urlHelper = new UrlHelper(_mockContext);
            _linkBuilder = new HateoasLinkBuilder(_urlHelper);
        }

        public void Dispose()
        {
        }
    }

    public class HateoasLinkBuilderTests : IClassFixture<LinkBuilderFixture>
    {
        LinkBuilderFixture _fixture;

        public HateoasLinkBuilderTests(LinkBuilderFixture fixture)
        {
            this._fixture = fixture;
        }

        [Theory]
        [MemberData(nameof(ValidLinkData))]
        public void BuildLink_ShouldBuildLink(string routeUrl, string linkController, string linkAction, string linkRef, HttpMethod linkMethod, int linkId)
        {
            // arrange

            // act
            HateoasLink link = _fixture._linkBuilder.Build(_fixture._mockContext, routeUrl, linkController, linkAction, linkRef, linkMethod, linkId);

            // assert
            //Uri uriResult;
            //bool refIsLink = Uri.TryCreate(link.Href, UriKind.RelativeOrAbsolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;

            Assert.True(Uri.IsWellFormedUriString(link.Href, UriKind.RelativeOrAbsolute));
            Assert.Equal(link.Method, linkMethod.Method);
        }

        [Theory]
        [MemberData(nameof(NullLinkData))]
        public void BuildLink_NullData_ShouldThrow(string routeUrl, string linkController, string linkAction, string linkRef, HttpMethod linkMethod, int linkId)
        {
            // arrange

            // act

            // assert
            Assert.Throws<ArgumentNullException>(() => _fixture._linkBuilder.Build(_fixture._mockContext, routeUrl, linkController, linkAction, linkRef, linkMethod, linkId));
        }

        #region helpers

        public static IEnumerable<Object[]> ValidLinkData
        {
            get
            {
                return new[]
                {
                    new Object[] { "default", "person", null, "list", HttpMethod.Get, 5 },
                    new Object[] { "default", "person", null, "list", HttpMethod.Get, null },
            };
            }
        }

        public static IEnumerable<Object[]> NullLinkData
        {
            get
            {
                return new[]
                {
                    new Object[] { null, "person", null, "list", HttpMethod.Get, 5 },
                    new Object[] { "default", null, null, "list", HttpMethod.Get, 5 },
                    new Object[] { "default", "person", null,  null, HttpMethod.Get, 5 },
                    new Object[] { "default", "person", null, "list", null, 5 },
            };
            }
        }

        #endregion
    }
}
