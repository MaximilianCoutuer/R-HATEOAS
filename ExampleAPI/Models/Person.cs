using ExampleAPI.Models;
using Newtonsoft.Json;
using RDHATEOAS.Controllers;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleAPI.Models
{
    public class Person : IIsHateoasEnabled
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Country Country { get; set; }

        // Hateoas Enabled
        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        List<HateoasLink> IIsHateoasEnabled.Links { get; set; } = new List<HateoasLink>();
    }
}
