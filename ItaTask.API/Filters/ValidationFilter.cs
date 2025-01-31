using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ItaTask.API.Filters;

public class ValidationFilter(ILogger<ValidationFilter> logger) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Invalid request data",
                Errors = errors
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            logger.LogWarning("FluentValidation failed: {Message}", validationException.Message);

            var errors = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Validation failed",
                Errors = errors
            });

            context.ExceptionHandled = true;
        }
    }
}