using Microsoft.AspNetCore.Mvc.Filters;

namespace Rhateoas.LinkAdders
{
    public interface ILinkAdder
    {
        void AddLinks(object currentObjectValue, ResultExecutingContext context);
    }
}
