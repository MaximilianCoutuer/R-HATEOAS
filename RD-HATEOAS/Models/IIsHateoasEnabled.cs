using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RDHATEOAS.Models
{
    /// <summary>
    /// Indicates that a model item is HATEOAS enabled and comes with a list of links.
    /// </summary>
    public interface IIsHateoasEnabled
    {
        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        List<HateoasLink> Links { get; set; }
    }
}
