using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Validators
{
    class LinkValidator
    {
        public bool ShouldAddLink()
        {
            return true;
        }
    }
}
