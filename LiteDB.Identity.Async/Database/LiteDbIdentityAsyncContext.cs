using System;
using LiteDB.Async;
using LiteDB.Identity.Database;

namespace LiteDB.Identity.Async.Database
{
    public sealed class LiteDbIdentityAsyncContext : ILiteDbIdentityAsyncContext, IDisposable
    {
        private readonly LiteDatabaseAsync liteDatabaseAsync;
        public LiteDbIdentityAsyncContext(string connectionStringName)
        {
            try
            {
                if(string.IsNullOrEmpty(connectionStringName))
                {
                    throw new ArgumentNullException("LiteDbIdentity", "LiteDbIdentity connection string is missing in appsettings.json configuration file");
                }

                liteDatabaseAsync = new LiteDatabaseAsync(connectionStringName, LiteDbIdentityMapper.GetMapper());
            }
            catch (Exception)
            {
                // add logger 
                throw;
            }
        }

        public ILiteDatabaseAsync LiteDatabaseAsync
        {
            get
            {
                ThrowIfDisposed();
                return liteDatabaseAsync;
            }
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #region IDisposable implementation 

        private bool disposed = false;

        // Public implementation of Dispose pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                liteDatabaseAsync.Dispose();
            }

            disposed = true;
        }

        ~LiteDbIdentityAsyncContext()
        {
            Dispose(false);
        }

        #endregion

    }
}
