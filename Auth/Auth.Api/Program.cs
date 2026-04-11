using Auth.Api.Controllers;
using Auth.Api.Extensions;
using Auth.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInjectDbContext(builder.Configuration);
builder.Services.AddAppConfig(builder.Configuration);
builder.Services.AddIdentityHandlersAndStores();
builder.Services.AddConfigureIdentityOptions();
builder.Services.AddIdentityAuthentication(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/openapi/v1.json", "api");
        });
}
 
app.UseHttpsRedirection();

app.UseConfigureCORS();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGroup("/api")
    .MapIdentityApi<AppUser>();

app.MapGroup("/api")
    .MapIdentityUserEndpoints(builder.Configuration);

app.Run();

