using ExampleAPI.Controllers;
using ExampleAPI.Models;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory;

namespace ExampleAPI.Tests
{
    public class PersonController_TestAbc
    {

        private PersonController personController;

        public PersonController_TestAbc()

        {

            var dbOptions = new DbContextOptionsBuilder<PeopleContext>()
                .UseInMemoryDatabase(databaseName: "ExampleAPI_IMDB")
                .Options;
            var mockPeopleContext = new PeopleContext(dbOptions);
            this.personController = new PersonController(mockPeopleContext);
        }
        

        [Fact]
        public void RequestFirstPersonGetThatPerson()
        {
            // arrange

            // act
            var person = personController.GetPerson(1);

            // assert
            Assert.NotNull(person);
            Assert.True(person.GetType() == typeof(Person));
        }

        [Fact]
        public void RequestPersonListGetListOfPersons()
        {
            // arrange

            // act
            var persons = personController.GetAllPersons();

            // assert
            Assert.NotNull(persons);
            Assert.True(persons.GetType() == typeof(List<Person>));
        }

        [Fact]
        public void RequestPersonOutOfRangeGetNull()
        {
            // arrange

            // act

            // assert
        }
    }
}
