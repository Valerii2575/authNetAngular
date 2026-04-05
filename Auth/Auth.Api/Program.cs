using Auth.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAuthentication(x =>
                    {
                        x.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        //x.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                        //x.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                        //x.DefaultForbidScheme = IdentityConstants.ApplicationScheme;
                        //x.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
                    })
            .AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                    //    ValidateIssuer = false,
                    //ValidateAudience = false,
                    //ValidateLifetime = true,
                    //ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    //ValidAudience = builder.Configuration["Jwt:Audience"],
                };
             });

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

app.UseCors(options =>
{
     options.WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod();


});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGroup("/api")
    .MapIdentityApi<AppUser>();

app.MapPost("/api/signup", async (
    UserManager<AppUser> userManager,
    [FromBody] UserRegistrationModel model
    ) =>
{
    AppUser user = new AppUser
    {
        UserName = model.Email,
        Email = model.Email,
        FullName = model.FullName
    };
    var result = await userManager.CreateAsync(user, model.Password);
    if (result.Succeeded)
    {
        return Results.Ok(new { Message = "User registered successfully" });
    }
    else
    {
        return Results.BadRequest(string.Join(";\n", result.Errors.Select(x => x.Description).ToList()));
    }
});

app.Run();

public class UserRegistrationModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}