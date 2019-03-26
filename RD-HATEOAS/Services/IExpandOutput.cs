using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.Services
{
    public interface IExpandOutput
    {
        string ExpandOutput(string output);
    }
}
