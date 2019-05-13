using ExampleAPI.Controllers;
using ExampleAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RDHATEOAS.Filters;
using RDHATEOAS.Rulesets;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RDHATEOAS.Tests.UnitTests.Filters
{
    public class AddHateoasLinksTests
    {
        [Fact]
        public void AddLinksToEmptyObject_ShouldAddLinks()
        {
            // arrange
            var actionContext = new ActionContext();
            var value = new Object();
            var actionResult = new OkObjectResult(value);
            var ExampleDbContext = new ExampleDbContext(null);
            var resultExecutingContext = new ResultExecutingContext(actionContext, null, actionResult, new PersonController(ExampleDbContext));
            var filter = new AddHateoasLinksAttribute(new string[] { }, typeof(HateoasRulesetBase));

            // act
            filter.OnResultExecuting(resultExecutingContext);

            // assert

            Assert.True(false);

        }

        // negative tests
        // wrong context
}
}
