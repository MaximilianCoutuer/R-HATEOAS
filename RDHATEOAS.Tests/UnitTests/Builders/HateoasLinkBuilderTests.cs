using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace RDHATEOAS.Tests.Builders
{
    public class LinkBuilderFixture : IDisposable
    {
        public readonly HateoasLinkBuilder _linkBuilder;
        public readonly ActionContext _mockContext;
        public readonly UrlHelper _urlHelper;

        public LinkBuilderFixture()
        {
            // "We all know how painful it is to mock a HttpContext"
            _mockContext = new ActionContext();
            _mockContext.HttpContext = new DefaultHttpContext();
            //mockContext.Response.Body = new MemoryStream();
            //mockContext.Response.Body.Seek(0, SeekOrigin.Begin);

            _urlHelper = new UrlHelper(this._mockContext);
            _linkBuilder = new HateoasLinkBuilder(_urlHelper);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }


    public class HateoasLinkBuilderTests : IClassFixture<LinkBuilderFixture>
    {
        [Fact]
        public async void BuildLink_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildLink_InvalidUrlHelper_ShouldThrow()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildLink_InvalidContext_ShouldThrow()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildLink_NullResponse_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildLink_NullRouteUrl_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildLink_NullLinkController_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildLink_NullLinkRel_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildLink_InvalidLinkMethod_ShouldThrow()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildLink_NullLinkId_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildSelfLink_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildSelfLink_NullResponse_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildSelfLink_NullRouteUrl_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }

        [Fact]
        public async void BuildSelfLink_NullLinkController_ShouldBuildLink()
        {
            Assert.NotNull(null);
        }
    }
}
