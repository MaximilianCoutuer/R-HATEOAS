using Microsoft.Extensions.Options;
using RDHATEOAS.LinkAdders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RDHATEOAS.Options
{
    public class HateoasOptions
    {
        public HateoasOptions()
        {

        }

        public List<ILinkAdderModel> linkAddersModel { get; set; } = new List<ILinkAdderModel>();
    }
}
