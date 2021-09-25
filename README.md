LiteDB.Identity.Async
======= 
I only added the async wrapper and adapted everything to it.  
Now you can simply use the async functions.  
[Nuget](https://www.nuget.org/packages/LiteDB.Identity.Async/)

Usage:
=======

Add default LiteDb.Identity.Async implementation in ConfigureServices method:
```csharp
        public void ConfigureServices(IServiceCollection services)
        {

            string connectionString = Configuration.GetConnectionString("IdentityLiteDB");
            services.AddLiteDbIdentityAsync(connectionString).AddDefaultTokenProviders().AddDefaultUI();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }
```

```
Usage of Database:
     
     ILiteDbIdentityAsyncContext _liteDb;
      await _liteDb.LiteDatabaseAsync.GetCollection<Map>("Map")
                .FindOneAsync(x => x.Id == id);


Usage of Authorize


    [Authorize]
    [Authorize(Roles = "Admin")]

```

Following interfaces has been implemented on :
- UserStore :
```csharp
    public class UserStoreAsync<TUser, TRole, TUserRole, TUserClaim, TUserLogin, TUserToken> : 
                                    IUserLoginStore<TUser>, 
                                    IUserStore<TUser>,
                                    IUserRoleStore<TUser>,
                                    IUserClaimStore<TUser>, 
                                    IUserPasswordStore<TUser>, 
                                    IUserSecurityStampStore<TUser>, 
                                    IUserEmailStore<TUser>, 
                                    IUserLockoutStore<TUser>, 
                                    IUserPhoneNumberStore<TUser>, 
                                    IQueryableUserStore<TUser>, 
                                    IUserTwoFactorStore<TUser>,
                                    IUserAuthenticationTokenStore<TUser>,
                                    IUserAuthenticatorKeyStore<TUser>,
                                    IUserTwoFactorRecoveryCodeStore<TUser>
```
- RoleStore :
```csharp
    public class RoleStoreAsync<TRole, TRoleClaim> : IQueryableRoleStore<TRole>, 
                                                IRoleStore<TRole>, 
                                                IRoleClaimStore<TRole>
```

You will find more examples in the repo:  

[PavlovRconWebserver](https://github.com/devinSpitz/PavlovRconWebserver)  

ps. currently only on the TestBranch.


Donate:
=======
Feel free to support my work by donating:

<a href="https://www.paypal.com/donate?hosted_button_id=JYNFKYARZ7DT4">
<img src="https://www.paypalobjects.com/en_US/CH/i/btn/btn_donateCC_LG.gif" alt="Donate with PayPal" />
</a>

Business:
=======

For business inquiries please use:

<a href="mailto:&#x64;&#x65;&#x76;&#x69;&#x6e;&#x40;&#x73;&#x70;&#x69;&#x74;&#x7a;&#x65;&#x6e;&#x2e;&#x73;&#x6f;&#x6c;&#x75;&#x74;&#x69;&#x6f;&#x6e;&#x73;">&#x64;&#x65;&#x76;&#x69;&#x6e;&#x40;&#x73;&#x70;&#x69;&#x74;&#x7a;&#x65;&#x6e;&#x2e;&#x73;&#x6f;&#x6c;&#x75;&#x74;&#x69;&#x6f;&#x6e;&#x73;</a>


References:
=======
- LiteDB.Identity - [https://github.com/quicksln/LiteDB.Identity#](https://github.com/quicksln/LiteDB.Identity#)
- Async Wrapper - [https://github.com/mlockett42/litedb-asyncc#](https://github.com/mlockett42/litedb-async#)
- LiteDB - [https://www.litedb.org/](https://www.litedb.org/)
- LiteDB Github - [https://github.com/mbdavid/LiteDB](https://github.com/mbdavid/LiteDB)
- AspNetCore Identity - [Introduction](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio)
- AspNetCore Github - [https://github.com/dotnet/aspnetcore/tree/master/src/Identity](https://github.com/dotnet/aspnetcore/tree/master/src/Identity)


Where to use it ?
=======
- Great for small and medium size AspNetCore Websites,
- Quick implementation of Authentication and Authorization mechanism for WebAPIs.

License
=======

[MIT](http://opensource.org/licenses/MIT)


