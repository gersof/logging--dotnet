using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.WebAPI.Middlewares
{
    public class LogResponseMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly Serilog.ILogger _logger;
        const string MessageTemplate = "HTTP  responded {StatusCode} {ResponseBody} in {Elapsed:0.0000} ms";

        static readonly Serilog.ILogger Log = Serilog.Log.ForContext<LogResponseMiddleware>();

        static readonly HashSet<string> HeaderWhitelist = new HashSet<string> { "Content-Type", "Content-Length", "User-Agent" };
        //private Func _defaultFormatter = (state, exception) => state;

        public LogResponseMiddleware(RequestDelegate next/*, Serilog.ILogger logger*/)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var bodyStream = context.Response.Body;

            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();
            var statusCode = context.Response?.StatusCode;
            var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;
            var log = level == LogEventLevel.Error ? LogForErrorContext(context) : Log;

            log.Write(level, MessageTemplate, statusCode, responseBody);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(bodyStream);
        }


        static Serilog.ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var loggedHeaders = request.Headers
                .Where(h => HeaderWhitelist.Contains(h.Key))
                .ToDictionary(h => h.Key, h => h.Value.ToString());

            var result = Log
                .ForContext("RequestHeaders", loggedHeaders, destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            return result;
        }
    }
}
