using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Api.Shared;
using Api.Shared.Exceptions;

namespace Api.Middleware
{
    public class MainMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ErrorLogService _errorLogService;

        public static Func<HttpContext, string> GetQueryString =>
            it => it.Request.QueryString.Value.Replace("?", string.Empty);

        public static Func<ServiceException.CodeError, string> GetErrorName =>
            it => Enum.GetName(typeof(ServiceException.CodeError), it);

        public MainMiddleware(RequestDelegate next, ErrorLogService errorLogService)
        {
            _next = next;
            _errorLogService = errorLogService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ServiceException ex)
            {
                _errorLogService.AddNote(GetQueryString(context), GetErrorName(ex.ErrorCode));
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _errorLogService.AddNote(GetQueryString(context), GetErrorName(ServiceException.CodeError.Error));
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var result = JsonConvert.SerializeObject(FormatResponse.Get(ex));
            var code = ex is ServiceException
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
