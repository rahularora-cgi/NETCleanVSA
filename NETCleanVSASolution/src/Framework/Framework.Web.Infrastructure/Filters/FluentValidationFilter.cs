namespace Framework.Infrastructure.Web.Filters
{
    // Runs FluentValidation for a given model type and short-circuits with 400 if invalid
    public class FluentValidationFilter<T> : IAsyncActionFilter where T : class
    {
        private readonly IValidator<T> _validator;

        public FluentValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // Find the action argument of type T
            if (!context.ActionArguments.Values.OfType<T>().Any())
            {
                await next();
                return;
            }

            var model = context.ActionArguments.Values.OfType<T>().First();

            ValidationResult result = await _validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                var modelState = context.ModelState;

                foreach (var error in result.Errors)
                {
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                context.Result = new BadRequestObjectResult(
                    new ValidationProblemDetails(modelState));
                return; // short-circuit
            }

            await next();
        }
    }
}