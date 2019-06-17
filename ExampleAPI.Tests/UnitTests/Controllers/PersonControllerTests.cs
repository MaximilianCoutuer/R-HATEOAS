using ExampleAPI.Controllers;
using ExampleAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ExampleAPI.Tests.UnitTests.Controllers
{
    public class DatabaseFixture : IDisposable
    {
        public PersonController personController;
        public readonly Person[] testPersons = new Person[10];
        public ExampleDbContext mockExampleDbContext;

        public DatabaseFixture()
        {
            // establish an in-memory database
            var dbOptions = new DbContextOptionsBuilder<ExampleDbContext>()
                .UseInMemoryDatabase(databaseName: "exampleapi")
                .Options;
            mockExampleDbContext = new ExampleDbContext(dbOptions);
            this.personController = new PersonController(mockExampleDbContext);

            // seed database with randomly generated test persons
            mockExampleDbContext.Database.EnsureCreated();
            for (int i = 0; i < 10; i++)
            {
                testPersons[i] = CreateRandomPerson();
                mockExampleDbContext.Add(testPersons[i]);
            }
            mockExampleDbContext.SaveChanges();
        }

        public void Dispose()
        {
            foreach (var person in mockExampleDbContext.Persons)
            {
                mockExampleDbContext.Remove(person);
            }
        }

        public static Person CreateRandomPerson()
        {
            return new Person()
            {
                FirstName = GetRandomString(16),
                LastName = GetRandomString(16),
                Age = new Random().Next(10, 99),
                Country = new Country()
                {
                    Name = GetRandomString(16),
                    Capital = new City()
                    {
                        Name = GetRandomString(16),
                        HistoricName = GetRandomString(16),
                    },
                    Population = new Random().Next(0, 5000000),
                },
            };
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
            // arrange
            var testPerson = DatabaseFixture.CreateRandomPerson();

            // act
            await _fixture.personController.PostPerson(testPerson);

            // assert
            var retrievedPerson = await _fixture.personController.GetPerson(testPerson.Id);
            Assert.Equal(testPerson, retrievedPerson);

        }

        [Fact]
        public async void PostPerson_NullPerson_ShouldThrowAsync()
        {
            // arrange
            Person emptyPerson = null;

            // act

            // assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _fixture.personController.PostPerson(emptyPerson));
        }

        [Fact]
        public async void PostPerson_DuplicatePerson_ShouldPostPersonAsync()
        {
            // arrange
            var testPerson = DatabaseFixture.CreateRandomPerson();

            // act
            await _fixture.personController.PostPerson(testPerson);


            // assert
            await Assert.ThrowsAsync<ArgumentException>(() => _fixture.personController.PostPerson(testPerson));
        }

        [Fact]
        public async void PutPerson_NewPerson_ShouldPutPersonAsync()
        {
            // arrange
            var testPerson = DatabaseFixture.CreateRandomPerson();

            // act
            await _fixture.personController.PutPerson(testPerson.Id, testPerson);

            // assert
            var retrievedPerson = await _fixture.personController.GetPerson(testPerson.Id);
            Assert.Equal(testPerson, retrievedPerson);

        }

        [Fact]
        public async void PutPerson_NullPerson_ShouldThrowAsync()
        {
            // arrange
            Person emptyPerson = null;

            // act

            // assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await _fixture.personController.PutPerson(5, emptyPerson));
        }

        [Fact]
        public async void PutPerson_ExistingPerson_ShouldPutPersonAsync()
        {
            // arrange
            var testPerson = DatabaseFixture.CreateRandomPerson();

            // act
            await _fixture.personController.PostPerson(testPerson);
            testPerson.FirstName = DatabaseFixture.GetRandomString(16);
            await _fixture.personController.PutPerson(testPerson.Id, testPerson);

            // assert
            var retrievedPerson = await _fixture.personController.GetPerson(testPerson.Id);
            Assert.Equal(testPerson, retrievedPerson);
        }

        [Fact]
        public async void DeletePerson_ShouldDeletePersonAsync()
        {
            // arrange
            var testPerson = DatabaseFixture.CreateRandomPerson();

            // act
            await _fixture.personController.PostPerson(testPerson);
            await _fixture.personController.DeletePerson(testPerson.Id);

            var actionResult = await _fixture.personController.GetPerson(testPerson.Id);
            var notFoundResult = (actionResult.Result as NotFoundResult);

            // assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void DeletePerson_InvalidID_Should404Async()
        {
            // arrange

            // act
            var actionResult = await _fixture.personController.DeletePerson(9999);
            var notFoundResult = (actionResult as NotFoundResult);

            // assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
