using Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRMS_API.Filters;

/// <summary>
/// Action filter that automatically validates ModelState before any controller action executes.
/// Returns a standardised 400 Bad Request using ApiResponse when the model is invalid.
/// Apply globally via MVC options, or per-controller/action via [ValidateModel].
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .SelectMany(ms => ms.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var response = ApiResponse.Fail("One or more validation errors occurred.", errors);

            context.Result = new BadRequestObjectResult(response);
        }
    }
}
