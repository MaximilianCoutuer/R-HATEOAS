﻿namespace ExampleAPI.Models
{
    public class Country
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public City Capital { get; set; }

        public int? Population { get; set; }

        public Country()
        {
        }

        public Country(string name, City capital, int? population)
        {
            Name = name;
            Capital = capital;
            Population = population;
        }
    }
}
