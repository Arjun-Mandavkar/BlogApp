namespace BlogApp.Utilities.MappingUtils
{
    public interface IMapper<TSource, TTarget>
    {
        public TTarget Map(TSource source);
    }
}
