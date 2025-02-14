using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Reflection.Metadata;
using System;
using System.Security.Claims;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Hosting.Server;

namespace IdentityServer;

public class Config
{
    public static IEnumerable<Client> Clients =>
    new Client[]
    {
              // removed these line for for Hybrid flow
             //  Identity Server-এর ConfigureServices মেথডে দুটি ক্লায়েন্ট রয়েছে।
            //  এখন, আমরা Movies Client কমেন্ট আউট করে দেব, কারণ আমরা এটি আর ব্যবহার করছি না।

            /*
            new Client
            {
                ClientId ="movieClient",
                AllowedGrantTypes= GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes={"movieAPI"}
            },
            */

            new Client
                {
                    ClientId = "movies_mvc_client",
                    ClientName = "Movies Mvc Web App",
                    //AllowedGrantTypes = GrantTypes.Code, // আমরা কোড ফ্লো ব্যবহার করব যা লগইন করার সময় টোকেন পেতে সাহায্য করবে।
                    AllowedGrantTypes = GrantTypes.Hybrid, // Add for for Hybrid flow
                    RequirePkce = false, // Add for for Hybrid flow
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>() 
                    {
                        "https://localhost:5002/signin-oidc" //--this as Client app root
                    },
                    PostLogoutRedirectUris = new List<string>() 
                    {
                        "https://localhost:5002/signout-callback-oidc"
                    },
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "movieAPI", //Add for for Hybrid flow
                        "roles" //new claims
                    }
                }

        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("movieAPI","Movie Api")
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
               new IdentityResources.OpenId(),
               new IdentityResources.Profile(),
               new IdentityResources.Address(),
               new IdentityResources.Email(),
               new IdentityResource("roles", "Role", new List<string>() { "role" })
        };

    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                //SubjectId ="0HNACP8VDLMMB:00000007",
                SubjectId ="5BE86359-073C-434B-AD2D-A3932222DABE",
                Username="Himel",
                Password = "1234",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName,"Himel"),// যখন লগইন করবে, তখন আমরা ক্লেমগুলির মাধ্যমে গিভেন নেম এবং ফ্যামিলি নেম অ্যাক্সেস করব।

                                                                //এই তথ্য ক্লেমস হিসেবে সিস্টেমে লগইন করার সময় দেখা যাবে।
                    new Claim(JwtClaimTypes.FamilyName,"ozkaya")
                }
            }   
        };
}
