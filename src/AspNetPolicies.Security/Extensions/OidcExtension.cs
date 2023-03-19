using System.Security.Claims;
using AspNetPolicies.Security.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AspNetPolicies.Security.Extensions;

public static class OidcExtension
{
    public static void AddOidcAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var oidcSettings = configuration.GetSection(OidcSettings.SectionName).Get<OidcSettings>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie("Cookies")
        .AddOpenIdConnect(options =>
        {
            options.Authority = oidcSettings.Authority;
            options.ClientId = oidcSettings.ClientId;
            options.ClientSecret = oidcSettings.ClientSecret;
            options.ResponseType = "code id_token";
            options.Scope.Add(oidcSettings.Scope);
            options.Scope.Add("offline_access");
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.BackchannelHttpHandler = new HttpClientHandler
            {
                UseProxy = false
            };
        })
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = oidcSettings.Authority;
            options.Audience = oidcSettings.ClientId;
            options.SaveToken = true;
            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
        });
    }
}