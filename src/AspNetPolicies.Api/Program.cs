using AspNetPolicies.Api.Extensions;
using AspNetPolicies.Data.Context;
using AspNetPolicies.Security.Extensions;
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

builder.Services.AddDbContext<DocumentsContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddServices();
builder.Services.AddSwagger(builder.Configuration, true);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerUI(builder.Configuration, true);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
