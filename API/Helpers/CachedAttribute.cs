using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cachedService = context.HttpContext.RequestServices.GetRequiredService<ICachedResponseService>();
            var cachedKey = GenerateCachedKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await  cachedService.GetCachedResponseAsync(cachedKey);

            if(!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };

                context.Result = contentResult;

                return;
            }

            var executedContext = await next();

            if(executedContext.Result is OkObjectResult okObjectResult)
            {
                await cachedService.CachedResponseAsync(cachedKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private string GenerateCachedKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach(var (key, value) in request.Query.OrderBy(q => q.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}