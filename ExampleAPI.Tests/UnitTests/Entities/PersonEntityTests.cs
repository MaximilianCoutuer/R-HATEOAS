﻿using ExampleAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace ExampleAPI.Tests.UnitTests.Entities
{
    public class PersonEntityTests
    {
        // TODO: Ask about scope

        [Fact]
        public void CreatePerson_ShouldCreateValidPerson()
        {
            // arrange
            var person = new Person();

            // act

            // assert
            Assert.NotNull(person);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("")]
        [InlineData(null)]
        public void SetPersonFirstName_ShouldSet(string name)
        {
            // arrange
            var person = new Person();

            // act
            person.FirstName = name;

            // assert
            Assert.Equal(name, person.FirstName);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("")]
        [InlineData(null)]
        public void SetPersonLastName_ShouldSet(string name)
        {
            // arrange
            var person = new Person();

            // act
            person.LastName = name;

            // assert
            Assert.Equal(name, person.LastName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(150)]
        public void SetPersonAge_ShouldSet(int age) {
            // arrange
            var person = new Person();

            // act
            person.Age = age;

            // assert
            Assert.Equal(age, person.Age);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(151)]
        public void SetPersonAge_Invalid_ShouldThrow(int age) {
            // arrange
            var person = new Person();

            // act/assert
            Assert.Throws<ArgumentOutOfRangeException>(() => person.Age = age);
        }

        [Theory, MemberData(nameof(CountryData))]
        public void SetPersonCountry_ShouldSet(string name, string capital, int? population)
        {
            // arrange
            var person = new Person();
            var country = new Country(name, capital, population);

            // act
            person.Country = country;

            // assert
            Assert.Equal(country, person.Country);
        }

        public static IEnumerable<Object[]> CountryData
        {
            get
            {
                return new[]
                {
                    new Object[] { "Test", "Test", 500000 },
                    new Object[] { "Test", null, 500000 },
                    new Object[] { "Test", "Test", null},
                    new Object[] { null, null, null },
            }   ;
            }
        }

    }
}