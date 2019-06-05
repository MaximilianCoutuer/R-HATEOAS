using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using ExampleAPI.Models;
using Rhateoas.Models;
using Rhateoas.Tests.Factories;
using Xunit;

namespace Rhateoas.Tests.IntegrationTests
{
    public class IntegrationTests : IClassFixture<CustomAPIFactory>
    {
        //private readonly CustomAPIFactory _factory;

        //public IntegrationTests(CustomAPIFactory factory)
        //{
        //    _factory = factory;
        //}

        //[Theory]
        //[InlineData("/api/person")]
        //public async void GetAllPersons_ShouldReturnPersonListAsync(string url)
        //{
        //    // arrange
        //    var httpClient = _factory.CreateClient();
        //    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        //    var person = new Person
        //    {
        //        FirstName = "Test",
        //        LastName = "Test"
        //    };
        //    var postContent = new ObjectContent(typeof(Person), person, new JsonMediaTypeFormatter());

        //    // act
        //    var postResponseMessage = await httpClient.PostAsync("api/person", postContent); // TODO: Bad request
        //    var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        //    var value = await httpResponseMessage.Content.ReadAsAsync<PersonListResult>();

        //    // assert
        //    // test if POST and GET are successful
        //    Assert.Equal(HttpStatusCode.Created, postResponseMessage.StatusCode);
        //    httpResponseMessage.EnsureSuccessStatusCode();

        //    // test if value is the correct person
        //    Assert.NotNull(value);
        //    var personName = value.List[0].LastName;
        //    Assert.Equal("Test", personName);

        //    // test if value contains the correct links
        //    var personLinks = (value.List[0] as IIsHateoasEnabled).Links;
        //    Assert.NotNull(personLinks);
        //    Assert.Equal(4, personLinks.Count);

        //    // test if the list itself contains the correct links
        //    var listLinks = (value as ListHateoasEnabled).Links;
        //    Assert.NotNull(listLinks);
        //}

        //private class PersonListResult : ListHateoasEnabled
        //{
        //    public Person[] List { get; set; }
        //}

        //[Theory]
        //[InlineData("/api/person/1")]
        //public async void GetPerson_ShouldReturnPersonAsync(string url)
        //{
        //    // arrange
        //    var httpClient = _factory.CreateClient();
        //    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        //    var person = new Person
        //    {
        //        FirstName = "Test",
        //        LastName = "Test"
        //    };
        //    var postContent = new ObjectContent(typeof(Person), person, new JsonMediaTypeFormatter());

        //    // act
        //    var postResponseMessage = await httpClient.PostAsync("api/person", postContent); // TODO: Bad request
        //    var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        //    var value = await httpResponseMessage.Content.ReadAsAsync<Person>();

        //    // assert
        //    // test if POST and GET are successful
        //    Assert.Equal(HttpStatusCode.Created, postResponseMessage.StatusCode);
        //    httpResponseMessage.EnsureSuccessStatusCode();

        //    // test if value is the correct person
        //    Assert.NotNull(value);
        //    var personName = value.LastName;
        //    Assert.Equal("Test", personName);

        //    // test if value contains the correct links
        //    var personLinks = (value as IIsHateoasEnabled).Links;
        //    Assert.NotNull(personLinks);
        //    Assert.Equal(4, personLinks.Count);
        //}

        //public bool ContainsHateoasLink(string value, HateoasLink link)
        //{
        //    // TODO: parse
        //    return false;
        //}
    }
}
