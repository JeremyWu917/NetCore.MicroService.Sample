# IdentityServer4

> `IdentityServer4` 是为 `ASP.NET Core` 系列量身打造的一款基于 `OpenID Connect` 和 `OAuth 2.0` 认证的框架
> 
> `IdentityServer4` 官方文档：[https://identityserver4.readthedocs.io/](https://identityserver4.readthedocs.io/)

## Postman 获取 Access Token

请求地址 `https://localhost:5001/connect/token` ，请求参数如下

| key           | value              |
| ------------- | ------------------ |
| client_id     | jeremy             |
| client_secret | secret             |
| grant_type    | client_credentials |
| scope         | scope1             |

**注意**

1. 请求地址根据实际情况进行调整
   
   1. 是否启用 `https`
   
   2. 是否修改默认端口

2. 请求参数根据实际情况进行调整
   
   1. `client_id` 用户自定义的 `ID`
   
   2. `client_secret` 用户自定义的 `secret`
   
   3. `grant_type` 用户自定义的授权类型
   
   4. `scope` 用户自定义的范围

3. 案例中的 `Config` 如下：

```csharp
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                //new ApiScope("scope2"),
            };

        //public static IEnumerable<ApiResource> ApiResources =>
        //    new ApiResource[]
        //    {
        //            new ApiResource("api1","#api1")
        //            {
        //                //!!!重要
        //                Scopes = { "scope1"}
        //            },
        //        new ApiResource("api2","#api2")
        //        {
        //            //!!!重要
        //            Scopes = { "scope2"}
        //        },
        //    };

        //public static IEnumerable<Client> Clients =>
        //    new Client[]
        //    {
        //        // m2m client credentials flow client
        //        new Client
        //        {
        //            ClientId = "m2m.client",
        //            ClientName = "Client Credentials Client",

        //            AllowedGrantTypes = GrantTypes.ClientCredentials,
        //            ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

        //            AllowedScopes = { "scope1" }
        //        },

        //        // interactive client using code flow + pkce
        //        new Client
        //        {
        //            ClientId = "interactive",
        //            ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

        //            AllowedGrantTypes = GrantTypes.Code,

        //            RedirectUris = { "https://localhost:44300/signin-oidc" },
        //            FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
        //            PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

        //            AllowOfflineAccess = true,
        //            AllowedScopes = { "openid", "profile", "scope2" }
        //        },
        //    };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                            new ApiResource("api1","#api1")
                            {
                                //!!!重要
                                Scopes = { "scope1"}
                            },
                //new ApiResource("api2","#api2")
                //{
                //    //!!!重要
                //    Scopes = { "scope2"}
                //},
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                    new Client
                    {
                        ClientId = "jeremy",
                        ClientName = "JeremyWu",

                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets = { new Secret("secret".Sha256()) },

                        AllowedScopes = { 
                            "scope1",
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                        }
                    },
            };

    }
}
```

4. 案例中的 `Startup`

```csharp
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Idp
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddTestUsers(TestUsers.Users);

            // in-memory, code config
            //builder.AddInMemoryIdentityResources(Config.IdentityResources);
            //builder.AddInMemoryApiScopes(Config.ApiScopes);
            //builder.AddInMemoryClients(Config.Clients);

            // in-memory, code config
            builder.AddInMemoryIdentityResources(Config.IdentityResources);
            builder.AddInMemoryApiScopes(Config.ApiScopes);
            //添加API资源
            builder.AddInMemoryApiResources(Config.ApiResources);
            builder.AddInMemoryClients(Config.Clients);


            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
```



**注意**

- 在启用 `Token` 认证鉴权的客户端中，授权回调的 `redirect_uris` 基础地址和端口，应该和客户端的基础地址和端口相同
