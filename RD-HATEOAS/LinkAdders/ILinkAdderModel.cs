using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RDHATEOAS.LinkAdders
{
    public interface ILinkAdderModel
    {
        bool CanHandleObjectOfType(Type type);
    }


}
