using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Serilog.Context;

namespace Api.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private const string CorrelationHeader = "X-Correlation-ID";
        public async Task InvokeAsync(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
        {
            var correlationId = context.Request.Headers[CorrelationHeader].FirstOrDefault()
                                ?? Guid.NewGuid().ToString();
            logger.LogInformation("Correlation id : {correlationId}", correlationId);
            context.Response.Headers[CorrelationHeader] = correlationId;
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                context.Items["CorrelationId"] = correlationId;
                await _next(context);
            }
        }
    }
}