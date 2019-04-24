using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RDHATEOAS.Models
{
    public interface IIsHateoasEnabled
    {
        // Hateoas Enabled
        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        List<HateoasLink> Links { get; set; }
    }
}
