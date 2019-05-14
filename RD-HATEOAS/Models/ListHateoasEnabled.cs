namespace RDHATEOAS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// Model class that contains both a list and relevant HATEOAS links for that list.
    /// </summary>
    /// <remarks>
    /// This allows us to add HATEOAS links to a list without modifying the List class.
    /// </remarks>
    public class ListHateoasEnabled : IIsHateoasEnabled
    {
        public List<object> List { get; set; } = new List<object>();

        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        public List<HateoasLink> Links { get; set; } = new List<HateoasLink>();
    }
}
