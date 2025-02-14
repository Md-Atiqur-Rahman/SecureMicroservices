using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Movies.Client.ApiServices;
using System.Reflection.Metadata;
using System.Security.Policy;
using Microsoft.Net.Http.Headers;
using Movies.Client.HttpHandlers;
using IdentityModel.Client;
using IdentityModel;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMovieAPIService, MovieAPIService>();
// Add services to the container.

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://localhost:5005";
    options.ClientId = "movies_mvc_client";
    options.ClientSecret = "secret";
    //options.ResponseType = "code";
    options.ResponseType = "code id_token";//for Hybrid flow

    //options.Scope.Add("openid");
    //options.Scope.Add("profile");
    options.Scope.Add("address");
    options.Scope.Add("email");
    options.Scope.Add("movieAPI");//for Hybrid flow
    options.Scope.Add("roles");//added new claims
    options.ClaimActions.MapUniqueJsonKey("role", "role");

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.GivenName,
        RoleClaimType = JwtClaimTypes.Role
    };
});




builder.Services.AddTransient<AuthenticationDelegatingHandler>();

builder.Services.AddHttpClient("MovieApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
})
.AddHttpMessageHandler<AuthenticationDelegatingHandler>();// Delegating Handler যুক্ত করব যাতে এটি স্বয়ংক্রিয়ভাবে টোকেন সংগ্রহ করতে পারে।
//এতে HttpClient একবার তৈরি হওয়ার সময়ই সব কনফিগারেশন সেট করা হবে, ফলে প্রতিবার URL উল্লেখ করার দরকার নেই।

// create for http client used for accessing the IDP
builder.Services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5005/");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

//builder.Services.AddSingleton(
//    new ClientCredentialsTokenRequest
//    {
//        Address = "https://localhost:5005/connect/token",
//        ClientId = "movieClient",
//        ClientSecret = "secret",
//        Scope = "movieAPI"
//    });

//  এখন আর সেই অপারেশনগুলোর প্রয়োজন নেই, তাই আমি সেই সার্ভিসগুলিকে কমেন্ট আউট করে দেব।
//  আমরা এখন Identity Server থেকে নতুন করে টোকেন নেওয়ার পরিবর্তে লগইনের সময় পাওয়া টোকেনটি ব্যবহার করব।
//  এখানে, আমরা বিদ্যমান HttpContext ব্যবহার করছি এবং আলাদাভাবে Identity Server-এ নতুন রিকোয়েস্ট পাঠানোর দরকার নেই।
//  আমরা এখন আর Movies Client ব্যবহার করছি না, কারণ আমরা শুধুমাত্র MVC Client ব্যবহার করছি।

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
