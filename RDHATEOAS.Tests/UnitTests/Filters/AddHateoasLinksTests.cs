using Microsoft.AspNetCore.Mvc.Filters;
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
        public async void AddLinksToEmptyObject_ShouldAddLinks()
        {
            //// can't unit test parameters
            //var context = new ResultExecutingContext();
            //AddHateosLinksAttribute filter = new AddHateosLinksAttribute(new Object[] { new string[] { }, typeof(HateoasRulesetBase) });
            //filter.OnResultExecuting(context);


            // instanciate filter
            // feed it a fake context
            // feed it a fake ruleset

        }

        // negative tests
        // wrong context
}
}
