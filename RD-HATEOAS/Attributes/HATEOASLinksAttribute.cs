using System;

namespace RDHATEOAS.Controllers
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class HATEOASLinksAttribute : Attribute
    {
        private string linksType;

        public HATEOASLinksAttribute(string LinksType) {
            this.linksType = LinksType;
        }

        public string getLinksType()
        {
            return linksType;
        }
    }
}