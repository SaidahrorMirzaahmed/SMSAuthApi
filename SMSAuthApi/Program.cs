using HospitalApi.WebApi.Extensions;
using HospitalApi.WebApi.RouteHelper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using SMSAuthApi.ApiServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISMSService, SMSService>();
builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();
builder.Services.AddExceptionHandlers();

builder.Services.AddMemoryCache();
builder.Services.AddControllers(options =>
    options.Conventions.Add(new RouteTokenTransformerConvention(new RouteHelper())));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://sayidahror.uz")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.InjectEnvironmentItems();
app.UseExceptionHandler();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
