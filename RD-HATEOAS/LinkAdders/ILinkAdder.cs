using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.LinkAdders
{
    public interface ILinkAdder
    {
        void AddLinks(object currentObjectValue, ResultExecutingContext context, int pathId, int arrayId);
    }
}
