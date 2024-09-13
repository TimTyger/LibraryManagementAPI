using LibraryAPI_Service.Models.ResponseDto;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI_Service.Helpers
{
    public class ErrorHandler
    { 
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandler> _logger;

        public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
                {
                    var modelState = context.Features.Get<ModelStateDictionary>();

                    if (modelState!=null && !modelState.IsValid)
                    {
                        
                            context.Response.ContentType = "application/json";
                            var errorResponse = new GenericResponse<Dictionary<string, string[]>>(false,"Model State Errors", modelState.ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()));

                            await context.Response.WriteAsJsonAsync(errorResponse);
                            return;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Handle the exception
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorResponse = new GenericResponse<string>(false, "Unexpected Error Occurred", "Error occurred");
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }

}
