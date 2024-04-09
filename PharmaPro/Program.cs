using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmaPro.Core.Contract.Identity;
using PharmaPro.DS;
using PharmaPro.Repositories.AuthorizationRepo;
using PharmaPro.ServiceRegistration;
using PharmaPRO.ServiceRegistration;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


String depolyConnectionString = builder.Configuration.GetConnectionString("SomeeHosting") ?? String.Empty;
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(depolyConnectionString));

builder.Services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("Jwt:Key").Value!))

    };
}
);



SwaggerServiceRegistration.AddApplicationServices(builder.Services);
ConfigureService.AddApplicationServices(builder.Services);
CorsServiceRegistration.AddApplicationServices(builder.Services);
builder.Services.AddScoped<IUserToken, UserToken>();


var app = builder.Build();
app.UseCors("ReactPolicy");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
