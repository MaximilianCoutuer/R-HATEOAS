using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RDHATEOAS.Models
{
    /// <summary>
    /// Model class that contains both a list and relevant HATEOAS links for that list.
    /// </summary>
    /// <remarks>
    /// This allows us to add HATEOAS links to a list without modifying the List<> class.
    /// </remarks>
    public class ListHateoasEnabled : IIsHateoasEnabled
    {
        public List<Object> List { get; set; } = new List<Object>();

        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        public List<HateoasLink> Links { get; set; } = new List<HateoasLink>();
    }
}
