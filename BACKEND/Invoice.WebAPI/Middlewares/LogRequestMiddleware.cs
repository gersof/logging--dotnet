using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Serilog.Events;
using System.IO;
using System.Threading.Tasks;

namespace Invoice.WebAPI.Middlewares
{
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate next;
        const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} {RequestBody} ";

        static readonly Serilog.ILogger Log = Serilog.Log.ForContext<LogRequestMiddleware>();

        public LogRequestMiddleware(RequestDelegate next)
        {
            this.next = next;
            //_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestBodyStream = new MemoryStream();
            var originalRequestBody = context.Request.Body;

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var url = UriHelper.GetDisplayUrl(context.Request);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();


            Log.Write(LogEventLevel.Information, MessageTemplate, context.Request.Method, GetPath(context), requestBodyText);

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            await next(context);
            context.Request.Body = originalRequestBody;
        }

        static string GetPath(HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget ?? httpContext.Request.Path.ToString();
        }
    }
}
