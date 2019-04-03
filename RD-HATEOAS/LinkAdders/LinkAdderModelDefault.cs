using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RDHATEOAS.LinkAdders
{
    public class LinkAdderModelDefault<T> : ILinkAdderModel
    {

        public LinkAdderModelDefault()
        {
        }

        public bool CanHandleObjectOfType(Type type)
        {
            return (type == typeof(T) || type == typeof(List<T>));
        }

        protected virtual Task<Object> AddLinksAsync(T item, IUrlHelper urlHelper)
        {
            // default implementation just returns the item, which is not actually async
            // but we return a task anyway so the compiler doesn't squawk at us
            return Task.FromResult((Object)item);
        }


    }
}
