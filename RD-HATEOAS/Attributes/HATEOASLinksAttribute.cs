using System;

namespace RDHATEOAS.Controllers
{
    [Obsolete]
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
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

        public string getDuckTest()
        {
            return "quack";
        }
    }
}