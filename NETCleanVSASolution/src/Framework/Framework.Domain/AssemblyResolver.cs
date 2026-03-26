namespace Framework.Domain
{
    public static class AssemblyResolver<T> where T : class
    {
        public static Assembly Assembly
        {
            get
            {

                Type T = typeof(T);
                Assembly assembly = typeof(T).Assembly;
                return assembly;
            }
        }
    }
}
