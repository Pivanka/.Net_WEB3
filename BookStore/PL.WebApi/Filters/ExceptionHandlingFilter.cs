using System.Net;
using System.Net.Mime;
using BLL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Serialization;

namespace PL.WebApi.Filters
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exceptionContextResult = new ContentResult
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ContentType = MediaTypeNames.Application.Json
            };

            dynamic errorMessage = context.Exception switch
            {
                NotFoundException ex => new
                {
                    Error = ex.ErrorType,
                    Message = ex.Message
                },
                ArgumentNullException ex => new
                {
                    Error = ErrorType.EmptyCart,
                    Message = ex.Message
                },
                InvalidUserCredentialsException ex => new
                {
                    Error = ex.ErrorType,
                    Message = ex.Message
                },
                _ => new
                {
                    Error = ErrorType.Unknown,
                    Message = context.Exception.Message
                },
            };
            
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            exceptionContextResult.Content = Newtonsoft.Json.JsonConvert.SerializeObject(errorMessage, new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Newtonsoft.Json.Formatting.Indented
            });

            context.Result = exceptionContextResult;
            context.ExceptionHandled = true;
        }
    }
}
