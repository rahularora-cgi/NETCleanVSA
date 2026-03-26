namespace Framework.Domain
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        IEnumerable<TDestination> Map(IEnumerable<TSource> sources);
    }

    public class Mapper<TSource, TDestination> : IMapper<TSource, TDestination>
        where TDestination : new()
    {
        private static readonly PropertyInfo[] SourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        private static readonly PropertyInfo[] DestinationProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        public TDestination Map(TSource source)
        {
            if (source == null)
                return default!;

            var destination = new TDestination();

            foreach (var sourceProperty in SourceProperties)
            {
                if (!sourceProperty.CanRead)
                    continue;

                var destinationProperty = Array.Find(DestinationProperties, p =>
                    p.Name == sourceProperty.Name &&
                    p.CanWrite &&
                    p.PropertyType.IsAssignableFrom(sourceProperty.PropertyType));

                if (destinationProperty != null)
                {
                    var value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }

            return destination;
        }

        public IEnumerable<TDestination> Map(IEnumerable<TSource> sources)
        {
            if (sources == null)
                yield break;

            foreach (var source in sources)
            {
                yield return Map(source);
            }
        }
    }

    public static class MapperExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TDestination : new()
        {
            var mapper = new Mapper<TSource, TDestination>();
            return mapper.Map(source);
        }

        public static IEnumerable<TDestination> MapTo<TSource, TDestination>(this IEnumerable<TSource> sources)
            where TDestination : new()
        {
            var mapper = new Mapper<TSource, TDestination>();
            return mapper.Map(sources);
        }

        public static TDto ToDto<TDto>(this object source)
            where TDto : new()
        {
            if (source == null)
                return default!;

            var mapperType = typeof(Mapper<,>).MakeGenericType(source.GetType(), typeof(TDto));
            var mapper = Activator.CreateInstance(mapperType);
            var mapMethod = mapperType.GetMethod(nameof(IMapper<object, object>.Map), new[] { source.GetType() });
            return (TDto)mapMethod!.Invoke(mapper, new[] { source })!;
        }

        public static IEnumerable<TDto> ToDto<TDto>(this IEnumerable<object> sources)
            where TDto : new()
        {
            if (sources == null)
                yield break;

            foreach (var source in sources)
            {
                yield return source.ToDto<TDto>();
            }
        }

        public static TEntity ToEntity<TEntity>(this object source)
            where TEntity : new()
        {
            if (source == null)
                return default!;

            var mapperType = typeof(Mapper<,>).MakeGenericType(source.GetType(), typeof(TEntity));
            var mapper = Activator.CreateInstance(mapperType);
            var mapMethod = mapperType.GetMethod(nameof(IMapper<object, object>.Map), new[] { source.GetType() });
            return (TEntity)mapMethod!.Invoke(mapper, new[] { source })!;
        }

        public static IEnumerable<TEntity> ToEntity<TEntity>(this IEnumerable<object> sources)
            where TEntity : new()
        {
            if (sources == null)
                yield break;

            foreach (var source in sources)
            {
                yield return source.ToEntity<TEntity>();
            }
        }
    }
}
