using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrecheManagement.API.Handlers;

public class ExceptionsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Console.WriteLine(context.Exception);

        if (context.Exception is CrecheManagementException crecheException)
        {
            context.HttpContext.Response.StatusCode = (int)crecheException.StatusCode;
            context.Result = new ObjectResult(new ErrorResponse()
            {
                StatusCode = (int)crecheException.StatusCode,
                ErrorMessage = crecheException.Message,
            });

            return;
        }

        if (context.Exception is ValidationException validationException)
        {
            context.HttpContext.Response.StatusCode = 400;
            context.Result = new ObjectResult(new ErrorResponse()
            {
                StatusCode = 400,
                ErrorMessage = ReturnMessages.DEFAULT_VALIDATION_ERROR,
                Errors = validationException.Errors.Select(x => new ErrorValidation()
                {
                    Field = x.PropertyName,
                    Message = x.ErrorMessage,
                }).ToList()
            });

            return;
        }

        context.HttpContext.Response.StatusCode = 400;
        context.Result = new ObjectResult(new ErrorResponse()
        {
            StatusCode = 500,
            ErrorMessage = ReturnMessages.DEFAULT_INTERNAL_ERROR,
        });
    }
}