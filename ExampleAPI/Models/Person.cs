namespace ExampleAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    using RDHATEOAS.Models;

    /// <summary>
    /// An example class.
    /// This class supports HATEOAS links because it implements IIsHateoasEnabled.
    /// </summary>
    public class Person : IIsHateoasEnabled
    {
        private int _age;

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [MinValue(0)]
        [MaxValue(150)]
        public int Age
        {
            get
            {
                return this._age;
            }

            set
            {
                if (value >= 0 && value <= 150) // future proofing
                {
                    this._age = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Country Country { get; set; }

        // implements IIsHateoasEnabled 
        [NotMapped]
        [JsonProperty(PropertyName = "_links")]
        List<HateoasLink> IIsHateoasEnabled.Links { get; set; } = new List<HateoasLink>();
    }

    public class MinValueAttribute : ValidationAttribute
    {
        private readonly int _minValue;

        public MinValueAttribute(int minValue)
        {
            _minValue = minValue;
        }

        public override bool IsValid(object value)
        {
            return (int)value >= _minValue;
        }
    }

    public class MaxValueAttribute : ValidationAttribute
    {
        private readonly int _maxValue;

        public MaxValueAttribute(int maxValue)
        {
            _maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return (int)value <= this._maxValue;
        }
    }
}
