using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Routing;
using RDHATEOAS.Models;
using System.Net.Http;
using System.Collections;
using RDHATEOAS.Rulesets;
using System.Threading.Tasks;
using System.Linq;
using RDHATEOAS.Extensions;
using RDHATEOAS.Builders;

namespace RDHATEOAS.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AddHateoasLinksAttribute : ResultFilterAttribute
    {

        public AddHateoasLinksAttribute(string test)
        {
        }

        private IHateoasRuleset GetControllerBasedLinkRulesets(ResultExecutingContext response)
        {
            var controllerActionDescriptor = response.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                Attribute[] attrs = (Attribute[])controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);
            }
            // TODO: do something with this
            return null;
        }

        //private IHateoasRuleset GetModelBasedLinkRulesets(OkObjectResult okObjectResult) {
        //    if (IsList(okObjectResult))
        //    {
        //        // result is a list
        //        List<Object> list = (List<Object>)okObjectResult.Value;
        //        Attribute[] attrs = System.Attribute.GetCustomAttributes(list.First().GetType());
        //        // TODO: do something with this
        //    }
        //    else
        //    {
        //        // result is a model
        //        Object item = (Object)okObjectResult.Value;
        //        Attribute[] attrs = System.Attribute.GetCustomAttributes(item.GetType());
        //        // TODO: do something with this
        //    }
        //    return null;
        //}

        //private IHateoasRuleset GetGlobalLinkRulesets(OkObjectResult okObjectResult)
        //{
        //    return null;
        //}


        public override void OnResultExecuting(ResultExecutingContext response)
        {
            if (response.Result is OkObjectResult okObjectResult && okObjectResult.StatusCode == 200)
            {
                // TODO: get rulesets
                IHateoasRuleset[] rulesets = new IHateoasRuleset[] { new HateoasRulesetFullLinks() };

                var urlHelper = new UrlHelper(response); // DI not possible?
                var hateoasLinkBuilder = new HateoasLinkBuilder(urlHelper);
                               
                if (okObjectResult.Value.GetType().IsList())
                {
                    Parallel.ForEach((List<Object>)(okObjectResult.Value), (element) =>
                     {
                         // TODO: send to link builder
                     });
                }
                else
                {
                    // HACK: Copy existing properties into dynamic object
                    IDictionary<string, object> itemDynamic = new ExpandoObject();
                    var item = okObjectResult.Value;
                    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item.GetType()))
                    {
                        itemDynamic.Add(property.Name, property.GetValue(item));
                    }

                    // loop through rulesets and add them to dynamic object
                    foreach (IHateoasRuleset ruleset in rulesets)
                    {
                        ruleset.AddDescribedLink(ref itemDynamic);
                    }


                    // set result value to dynamic object
                    okObjectResult.Value = itemDynamic;
                }

                //else if (okObjectResult.Value is PagedSearchDTO<Object> pagedSearch)
                //{
                //    Parallel.ForEach(pagedSearch.List.ToList(), (element) =>
                //    {
                //        // result is a paged search??
                //        //test(element, urlHelper);
                //    });
                //}
            }
            base.OnResultExecuting(response);
        }



        //private HateoasLink[] GenerateLinkObject(IUrlHelper urlHelper, ResultExecutingContext response)
        //{
        //    var builtLink = response.HttpContext.Request.Host.ToUriComponent()
        //        + urlHelper.RouteUrl("Testroute", new {
        //            controller = "person",
        //            id = 1,
        //        });

        //    HateoasLink[] test = {
        //        new HateoasLink(builtLink, "Testlink1", HttpMethod.Get),
        //        new HateoasLink(builtLink, "Testlink2", HttpMethod.Post)
        //    };
        //    return test;
        //}

    }
}