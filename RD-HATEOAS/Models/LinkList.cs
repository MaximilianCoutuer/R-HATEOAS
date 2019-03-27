using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.Models
{
    public abstract class LinkList
    {
        [JsonProperty(PropertyName = "_links")]
        private Dictionary<string, HateoasLink> linkList { get; set; }   // TODO: string -> enum
    }
}
