using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RDHATEOAS.Rulesets
{
    interface IHateoasRuleset
    {
        HttpMethod[] GetHttpMethods();
    }
}
