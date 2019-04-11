using ExampleAPI.Models;
using RDHATEOAS.Controllers;
using RDHATEOAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleAPI.Models
{
    //[HATEOASLinks("test_links_in_model")]
    public class Person : IsHateoasEnabled
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Country Country { get; set; }

    }
}
