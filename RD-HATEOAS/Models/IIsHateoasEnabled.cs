using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDHATEOAS.Models
{
    /// <summary>
    /// Implement this on a model class to indicate that it is HATEOAS enabled and comes with a list of links.
    /// </summary>
    public interface IIsHateoasEnabled
    {
        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        List<HateoasLink> Links { get; set; }
    }
}
