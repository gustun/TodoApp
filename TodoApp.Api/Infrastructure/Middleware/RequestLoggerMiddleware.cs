using Microsoft.AspNetCore.Http;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;
using TodoApp.Api.ViewModels.Base;
using TodoApp.Common.Models.Base;
using TodoApp.Common.Enums;
using System.Net;
using Newtonsoft.Json;
using TodoApp.Common;
using System.Collections.Generic;

namespace TodoApp.Api.Infrastructure.Middleware
{
    public class RequestLoggerMiddleware
    {
        const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        const string ErrorMessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms. ErrorTraceId: {ErrorTraceId}";

        static readonly ILogger Log = Serilog.Log.ForContext<RequestLoggerMiddleware>();

        readonly RequestDelegate _next;

        public RequestLoggerMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var sw = Stopwatch.StartNew();
            try
            {
                await _next(httpContext);
                sw.Stop();

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : Log;
                log.Write(level, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, statusCode, sw.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, sw);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex, Stopwatch sw)
        {
            sw.Stop();
            var errorGuid = Guid.NewGuid();
            LogForErrorContext(httpContext)
                .Error(ex, ErrorMessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, sw.Elapsed.TotalMilliseconds, errorGuid);

            var exceptionResponse = new ErrorResponse
            {
                ErrorTraceId = errorGuid.ToString(),
                Messages = new List<Notification>
                {
                    new Notification {Message = "Unexpected error occurred.", NotificationType = ENotificationType.Error}
                }
            };

            httpContext.Response.StatusCode = HttpStatusCode.InternalServerError.ToInt();
            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(exceptionResponse));
        }

        static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

            return result;
        }
    }
}
