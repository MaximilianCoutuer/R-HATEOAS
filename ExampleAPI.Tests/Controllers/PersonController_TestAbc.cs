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
using Microsoft.AspNetCore.Mvc;

namespace ExampleAPI.Tests
{
    public class PersonController_TestAbc
    {

        private PersonController personController;
        private Person testPerson;
        private PeopleContext mockPeopleContext;

        public PersonController_TestAbc()

        {
            var dbOptions = new DbContextOptionsBuilder<PeopleContext>()
                .UseInMemoryDatabase(databaseName: "exampleapi")
                .Options;
            mockPeopleContext = new PeopleContext(dbOptions);
            this.personController = new PersonController(mockPeopleContext);

            // seed database with test person
            testPerson = new Person()
            {
                FirstName = "Maximilian",
                LastName = "Coutuer",
                Age = 35,
                Country = new Country()
                {
                    Name = "Belgium",
                    Capital = "Brussels"
                },
            };
            mockPeopleContext.Add(testPerson);
            mockPeopleContext.SaveChanges();
        }
        

        [Fact]
        public async void RequestFirstPersonGetThatPerson()
        {
            // arrange

            // act
            var actionResult = await personController.GetPerson(testPerson.Id);
            var person = (actionResult.Result as OkObjectResult).Value;

            // assert
            Assert.NotNull(person);
            Assert.Equal(testPerson, person);
        }

        [Fact]
        public async void RequestPersonListGetListOfPersons()
        {
            // arrange

            // act
            var actionResult = await personController.GetAllPersons();
            var persons = (actionResult.Result as OkObjectResult).Value;

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
