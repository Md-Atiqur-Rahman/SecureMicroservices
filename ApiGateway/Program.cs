using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot.json configuration file
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);


builder.Services.AddAuthentication().AddJwtBearer("IdentityApiKey", x =>
{
    x.Authority = "https://localhost:5005";
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };
});


// Add Ocelot services
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();



app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Use Ocelot middleware
await app.UseOcelot();

app.Run();
