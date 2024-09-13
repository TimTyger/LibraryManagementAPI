using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI_Service.Helpers
{
    public class PerformanceLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceLogger> _logger;

        public PerformanceLogger(RequestDelegate next, ILogger<PerformanceLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var requestId = Guid.NewGuid().ToString();
            var request = context.Request;
            _logger.LogInformation($"Request Method: {request.Method} | Request Path: {request.Path} | Request Body: {await ReadRequestBodyAsync(request)} | RequestId: {requestId}");

            var originalResponseBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var response = context.Response;
                _logger.LogInformation($"Response Status Code: {response.StatusCode} | Response Body: {await ReadResponseBodyAsync(response)} | RequestId: {requestId}");

                // Reset the response body stream position and copy it back to the original stream
                await responseBody.CopyToAsync(originalResponseBodyStream);
            }

            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation($"Request Path: {context.Request.Path} | Execution Time: {elapsedMs} ms. | RequestId: {requestId}");
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        private async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin); 
            return body;
        }
    }

}
