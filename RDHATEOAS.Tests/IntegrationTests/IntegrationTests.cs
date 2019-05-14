namespace RDHATEOAS.Tests.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Text;
    using System.Threading.Tasks;
    using ExampleAPI.Models;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using RDHATEOAS.Models;
    using RDHATEOAS.Tests.Factories;
    using RDHATEOAS.Tests.Mocks;
    using Xunit;

    public class IntegrationTests : IClassFixture<CustomAPIFactory>
    {
        private readonly CustomAPIFactory _factory;

        public IntegrationTests(CustomAPIFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/person")]
        public async void GetAllPersons_ShouldReturnPersonListAsync(string url)
        {
            // arrange
            var httpClient = _factory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var person = new Person
            {
                FirstName = "Test",
                LastName = "Test"
            };
            var postContent = new ObjectContent(typeof(Person), person, new JsonMediaTypeFormatter());

            // act
            // TODO: is added
            var postResponseMessage = await httpClient.PostAsync("api/person", postContent); // TODO: Bad request
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            var test = await httpResponseMessage.Content.ReadAsAsync<PersonListResult>();
            var value = await httpResponseMessage.Content.ReadAsStringAsync();
            var valueScrubbed = value.Replace(@"\", "aaaaaa").Trim(new char[1] { '"' });
            var personList = JsonConvert.DeserializeObject<List<Person>>(valueScrubbed);

            // assert
            Assert.Equal(HttpStatusCode.Created, postResponseMessage.StatusCode);
            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.True(personList is List<Person>);
        }

        private class PersonListResult
        {
            public Person[] List { get; set; }
        }

        [Theory]
        [InlineData("/api/person/1")]
        public async void GetPerson_ShouldReturnPersonAsync(string url)
        {
            // arrange
            var httpClient = _factory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var person = new Person
            {
                FirstName = "Test",
                LastName = "Test"
            };
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
