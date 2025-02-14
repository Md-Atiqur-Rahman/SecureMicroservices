using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Movies.Client.HttpHandlers;

//এখানে getToken() মেথড ব্যবহার করা হয়নি, কারণ আমরা Delegating Handler ব্যবহার করব যা স্বয়ংক্রিয়ভাবে টোকেন সংগ্রহ করে সেট করবে।
//এটি আমাদের Http অনুরোধ ইন্টারসেপ্ট করবে এবং Identity Server থেকে টোকেন সংগ্রহ করে সেট করবে।
//এই Delegating Handler এখন প্রতিটি Http অনুরোধের আগে টোকেন সংগ্রহ করে Authorization Header এ সেট করবে।
public class AuthenticationDelegatingHandler: DelegatingHandler 
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ClientCredentialsTokenRequest _tokenRequest;

    public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest tokenRequest)
    {
        _httpClientFactory = httpClientFactory;
        _tokenRequest = tokenRequest;
    }

    //This set method provide to intercepting the Http request before send the request we can apply our main logic on on the on front of these methods.
    protected override  async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("IDPClient");
        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(_tokenRequest);

        if (tokenResponse.IsError)
        {
            throw new HttpRequestException("Something went wrong while requesting the access token");
        }
        request.SetBearerToken(tokenResponse.AccessToken);
        return await base.SendAsync(request, cancellationToken);
    }
}
