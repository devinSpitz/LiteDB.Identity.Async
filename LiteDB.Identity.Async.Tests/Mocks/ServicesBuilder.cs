using LiteDB.Identity.Async.Database;
using LiteDB.Identity.Async.Extensions;
using LiteDB.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;

namespace LiteDB.Identity.Async.Tests.Mocks
{
    internal class ServicesBuilder : IServicesBuilder
    {
        private readonly IServiceCollection services;
        private ServiceProvider provider;
        public ServicesBuilder()
        {
            services = new ServiceCollection();
        }
        public void Build()
        {
            services.AddHttpContextAccessor();
            services.AddLogging();
            services.AddLiteDbIdentityAsync("Filename=:memory:;");
            provider = services.BuildServiceProvider();
        }

        public RoleManager<LiteDbRole> GetRoleManager()
        {
            return provider.GetService<RoleManager<LiteDbRole>>();
        }

        public UserManager<LiteDbUser> GetUserManager()
        {
            return provider.GetService<UserManager<LiteDbUser>>();
        }
    }
}
