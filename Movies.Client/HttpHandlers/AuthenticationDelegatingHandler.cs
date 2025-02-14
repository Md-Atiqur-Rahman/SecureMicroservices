using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;
using static System.Net.WebRequestMethods;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;
using System;
using Humanizer;
using NuGet.Configuration;
using NuGet.Protocol.Plugins;
using static IdentityModel.OidcConstants;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace Movies.Client.HttpHandlers;

//এখানে getToken() মেথড ব্যবহার করা হয়নি, কারণ আমরা Delegating Handler ব্যবহার করব যা স্বয়ংক্রিয়ভাবে টোকেন সংগ্রহ করে সেট করবে।
//এটি আমাদের Http অনুরোধ ইন্টারসেপ্ট করবে এবং Identity Server থেকে টোকেন সংগ্রহ করে সেট করবে।
//এই Delegating Handler এখন প্রতিটি Http অনুরোধের আগে টোকেন সংগ্রহ করে Authorization Header এ সেট করবে।
public class AuthenticationDelegatingHandler: DelegatingHandler 
{

    //  So in this layer we have a token request.
    //  As you can see that we were a token request and now we will reach the token from OpenID Connect when logging the system.
    //  So for that purpose, I'm going to inject a Http accessor.
    //  So let me comment this line of codes because we are not using a Http client and token request anymore.


    /*
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ClientCredentialsTokenRequest _tokenRequest;

    public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest tokenRequest)
    {
        _httpClientFactory = httpClientFactory;
        _tokenRequest = tokenRequest;
    }
    */
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }



    //This set method provide to intercepting the Http request before send the request we can apply our main logic on on the on front of these methods.
    protected override  async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //  Now we should not send any additional request to identityserver4 for getting token because Token already requested.
        //   One OpenID Connect establishment.
        //    For that purpose, I'm going to I'm going to comment this line, this line of codes.
        //     Because we will not send a request for getting token from the identity server anymore.
        //      Once login the system with OpenID Connect.
        //       We had already getting token and we are going to get token with Http context accessor.

        /*
         var httpClient = _httpClientFactory.CreateClient("IDPClient");
         var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(_tokenRequest);

         if (tokenResponse.IsError)
         {
             throw new HttpRequestException("Something went wrong while requesting the access token");
         }
         request.SetBearerToken(tokenResponse.AccessToken);
        */

        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        if (!string.IsNullOrWhiteSpace(accessToken)) 
        {
            request.SetBearerToken(accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
}
}
