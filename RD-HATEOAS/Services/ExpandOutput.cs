using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.Services
{
    public class ExpandOutput : IExpandOutput
    {
        string IExpandOutput.ExpandOutput(string output)
        {
            return "output modified by ExpandOutput";
        }
    }
}
