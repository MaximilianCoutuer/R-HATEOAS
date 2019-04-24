using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RDHATEOAS.Models
{
    // TODO: This is horrible and needs a rewrite
    public class ListHateoasEnabled : IIsHateoasEnabled
    {
        public List<Object> list = new List<Object>();

        // Hateoas Enabled
        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        List<HateoasLink> IIsHateoasEnabled.Links { get; set; } = new List<HateoasLink>();
    }
}
