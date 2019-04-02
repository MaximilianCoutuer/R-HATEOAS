using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.ComponentModel;
using RDHATEOAS.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Builders;
using RDHATEOAS.Models;
using System.Net.Http;

namespace RDHATEOAS.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class LinkEnabledAttribute : ResultFilterAttribute
    {
        public LinkEnabledAttribute(string test)
        {
            //_urlHelper = urlHelper;
        }

        public override void OnResultExecuting(ResultExecutingContext response)
        {
            // obtain attributes attached to controller method
            var controllerActionDescriptor = response.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);
            }

            //var enricher = _hyperMediaFilterOptions.ObjectContentResponseEnricherList.FirstOrDefault(x => x.CanEnrich(context));
            //if (enricher != null) Task.FromResult(enricher.Enrich(context));

            if (response.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
                if (okObjectResult.Value is Object item)
                {
                    // result is a single object

                    IUrlHelper urlHelper = new UrlHelper(response);   // DI not possible because the filter is an attribute
                    //var builtLink = response.HttpContext.Request.Host.ToUriComponent();
                    //builtLink += urlHelper.RouteUrl("Testroute", new { controller = "person", id = 1 });
                    //var testlink = test.Action("GetAllPersons", "PersonController");
                    // hardcode?

                    //var builtLink = GenerateLinkObject(new UrlHelper(response), response).Href;
                    var builtLink = GenerateLinkObject(urlHelper, response);
                    // 

                    //var builtLink = (new HATEOASLinkObjectBuilder(new UrlHelper(response))).Build(response).Href;

                    IDictionary<string, object> itemWithLink = new ExpandoObject(); // initializing as a dictionary so we can use Add();


                    // HACK HACK HACK
                    // this is horrible
                    // isn't there a better way?
                    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item.GetType()))
                        itemWithLink.Add(property.Name, property.GetValue(item));
                    itemWithLink.Add("links", builtLink);
                    okObjectResult.Value = itemWithLink;
                }
                //else if (okObjectResult.Value.GetType().GetGenericTypeDefinition() == typeof(List<>)
                //?
                else if (okObjectResult.Value is List<Object> list)
                {
                    Parallel.ForEach(list.ToList(), (element) =>
                    {
                        IDictionary<string, object> itemWithLink = new ExpandoObject();
                        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(element.GetType()))
                            itemWithLink.Add(property.Name, property.GetValue(element));
                        itemWithLink.Add("_link", "rofl");
                    });
                }
                //else if (okObjectResult.Value is PagedSearchDTO<Object> pagedSearch)
                //{
                //    Parallel.ForEach(pagedSearch.List.ToList(), (element) =>
                //    {
                //        // result is a paged search??
                //        //test(element, urlHelper);
                //    });
                //}
            //await Task.FromResult<object>(null);

            base.OnResultExecuting(response);
        }

        private HateoasLink[] GenerateLinkObject(IUrlHelper urlHelper, ResultExecutingContext response)
        {
            var builtLink = response.HttpContext.Request.Host.ToUriComponent();
            builtLink += urlHelper.RouteUrl("Testroute", new { controller = "person", id = 1 });

            HateoasLink[] test = {
                new HateoasLink("Testlink1", "Testlink2", HttpMethod.Get),
                new HateoasLink("Testlink2", "Testlink1", HttpMethod.Post)
            };
            return test;
        }
    }
}