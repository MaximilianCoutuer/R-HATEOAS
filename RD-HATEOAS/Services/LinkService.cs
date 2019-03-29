using Microsoft.AspNetCore.Mvc.Controllers;
using RDHATEOAS.Controllers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RDHATEOAS.Services
{
    public class LinkService
    {
        public string AddLinksToOutput<TResource>(ref TResource resource, [CallerMemberName] string memberName = "")
        {

            //HATEOASLinksAttribute[] AttributeArray2 = (HATEOASLinksAttribute[])(RouteMap.GetCurrentRoute().LinksAttributes());
        HATEOASLinksAttribute[] AttributeArray = (HATEOASLinksAttribute[])(resource.GetType()).GetCustomAttributes(typeof(HATEOASLinksAttribute), true);
            return ""+AttributeArray.Length;
        }

        //public async Task AddLinksToResponse(ResultExecutingContext response)
        //{
        //    var urlHelper = (new UrlHelperFactory()).GetUrlHelper(response);

        //    if (response.Result is OkObjectResult okObjectResult)
        //        if (okObjectResult.Value is T model)
        //        {
        //            await AddLinks(model, urlHelper);
        //        }
        //        else if (okObjectResult.Value is List<T> collection)
        //        {
        //            Parallel.ForEach(collection.ToList(), (element) =>
        //            {
        //                AddLinks(element, urlHelper);
        //            });
        //        }
        //        else if (okObjectResult.Value is PagedSearchDTO<T> pagedSearch)
        //        {
        //            Parallel.ForEach(pagedSearch.List.ToList(), (element) =>
        //            {
        //                AddLinks(element, urlHelper);
        //            });
        //        }
        //    await Task.FromResult<object>(null);
        //}

        private void AddLinks()
        {

        }
    }
}
