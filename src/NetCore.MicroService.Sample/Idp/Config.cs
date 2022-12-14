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
                //new ApiScope("scope1"),
                ////new ApiScope("scope2"),
                new ApiScope("api1.test.scope"),
                new ApiScope("api1.weather.scope"),
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
                                //Scopes = { "scope1"}
                                Scopes = { "api1.weather.scope", "api1.test.scope" }
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
                            //"scope1",
                            //IdentityServerConstants.StandardScopes.OpenId,
                            //IdentityServerConstants.StandardScopes.Profile,
                            "api1.weather.scope","api1.test.scope"
                        }
                    },
            };

    }
}