using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RDHATEOAS.Models
{
    public class ListHateoasEnabled : IsHateoasEnabled
    {
        // TODO: This is horrible and needs a rewrite
        public List<Object> list = new List<Object>();
    }
}
