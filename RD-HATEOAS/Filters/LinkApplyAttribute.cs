using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace RDHATEOAS.Filters
{
    class LinkApplyAttribute : ResultFilterAttribute
    {
        //private readonly HATEOASConfigServiceContainer configServiceContainer;

        public LinkApplyAttribute()
        {
            // inject service via service container to access global policies
            //configServiceContainer = configServiceContainer;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            AddLinksToResult(context);
            base.OnResultExecuting(context);
        }

        private void AddLinksToResult(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult)
            {

                //var enricher = _hyperMediaFilterOptions.ObjectContentResponseEnricherList.FirstOrDefault(x => x.CanEnrich(context));
                //if (enricher != null) Task.FromResult(enricher.Enrich(context));
            }
        }
    }
}
