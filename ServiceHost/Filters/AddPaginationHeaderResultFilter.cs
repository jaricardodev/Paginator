using System.Threading.Tasks;
using Jaricardodev.Paginator.Model.Capabilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Jaricardodev.Paginator.Persistence.Filters
{
    public class AddPaginationHeaderResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var result = context.Result as ObjectResult;
            if (result?.Value is IPaginatedList value)
            {
                var paginationHeader = new
                {
                    value.TotalItemsCount,
                    value.TotalPageCount
                };

                context.HttpContext.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));
            }
            await next();
        }
    }
}
