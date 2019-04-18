using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.Models
{
    interface IIsHateoasEnabled
    {

    [NotMapped]
    [JsonProperty(PropertyName = "_links")]
    public HateoasLink[] Links { get; set; }
    }
}
