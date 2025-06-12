using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Middlewares
{
    public class ValidateModelAndHandleErrorsFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MinValue;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors =
                [
                    .. context
                        .ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage),
                ];

                context.Result = new BadRequestObjectResult(new { Errors = errors });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
