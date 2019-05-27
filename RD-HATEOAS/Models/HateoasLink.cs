using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;

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
        private IDictionary<string, string> _queryString = new Dictionary<string, string>();

        // rel (usually a standard name based on convention)
        [Required]
        public string Rel { get; set; } = "self";

        // URI
        [Required]
        public string Href
        {
            get
            {
                var url = new StringBuilder();
                url.Append(_href);
                url.Append("?");
                foreach (var queryStringEntry in _queryString)
                {
                    url.Append(queryStringEntry.Key + "=" + queryStringEntry.Value + "&");
                }

                url.Remove(url.Length - 1, 1);
                return url.ToString();
            }

            set
            {
                _href = value?.Replace("%2F", "/");
            }
        }

        // HTTP method
        [Required]
        public HttpMethod HttpMethod
        {
            set { _method = value; }
        }

        public string Method
        {
            get { return _method.Method; }
        }

        // optional properties
        public string Hreflang { get; private set; }

        public string Media { get; private set; }

        public string Title { get; private set; }

        public string Type { get; private set; }

        #endregion

        #region constructors

        public HateoasLink(string href, string rel, HttpMethod httpMethod)
        {
            this.Href = href;
            this.Rel = rel;
            this.HttpMethod = httpMethod;
        }

        public HateoasLink()
            : this(string.Empty, "self", HttpMethod.Get)
        {
        }

        #endregion

        #region methods

        public HateoasLink AddHreflang(string hreflang)
        {
            this.Hreflang = hreflang;
            return this;
        }

        public HateoasLink AddMedia(string media)
        {
            this.Media = media;
            return this;
        }

        public HateoasLink AddTitle(string title)
        {
            this.Title = title;
            return this;
        }

        public HateoasLink AddType(string type)
        {
            this.Type = type;
            return this;
        }

        /// <summary>
        /// Appends a key/value pair to the query string of this HATEOAS link.
        /// </summary>
        /// <param name="key">The key to append.</param>
        /// <param name="value">The value to append.</param>
        /// <returns>
        /// The created link.
        /// </returns>
        public HateoasLink ExtendQueryString(string key, string value)
        {
            this._queryString.Add(key, value);
            return this;
        }

        #endregion

    }
}
