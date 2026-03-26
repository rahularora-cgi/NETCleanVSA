namespace Framework.Application
{
    /// <summary>
    /// Represents a void type for commands that don't return a value
    /// </summary>
    public readonly struct Unit
    {
        public static readonly Unit Value = new();
    }
}