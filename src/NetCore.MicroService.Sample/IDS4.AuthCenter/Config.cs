﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IDS4.AuthCenter
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
                new ApiScope("orderApiScope"),
                new ApiScope("productApiScope"),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                    new ApiResource("orderApi","订单服务")
                    {
                        ApiSecrets ={ new Secret("orderApi secret".Sha256()) },
                        Scopes = { "orderApiScope" }
                    },
                    new ApiResource("productApi","产品服务")
                    {
                        ApiSecrets ={ new Secret("productApi secret".Sha256()) },
                        Scopes = { "productApiScope" }
                    }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "jeremy",
                    ClientName = "JeremyWu",

                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { "https://localhost:5001/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "orderApiScope", "productApiScope"
                    },
                    AllowAccessTokensViaBrowser = true,

                    RequireConsent = true,//是否显示同意界面
                    AllowRememberConsent = false,//是否记住同意选项
                }
            };
    }
}