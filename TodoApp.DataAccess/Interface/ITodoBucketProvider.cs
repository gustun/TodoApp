using Couchbase.Extensions.DependencyInjection;

namespace TodoApp.DataAccess.Interface
{
    // with that, I can use multiple buckets in my repository clasess.
    public interface ITodoBucketProvider : INamedBucketProvider
    {
    }
}
