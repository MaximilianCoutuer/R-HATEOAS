using Newtonsoft.Json;
using RDHATEOAS.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleAPI.Models
{
    public class Country : IIsHateoasEnabled
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Capital { get; set; }

        public int? Population { get; set; }

        public Country()
        {
        }

        public Country(string name, string capital, int? population)
        {
            Name = name;
            Capital = capital;
            Population = population;
        }

        // implements IIsHateoasEnabled 
        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        List<HateoasLink> IIsHateoasEnabled.Links { get; set; } = new List<HateoasLink>();
    }
}
