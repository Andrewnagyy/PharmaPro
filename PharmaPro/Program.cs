using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmaPro.Core.Contract.Identity;
using PharmaPro.DS;
using PharmaPro.Repositories.AuthorizationRepo;
using PharmaPro.SendGrid.Model;
using PharmaPro.ServiceRegistration;
using PharmaPRO.ServiceRegistration;
using ServiceStack;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using static PharmaPro.SendGrid.Service.EmailSenderService;

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
});

builder.Services.AddTransient<SmtpClient>(provider =>
{
    var smtpClient = new SmtpClient();
    return smtpClient;
});

SwaggerServiceRegistration.AddApplicationServices(builder.Services);
ConfigureService.AddApplicationServices(builder.Services);
CorsServiceRegistration.AddApplicationServices(builder.Services);

builder.Services.AddScoped<IUserToken, UserToken>();
builder.Services.AddScoped<IOTPGenerator, OTPGenerator>();
builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("SendGrid"));
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

var app = builder.Build();
app.UseCors("ReactPolicy");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
