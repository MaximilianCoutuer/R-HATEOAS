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
        public HttpMethod Method { get; set; }
        public string kitten = "lol";
    }
}
