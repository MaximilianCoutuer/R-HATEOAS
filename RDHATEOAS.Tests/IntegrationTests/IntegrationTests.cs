using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RDHATEOAS.Tests.Integration
{
    //public class IntegrationTests : IClassFixture<WebApplicationFactory<RazorPagesProject.Startup>>
    //{
    //    private readonly WebApplicationFactory<RazorPagesProject.Startup> _factory;

    //    public BasicTests(WebApplicationFactory<RazorPagesProject.Startup> factory)
    //    {
    //        _factory = factory;
    //    }

    //    [Theory]
    //    [InlineData("/")]
    //    [InlineData("/Index")]
    //    [InlineData("/About")]
    //    [InlineData("/Privacy")]
    //    [InlineData("/Contact")]
    //    public async void GetEndpoints_Success(string url)
    //    {
    //        // arrange
    //        var client = _factory.CreateClient();

    //        // act
    //        var response = await client.GetAsync(url);

    //        // assert
    //        response.EnsureSuccessStatusCode(); // Status Code 200-299
    //        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
    //    }
    //}
}
