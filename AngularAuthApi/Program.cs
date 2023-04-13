using AngularAuthApi.Authentication.DependencyInjection;
using AngularAuthApi.Data_Access;
using Microsoft.EntityFrameworkCore;


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
app.UseCors("MyPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
