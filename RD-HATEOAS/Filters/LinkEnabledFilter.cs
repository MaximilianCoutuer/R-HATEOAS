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
using Microsoft.AspNetCore.Mvc.Routing;

namespace RDHATEOAS.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class LinkEnabledAttribute : ResultFilterAttribute
    {
        private readonly string _test;

        public LinkEnabledAttribute(string test)
        {
            _test = test;
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
                    var test = (new UrlHelper(response));
                    var testlink = test.RouteUrl("Testroute", new { controller = "api/person/", id = 1 });
                    //var testlink = test.Action("GetAllPersons", "PersonController");

                    IDictionary<string, object> itemWithLink = new ExpandoObject(); // initializing as a dictionary so we can use Add();
                    // HACK HACK HACK
                    // this is horrible
                    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item.GetType()))
                        itemWithLink.Add(property.Name, property.GetValue(item));
                    itemWithLink.Add("_link", testlink);
                    okObjectResult.Value = itemWithLink;
                }
                //else if (okObjectResult.Value.GetType().GetGenericTypeDefinition() == typeof(List<>)
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
    }
}