using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "EZ2Track Crawler API", Version = "v1"});
    c.AddSecurityDefinition("oauth2",
        new OpenApiSecurityScheme
        {
            Description = "OAuth2.0 Auth Code with PKCE",
            Name = "oauth2",
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(builder.Configuration["Swagger:AuthorizationUrl"]),
                    TokenUrl = new Uri(builder.Configuration["Swagger:TokenUrl"]),
                    Scopes = new Dictionary<string, string>
                    {
                        {builder.Configuration["CrawlerApi:Scope"], "read the api"}
                    }
                }
            }
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
            },
            new[] {builder.Configuration["CrawlerApi:Scope"]}
        }
    });
});

// Removes default logging providers and add serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(swaggerUiOptions =>
    {
        swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "EZ2Track Crawler API v1");
        swaggerUiOptions.OAuthClientId(builder.Configuration["Swagger:OpenIdClientId"]);
        swaggerUiOptions.OAuthUsePkce();
        swaggerUiOptions.OAuthScopeSeparator(" ");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
