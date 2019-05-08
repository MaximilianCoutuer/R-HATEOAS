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
using System.Text;

namespace ExampleAPI.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public PersonController personController;
        public readonly Person[] testPersons = new Person[10];
        public PeopleContext mockPeopleContext;

        public DatabaseFixture()
        {
            // establish an in-memory database
            var dbOptions = new DbContextOptionsBuilder<PeopleContext>()
                .UseInMemoryDatabase(databaseName: "exampleapi")
                .Options;
            mockPeopleContext = new PeopleContext(dbOptions);
            this.personController = new PersonController(mockPeopleContext);

            // seed database with randomly generated test persons
            mockPeopleContext.SaveChanges();
            for (int i = 0; i < 10; i++)
            {
                testPersons[i] = new Person()
                {
                    FirstName = GetRandomString(16),
                    LastName = GetRandomString(16),
                    Age = new Random().Next(10,99),
                    Country = new Country()
                    {
                        Name = GetRandomString(16),
                        Capital = GetRandomString(16),
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

        public static string GetRandomString(int length)
        {
            var r = new Random();
            var s = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                s.Append((char)r.Next(65, 91));
            }
            return s.ToString();
        }
    }

    public class PersonControllerTests : IClassFixture<DatabaseFixture> {

        DatabaseFixture _fixture;

        public PersonControllerTests(DatabaseFixture fixture)
        {
            this._fixture = fixture;
        }

        [Fact]
        public async void GetPerson_FirstId_ShouldRetrievePersonAsync()
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
        public async void GetAllPersons_ShouldRetrievePersonListAsync()
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
        public async void GetPaginatedList_ShouldRetrieveCorrectPersonAsync()
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

        [Fact]
        public async void PostPerson_ShouldPostPersonAsync()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void PostPerson_NullPerson_ShouldThrowAsync()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void PostPerson_DuplicatePerson_ShouldPostPersonAsync()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void DeletePerson_ShouldDeletePersonAsync()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void DeletePerson_InvalidPerson_ShouldThrowAsync()
        {
            throw new NotImplementedException();
        }
    }
}
