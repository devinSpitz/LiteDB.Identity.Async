using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;
using LiteDB.Async;
using LiteDB.Identity.Async.Database;
using LiteDB.Identity.Database;
using LiteDB.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace LiteDB.Identity.Async
{
   public class RoleStoreAsync<TRole, TRoleClaim> : IQueryableRoleStore<TRole>, 
                                                IRoleStore<TRole>, 
                                                IRoleClaimStore<TRole>, 
                                                IDisposable
        where TRole : LiteDbRole, new()
        where TRoleClaim : LiteDbRoleClaim, new()
    {
        private readonly ILiteCollectionAsync<TRole> roles;
        private readonly ILiteCollectionAsync<TRoleClaim> roleClaim;
        public RoleStoreAsync(ILiteDbIdentityAsyncContext dbContext) 
        {
            this.roles = dbContext.LiteDatabaseAsync.GetCollection<TRole>(typeof(TRole).Name);
            this.roleClaim = dbContext.LiteDatabaseAsync.GetCollection<TRoleClaim>(typeof(TRoleClaim).Name);
        }

        public IQueryable<TRole> Roles => roles.FindAllAsync().GetAwaiter().GetResult().AsQueryable();

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            await roles.InsertAsync(role);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            await roles.DeleteAsync(role.Id);

            return IdentityResult.Success;
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (roleId == null)
            {
                throw new ArgumentNullException(nameof(roleId));
            }

            var result = await roles.FindByIdAsync(new ObjectId(roleId));

            return result;
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (normalizedRoleName == null)
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            var result = await roles.FindOneAsync(r=> r.NormalizedName.Equals(normalizedRoleName, StringComparison.InvariantCultureIgnoreCase) ||
                    r.Name.Equals(normalizedRoleName, StringComparison.InvariantCultureIgnoreCase));

            return result;
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Id == null ? null : role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.NormalizedName = normalizedName.ToUpper();

            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.Name = roleName;
            roles.UpdateAsync(role);

            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            roles.UpdateAsync(role);

            return Task.FromResult(IdentityResult.Success);
        }

        public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var roleClaims = (await  roleClaim.FindAsync(c => c.RoleId == role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();

            return roleClaims;
        }

        public Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var newRoleClaim = new TRoleClaim { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
            roleClaim.InsertAsync(newRoleClaim);

            return Task.CompletedTask;
        }

        public async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var claimsToRemove = await roleClaim.Query().Where(r => r.RoleId.Equals(role.Id) && r.ClaimValue == claim.Value && r.ClaimType == claim.Type).ToListAsync();
            
            if(claimsToRemove.Any())
            {
                foreach(var rc in claimsToRemove)
                {
                    await roleClaim.DeleteAsync(rc.Id);
                }
            }

        }


        protected void ThrowIfDisposed()
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
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
     
            }

            disposed = true;
        }

        ~RoleStoreAsync()
        {
            Dispose(false);
        }

        #endregion
    }
}