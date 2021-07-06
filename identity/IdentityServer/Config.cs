// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Test;
using System;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("employee-apis", "Employee APIs"),
            };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new ApiResource("api-1", "Backend API")
            {
                Scopes = { "employee-apis" }
            }
        };


        public static IEnumerable<Client> Clients(KnownClients knownClients) =>
            new Client[]
            {
                new Client
                {
                    ClientId = "frontend-1",

                    // no interactive user, use the clientid/secret for authentication
                    //AllowedGrantTypes = GrantTypes.Code,
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("frontend-secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "employee-apis"
                    },

                    RedirectUris = knownClients.Clients["frontend-1"].Redirects,
                    RequirePkce = true,
                    AllowOfflineAccess = true,
                    RequireConsent = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 60 * 60 * 7
                }
            };

        public static List<TestUser> Users => new List<TestUser>()
        {
            new TestUser()
            {
                Username = "graeme",
                Password = "p@ssw0rd",
                IsActive = true,
                SubjectId = "12345679",
                ProviderName = "idsrv",
                Claims = new List<Claim>()
                {
                    new Claim("name", "graeme")                }
            },
            new TestUser()
            {
                Username = "fred",
                Password = "p@ssw0rd",
                IsActive = true,
                SubjectId = "12345678",
                ProviderName = "idsrv",
                Claims = new List<Claim>()
                {
                    new Claim("name", "fred")
                }
            },
        };
    }
}