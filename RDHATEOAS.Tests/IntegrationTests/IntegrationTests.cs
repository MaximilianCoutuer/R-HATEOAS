using ExampleAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using RDHATEOAS.Models;
using RDHATEOAS.Tests.Factories;
using RDHATEOAS.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RDHATEOAS.Tests.IntegrationTests
{
    public class IntegrationTests : IClassFixture<CustomAPIFactory>
    {
        private readonly CustomAPIFactory _factory;

        public IntegrationTests(CustomAPIFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/person")]
        public async void GetAllPersons_ShouldReturnPersonsAsync(string url)
        {
            // arrange
            var httpClient = _factory.CreateClient();
            var uri = new Uri("https://localhost:44356/api/person");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var person = new Person();

            // act
            var httpResponseMessage = await httpClient.GetAsync(uri);
            var value = await httpResponseMessage.Content.ReadAsAsync<Person>();

            // assert
            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.True(value is Person);   // this won't work but it'll tell me what it actually is
        }

        [Theory]
        [InlineData("/api/person/1")]
        public async void GetPerson_ShouldReturnPersonAsync(string url)
        {
            // arrange
            var httpClient = _factory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var person = new Person();
            person.FirstName = "Test";
            person.LastName = "Test";
            var postContent = new ObjectContent(typeof(Person), person, new JsonMediaTypeFormatter());

            // act
            // TODO: is added
            var postResponseMessage = await httpClient.PostAsync("api/person", postContent); // TODO: Bad request
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var value = await httpResponseMessage.Content.ReadAsAsync<Person>();

            // assert
            Assert.Equal(HttpStatusCode.Created, postResponseMessage.StatusCode);
            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.True(value is Person);
        }

        public bool ContainsHateoasLink(string value, HateoasLink link)
        {
            // TODO: parse

            return false;
        }
    }
}
