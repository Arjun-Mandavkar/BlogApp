namespace BlogApp.Services.MappingServices
{
    public interface IMapper<TSource, TTarget>
    {
        public TTarget Map(TSource source);
    }
}
