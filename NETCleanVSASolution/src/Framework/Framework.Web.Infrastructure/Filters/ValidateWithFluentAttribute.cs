namespace Framework.Infrastructure.Web.Filters
{
    public class ValidateWithFluentAttribute<T> : TypeFilterAttribute where T : class
    {
        public ValidateWithFluentAttribute() : base(typeof(FluentValidationFilter<T>))
        {
        }
    }
}