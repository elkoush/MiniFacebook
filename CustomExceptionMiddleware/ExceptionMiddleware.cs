using MiniFacebook.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using MiniFacebook.Repository;

namespace MiniFacebook.CustomExceptionMiddleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILoggerManager _logger;
		public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
		{
			_logger = logger;
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			
			catch (Exception ex)
			{
				_logger.LogError($"Error : {ex}");
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var message = exception switch
			{
				AccessViolationException =>  "Access violation error ",
				_ => "Internal Server Error ."
			};

			await context.Response.WriteAsync(new ErrorDetails()
			{
				StatusCode = context.Response.StatusCode,
				Message = message
			}.ToString());
		}
	}
}
