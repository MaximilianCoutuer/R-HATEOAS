using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace RDHATEOAS.Models
{
    /// <summary>
    /// A HATEOAS link.
    /// The link contains all relevant HATEOAS fields and getters and setters for each.
    /// </summary>
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
            set { _href = (value != null ? value.Replace("%2F", "/") : null); }
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
        public string Hreflang { get; private set; }
        public string Media { get; private set; }
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

        public HateoasLink() : this("", "self", HttpMethod.Get) { }

        #endregion

        #region methods

        public HateoasLink AddHreflang(string Hreflang)
        {
            this.Hreflang = Hreflang;
            return this;
        }

        public HateoasLink AddMedia(string Media)
        {
            this.Media = Media;
            return this;
        }

        public HateoasLink AddTitle(string Title)
        {
            this.Title = Title;
            return this;
        }

        public HateoasLink AddType(string Type)
        {
            this.Type = Type;
            return this;
        }

        #endregion

    }
}
