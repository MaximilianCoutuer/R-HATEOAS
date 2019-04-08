using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Models
{
    public class HateoasLink
    {
        #region fields
        private string _href;
        private HttpMethod _method;

        // rel (usually a standard name based on convention)
        [Required]
        public string Rel { get; set; } = "self";

        // URI
        [Required]
        public string Href
        {
            get { return _href; }
            set { _href = value.Replace("%2F", "/"); }
        }

        // HTTP method
        [Required]
        public HttpMethod HttpMethod {
            set { _method = value; }
        }
        public string Method
        {
            get { return _method.Method; }
        }

        // optional properties
        public string Hreflang { get; set; }
        public string Media { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        #endregion

        #region constructors
        public HateoasLink(string Href, string Rel, HttpMethod HttpMethod)
        {
            this.Href = Href;
            this.Rel = Rel;
            this.HttpMethod = HttpMethod;
        }

        public HateoasLink() : this("", "self", HttpMethod.Get)
        {
        }
        #endregion

    }
}
