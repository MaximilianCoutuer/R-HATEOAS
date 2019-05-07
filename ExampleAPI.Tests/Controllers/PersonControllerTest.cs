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
using System;

namespace ExampleAPI.Tests
{
    public class DatabaseFixture : IDisposable
    {

        public PersonController personController;
        public readonly Person[] testPersons = new Person[10];
        public PeopleContext mockPeopleContext;

        public DatabaseFixture()
        {
            var dbOptions = new DbContextOptionsBuilder<PeopleContext>()
                .UseInMemoryDatabase(databaseName: "exampleapi")
                .Options;
            mockPeopleContext = new PeopleContext(dbOptions);
            this.personController = new PersonController(mockPeopleContext);

            // seed database with test persons
            mockPeopleContext.SaveChanges();
            for (int i = 0; i < 10; i++)
            {
                testPersons[i] = new Person()
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
                mockPeopleContext.Add(testPersons[i]);
            }
            mockPeopleContext.SaveChanges();
        }

        public void Dispose()
        {
            foreach (var person in mockPeopleContext.Persons)
            {
                mockPeopleContext.Remove(person);
            }
        }
    }

    public class PersonControllerTests : IClassFixture<DatabaseFixture> {

        DatabaseFixture _fixture;

        public PersonControllerTests(DatabaseFixture fixture)
        {
            this._fixture = fixture;
        }

        [Fact]
        public async void GetPerson_FirstId_ShouldRetrievePerson()
        {
            // arrange

            // act
            var actionResult = await _fixture.personController.GetPerson(_fixture.testPersons[0].Id);
            var person = (actionResult.Result as OkObjectResult).Value;

            // assert
            Assert.NotNull(person);
            Assert.Equal(_fixture.testPersons[0], person);
        }

        [Fact]
        public async void GetAllPersons_ShouldRetrievePersonList()
        {
            // arrange

            // act
            var actionResult = await _fixture.personController.GetAllPersons();
            var persons = (actionResult.Result as OkObjectResult).Value;

            // assert
            Assert.NotNull(persons);
            Assert.True(persons.GetType() == typeof(List<Person>));
        }

        [Fact]
        public async void GetPerson_InvalidId_Should404Async()
        {
            // arrange

            // act
            var actionResult = await _fixture.personController.GetPerson(9999);
            var notFoundResult = (actionResult.Result as NotFoundResult);

            // assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async void GetPaginatedList_ShouldRetrieveCorrectPerson()
        {
            // arrange

            // act
            var actionResult = await _fixture.personController.GetPaginatedList(1, 1);
            var test = await _fixture.personController.GetAllPersons();
            var person = (actionResult.Result as OkObjectResult).Value;

            // assert
            Assert.NotNull(person);
            Assert.Equal(new List<Person>() { _fixture.testPersons[1] }, person);
        }
    }
}
