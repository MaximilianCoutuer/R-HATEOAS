namespace ExampleAPI.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// An example class.
    /// </summary>
    public class Person
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
