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

        public HateoasLink(string Href, string Rel, HttpMethod Method)
        {
            this.Href = Href;
            this.Rel = Rel;
            this.Method = Method;
        }
        
        [Required]
        public string Href
        {
            get { return _href; }
            set { _href = value.Replace("%2F", "/"); }
        }
        
        [Required]
        public string Rel { get; set; }

        [Required]
        public HttpMethod Method { get; set; }  // TODO: other types such as self and next/previous??

        public string Hreflang { get; set; }
        public string Media { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }
}
