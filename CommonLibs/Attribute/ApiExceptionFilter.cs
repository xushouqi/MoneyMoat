using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace CommonLibs
{
    [AttributeUsage(AttributeTargets.All)]
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        public ApiExceptionFilter(
            ILoggerFactory logFactory,
            IHostingEnvironment hostingEnvironment
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logFactory.CreateLogger("Error");
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(string.Format("Exception={0}\n ExceptionSource={1}\n StackTrace={2}", 
                context.Exception.Message, context.Exception.Source, context.Exception.StackTrace));

            //if (!_hostingEnvironment.IsDevelopment())
            //{
            //    // do nothing
            //    return;
            //}

            //context.ModelState;
            //var result = new ViewResult { ViewName = "CustomError" };
            //result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
            //result.ViewData.Add("Exception", context.Exception);
            //// TODO: Pass additional detailed data via ViewData
            //context.Result = result;
        }
    }

}
