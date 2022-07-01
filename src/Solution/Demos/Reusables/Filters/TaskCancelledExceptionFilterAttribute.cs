using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Reusables.Filters
{
    public class TaskCancelledExceptionFilterAttribute : IAsyncExceptionFilter, IExceptionFilter
    {
        private readonly ILogger _logger;
        public TaskCancelledExceptionFilterAttribute(ILogger<TaskCancelledExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not TaskCanceledException)
            {
                return;
            }

            _logger.LogWarning("Request was cancelled");
            context.Result = new StatusCodeResult(499);
            context.ExceptionHandled = true;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is not TaskCanceledException)
            {
                return Task.CompletedTask;
            }

            _logger.LogWarning("Request was cancelled");
            context.Result = new StatusCodeResult(499);
            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
