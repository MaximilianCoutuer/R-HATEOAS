using ExampleAPI.Controllers;
using ExampleAPI.Models;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace ExampleAPI.Tests
{
    public class PersonController_TestAbc
    {

        private readonly PersonController _personController;

        public PersonController_TestAbc()

        {
            var mockDbSet = Substitute.For(DbSet<Person>, IQueryable<Person>);
            var context = new PeopleContext(); // TODO: mock PersonContext
            this._personController = new PersonController(context);
        }

        [Fact]
        public void RequestFirstPersonGetThatPerson()
        {
            // arrange

            // act
            var person = _personController.GetPerson(1);

            // assert
            Assert.True(person.GetType() == typeof(Person));
        }

        [Fact]
        public void RequestPersonListGetListOfPersons()
        {
            // arrange

            // act
            var persons = _personController.GetAllPersons();

            // assert
            Assert.True(persons.GetType() == typeof(List<Person>));
        }
    }
}
