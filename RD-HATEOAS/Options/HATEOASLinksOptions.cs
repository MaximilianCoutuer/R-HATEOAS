using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RDHATEOAS.Options
{
    public class HATEOASLinksOptions
    {
        public HATEOASLinksOptions()
        {
            // options
        }

        [Required]
        public string testSetting1 { get; set; }
    }
}
