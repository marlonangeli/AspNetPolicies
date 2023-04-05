using System.Net;
using AspNetPolicies.Api.Extensions;
using AspNetPolicies.Data.Context;
using AspNetPolicies.Security.Exceptions;
using AspNetPolicies.Security.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOidcAuthentication(builder.Configuration);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddCors();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddVersioning();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<DocumentsContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddServices();
builder.Services.AddSwagger(builder.Configuration, true);

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception?.Error is UnauthorizedException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                message = exception.Error.Message
            }));
        }
    });
});


// Configure the HTTP request pipeline.
app.UseSwaggerUI(builder.Configuration, true);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
