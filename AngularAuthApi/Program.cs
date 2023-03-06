using AngularAuthApi.Authentication;
using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.Authentication.OptionsSetup;
using AngularAuthApi.Authentication.Repositories;
using AngularAuthApi.Authentication.Repositories.Abstract;
using AngularAuthApi.Authentication.Utilities;
using AngularAuthApi.Authentication.Utilities.Abstract;
using AngularAuthApi.Data_Access;
using AngularAuthApi.DcfCalculator.Abstract;
using AngularAuthApi.DcfCalculator.HttpClient;
using AngularAuthApi.DcfCalculator.Services;
using AngularAuthApi.Repository;
using AngularAuthApi.Repository.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("ConnStr1"));
});
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IJwtProvider,JwtProvider>();
builder.Services.AddScoped<ITokenGenerator,TokenGenerator>();
builder.Services.AddScoped<IRefreshTokenProvider,RefreshTokenProvider>();
builder.Services.AddScoped<IConfRefreshTokenValidationParameters,ConfRefreshTokenValidationParameters>();
builder.Services.AddScoped<IRefreshTokenValidate,RefreshTokenValidate>();
builder.Services.AddScoped<ITokenRepository,TokenRepository>();
builder.Services.AddScoped<IAuthenticator,Authenticator>();
builder.Services.AddScoped<IDecodeJwt, DecodeJwt>();
builder.Services.ConfigureOptions<JwtConfigOptionsSetup>();
builder.Services.ConfigureOptions<RefreshJwtConfigOptionsSetup>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
//Other services not related to auth
builder.Services.AddScoped<IUserInfoRepository, UserInfoRepository>();
builder.Services.AddScoped<IFinancialPrepHttpCalls,FinancialPrepHttpCalls>();
//HttpClient
builder.Services.AddHttpClient(name:"FinancialApi", client =>
{
    client.BaseAddress = new Uri("https://financialmodelingprep.com/api/v3/");
    

});
//Dcf Services
builder.Services.AddScoped<ICoreDcfService,CoreDcfService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
