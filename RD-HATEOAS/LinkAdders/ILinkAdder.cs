using Microsoft.AspNetCore.Mvc.Filters;

namespace Rdhateoas.LinkAdders
{
    public interface ILinkAdder
    {
        void AddLinks(object currentObjectValue, ResultExecutingContext context);
    }
}
