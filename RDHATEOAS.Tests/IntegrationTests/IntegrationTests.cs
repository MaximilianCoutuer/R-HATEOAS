using ExampleAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using RDHATEOAS.Models;
using RDHATEOAS.Tests.Factories;
using RDHATEOAS.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RDHATEOAS.Tests.IntegrationTests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<TestStartup>>
    {
        private readonly WebApplicationFactory<TestStartup> _factory;

        public IntegrationTests(WebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/person/5")]
        public async void GetPerson_ShouldReturnPersonAsync(string url)
        {
            // arrange
            var httpClient = _factory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            // act
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var value = await httpResponseMessage.Content.ReadAsAsync<Person>();

            // assert
            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.True(value is Person);   // this won't work but it'll tell me what it actually is
        }

        public bool ContainsHateoasLink(string value, HateoasLink link)
        {
            // TODO: parse

            return false;
        }
    }
}
