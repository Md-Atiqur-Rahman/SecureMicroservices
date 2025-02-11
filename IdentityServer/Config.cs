using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using System.Reflection.Metadata;
using System;
using Duende.IdentityServer.Test;

namespace IdentityServer;

public class Config
{
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
                
        };

    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {

        };
}
