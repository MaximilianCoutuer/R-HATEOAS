using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RDHATEOAS.Models
{
    public abstract class IsHateoasEnabled
    {
        // TODO: Multiple inheritance is problem?

        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        public HateoasLink[] Links { get; set; }
    }
}
