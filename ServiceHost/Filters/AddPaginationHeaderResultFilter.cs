using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaricardodev.Paginator.Model.Capabilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServiceHost.Filters
{
    public class AddPaginationHeaderResultFilter : IAsyncResultFilter
    {
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            throw new NotImplementedException();

            /* var result = context.Result as ObjectResult;
             if (result?.Value is IPaginatedList value)
                 result.Value = new
                 {
                     Id = value.Id,
                     Username = value.Username,
                     Fullname = value.Fullname,
                     Mobile = value.Mobile,
                     Email = value.Email,
                     UpdatedAt = value.UpdatedAt
                 };
             await next();*/
        }
    }
}
