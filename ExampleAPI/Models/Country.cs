using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleAPI.Models
{
    public class Country
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
    }
}
