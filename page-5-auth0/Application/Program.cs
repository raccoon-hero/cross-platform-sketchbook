using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CROSS_Task5.Support;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Threading.Tasks;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var auth0Settings = builder.Configuration.GetSection("Auth0");


builder.Services
    .AddAuthentication(options =>
    {
        // Set the default scheme
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "Auth0";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("Auth0", options =>
    {
        options.Authority = $"https://{auth0Settings["Domain"]}";
        options.ClientId = auth0Settings["ClientId"];
        options.ClientSecret = auth0Settings["ClientSecret"];

        options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/callback");

        options.ClaimsIssuer = "Auth0";

        options.Events = new OpenIdConnectEvents
        {
            OnTokenValidated = context =>
            {
                // Extract phone number from token and add it as a claim
                var phoneClaim = context.Principal.FindFirst("https://claims.example.com/phone_number");
                if (phoneClaim != null)
                {
                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    claimsIdentity?.AddClaim(new Claim("phone_number", phoneClaim.Value));
                }
                // Extract full name from token and add it as a claim
                var fullNameClaim = context.Principal.FindFirst("https://claims.example.com/full_name");
                if (fullNameClaim != null)
                {
                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    claimsIdentity?.AddClaim(new Claim("full_name", fullNameClaim.Value));
                }
                return Task.CompletedTask;
            }
        };
    });


// Configure the HTTP request pipeline.
builder.Services.ConfigureSameSiteNoneCookies();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();