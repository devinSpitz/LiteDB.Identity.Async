using LiteDB.Identity.Models;
using LiteDB.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using LiteDB.Identity.Database;
using System;
using LiteDB.Identity.Async.Database;

namespace LiteDB.Identity.Async.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="IServiceCollection"/> to add identity LiteDB stores.
    /// </summary>
    public static class IdentityLiteDbAsyncBuilderExtensions
    {
        /// <summary>
        /// Adds LiteDB identity default configuration to IServiceCollection.
        /// </summary>
        public static IdentityBuilder AddLiteDbIdentityAsync(this IServiceCollection builder, Action<LiteDbIdentityAsyncOptions> configuration)
        {
            var options = new LiteDbIdentityAsyncOptions();
            configuration?.Invoke(options);

            if (string.IsNullOrEmpty(options.ConnectionString))
            {
                throw new ArgumentNullException(nameof(options.ConnectionString));
            }

            builder.AddScoped<ILiteDbIdentityAsyncContext, LiteDbIdentityAsyncContext>(c => new LiteDbIdentityAsyncContext(options.ConnectionString));

            return ConfigureStors(builder);
        }

        /// <summary>
        /// Adds LiteDB identity default configuration to IServiceCollection.
        /// </summary>
        public static IdentityBuilder AddLiteDbIdentityAsync(this IServiceCollection builder, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            builder.AddScoped<ILiteDbIdentityAsyncContext, LiteDbIdentityAsyncContext>(c => new LiteDbIdentityAsyncContext(connectionString));

            return ConfigureStors(builder);
        }

        private static IdentityBuilder ConfigureStors(IServiceCollection builder)
        {
            // Identity stores
            builder.TryAddScoped<IUserStore<LiteDbUser>, UserStoreAsync<LiteDbUser, LiteDbRole, LiteDbUserRole, LiteDbUserClaim, LiteDbUserLogin, LiteDbUserToken>>();
            builder.TryAddScoped<IRoleStore<LiteDbRole>, RoleStoreAsync<LiteDbRole, LiteDbRoleClaim>>();

            var identityBuilder = builder.AddIdentity<LiteDbUser, LiteDbRole>();

            return identityBuilder;
        }
    }

}