using Microsoft.AspNetCore.Mvc.Filters;

namespace RDHATEOAS.LinkAdders
{
    public interface ILinkAdder
    {
        void AddLinks(object currentObjectValue, ResultExecutingContext context, int pathId, int arrayId);
    }
}
