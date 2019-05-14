namespace RDHATEOAS.Tests.UnitTests.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ExampleAPI.Controllers;
    using ExampleAPI.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;
    using RDHATEOAS.Filters;
    using RDHATEOAS.Rulesets;
    using Xunit;

    public class AddHateoasLinksTests
    {
        [Fact]
        public void AddLinksToEmptyObject_ShouldAddLinks()
        {
            // TODO: Should we even try to mock this?

            // arrange
            var actionContext = new ActionContext();
            var value = new object();
            var actionResult = new OkObjectResult(value);
            var optionsBuilder = new DbContextOptionsBuilder<ExampleDbContext>();
            var exampleDbContext = new ExampleDbContext(optionsBuilder.Options);
            actionContext.HttpContext = new DefaultHttpContext();
            var resultExecutingContext = new ResultExecutingContext(actionContext, null, actionResult, new PersonController(exampleDbContext));
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
