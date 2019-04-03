using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Models
{
    public class HateoasLink
    {

        private string _href;
        private HttpMethod _method;

        public HateoasLink(string Href, string Rel, HttpMethod HttpMethod)
        {
            this.Href = Href;
            this.Rel = Rel;
            this.HttpMethod = HttpMethod;
        }
        
        [Required]
        public string Href
        {
            get { return _href; }
            set { _href = value.Replace("%2F", "/"); }
        }
        
        [Required]
        public string Rel { get; set; } // ex. "Self"

        [Required]
        public HttpMethod HttpMethod {
            set { _method = value; }
        }

        public string Method
        {
            get { return _method.Method; }
        }
        // TODO: other types such as self and next/previous??

        public string Hreflang { get; set; }
        public string Media { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        // Schema
    }
}
