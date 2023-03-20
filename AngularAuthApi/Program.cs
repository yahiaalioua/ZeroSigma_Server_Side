using AngularAuthApi.Authentication;
using AngularAuthApi.Authentication.Abstractions;
using AngularAuthApi.Authentication.DependencyInjection;
using AngularAuthApi.Authentication.OptionsSetup;
using AngularAuthApi.Authentication.Repositories;
using AngularAuthApi.Authentication.Repositories.Abstract;
using AngularAuthApi.Authentication.Utilities;
using AngularAuthApi.Authentication.Utilities.Abstract;
using AngularAuthApi.Core.DcfCalculator.Abstract;
using AngularAuthApi.Core.DcfCalculator.HttpClient;
using AngularAuthApi.Core.DcfCalculator.Services;
using AngularAuthApi.Data_Access;
using AngularAuthApi.Data_Access.Options;
using AngularAuthApi.Repository;
using AngularAuthApi.Repository.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("ConnStr1"));
});

builder.Services
    .AddAuthenticationandAuthorization()
    .AddRepositories()
    .AddCoreDiscountedCashFlow();


//HttpClient
builder.Services.AddHttpClient(name:"FinancialApi", client =>
{
    client.BaseAddress = new Uri("https://financialmodelingprep.com/api/v3/");
    

});

//caching
builder.Services.AddResponseCaching();



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
