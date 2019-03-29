using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Models
{
    public class HateoasLink
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public HttpMethod Action { get; set; }  // TODO: other types such as self and next/previous??
        public string TypeKeyword { get; set; } // "details" etc
    }
}
