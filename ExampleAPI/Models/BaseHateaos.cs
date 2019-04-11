using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleAPI.Models
{
    public abstract class BaseHateaos
    {
        [NotMapped]
        public IEnumerable<string> Links { get; set; }

    }
}
