
using LiteDB.Async;

namespace LiteDB.Identity.Async.Database
{
    public interface ILiteDbIdentityAsyncContext
    {
        ILiteDatabaseAsync LiteDatabaseAsync { get; }
    }
}
