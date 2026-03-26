namespace Framework.Domain
{
    public sealed record Error(string Code, string Message)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static Error NullValue => new("Error.NullValue", "The specified value is null.");

        public static Error NotFound(string entityName, object id) =>
            new($"{entityName}.NotFound", $"{entityName} with id '{id}' was not found.");

        public static Error Validation(string message) =>
            new("Error.Validation", message);

        public static Error Conflict(string message) =>
            new("Error.Conflict", message);

        public static Error Unauthorized(string message = "Unauthorized access.") =>
            new("Error.Unauthorized", message);

        public static Error Forbidden(string message = "Forbidden access.") =>
            new("Error.Forbidden", message);

        public static Error Custom(string code, string message) =>
            new(code, message);
    }
}