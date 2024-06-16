using System.Security.Claims;
using API_Gateway.Auth0;
using API_Gateway.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Set base path and add environment variables
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Authentication
builder.Services.AddAuthentication(sharedOptions =>
    {
        sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
    });

// Add Ocelot and Kafka services
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddScoped<KafkaProducer>();

// Configure CORS
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseOcelot().Wait();
app.Run();