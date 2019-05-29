using Rdhateoas.Models;
using Xunit;

namespace Rdhateoas.Tests.UnitTests.Entities
{
    public class HateoasLinkBuilderEntityTests
    {
        [Fact]
        public void CreateLink_ShouldCreateValidLink()
        {
            // arrange
            var link = new HateoasLink();

            // act

            // assert
            Assert.NotNull(link);
            Assert.Equal("self", link.Rel);
            Assert.Equal("GET", link.Method);
        }

        [Theory]
        [InlineData("http://www.realdolmen.com")]
        [InlineData("")]
        public void SetLinkHref_ShouldSet(string data)
        {
            // arrange
            var link = new HateoasLink
            {

                // act
                Href = data
            };

            // assert
            Assert.Equal(data, link.Href);
        }

        [Theory]
        [InlineData(null)]
        public void SetLinkHref_Null_ShouldSetEmpty(string data)
        {
            // arrange
            var link = new HateoasLink
            {

                // act
                Href = data
            };

            // assert
            Assert.Equal("", link.Href);
        }

        [Theory]
        [InlineData("list")]
        [InlineData("")]
        [InlineData(null)]
        public void SetLinkRel_ShouldSet(string data)
        {
            // arrange
            var link = new HateoasLink
            {

                // act
                Rel = data
            };

            // assert
            Assert.Equal(data, link.Rel);
        }

        [Theory]
        [InlineData("en-us")]
        [InlineData("")]
        [InlineData(null)]
        public void SetLinkHreflang_ShouldSet(string data)
        {
            // arrange
            var link = new HateoasLink();

            // act
            link.AddHreflang(data);

            // assert
            Assert.Equal(data, link.Hreflang);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("")]
        [InlineData(null)]
        public void SetLinkMedia_ShouldSet(string data)
        {
            // arrange
            var link = new HateoasLink();

            // act
            link.AddMedia(data);

            // assert
            Assert.Equal(data, link.Media);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("")]
        [InlineData(null)]
        public void SetLinkTitle_ShouldSet(string data)
        {
            // arrange
            var link = new HateoasLink();

            // act
            link.AddTitle(data);

            // assert
            Assert.Equal(data, link.Title);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("")]
        [InlineData(null)]
        public void SetLinkType_ShouldSet(string data)
        {
            // arrange
            var link = new HateoasLink();

            // act
            link.AddType(data);

            // assert
            Assert.Equal(data, link.Type);
        }
    }
}
