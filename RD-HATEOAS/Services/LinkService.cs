using RDHATEOAS.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.Services
{
    public class LinkService : ILinkService
    {
        public string AddLinksToOutput<TResource>(ref TResource resource)
        {
            HATEOASLinksAttribute[] AttributeArray = (HATEOASLinksAttribute[])(resource.GetType()).GetCustomAttributes(typeof(HATEOASLinksAttribute), true);
            return ""+AttributeArray.Length;
        }
    }
}
