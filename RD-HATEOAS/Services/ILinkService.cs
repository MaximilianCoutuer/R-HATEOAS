using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.Services
{
    public interface ILinkService
    {
        string AddLinksToOutput<TResource>(ref TResource resource);
    }
}
