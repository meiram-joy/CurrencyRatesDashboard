using System.Text;
using CurrencyRates.Application.Behaviors;
using CurrencyRates.Application.Interfaces;
using CurrencyRates.Application.Services;
using CurrencyRates.Application.Validators;
using CurrencyRates.Domain.Currency.Services;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;
using CurrencyRates.Infrastructure;
using CurrencyRates.Infrastructure.External;
using CurrencyRates.Infrastructure.Serialization;
using CurrencyRates.Infrastructure.Services.Auth;
using CurrencyRates.Infrastructure.Services.Export;
using CurrencyRatesDashboard.BlazoreUIss.Components;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddScoped<ExportService>();
builder.Services.AddInfrastructure(builder.Configuration);
// Add services to the container.
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<FileUtil>();
builder.Services.AddScoped<ICurrencyRateApiClient, CurrencyRateApiClient>();
builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();
builder.Services.AddScoped<ThemeService>(); 

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Role.Admin.Name);
        });
        options.AddPolicy("UserOnly", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole(Role.User.Name);
        });
    }
);
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(typeof(CurrencyRateValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Добавим JWT поддержку в Swagger UI
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Введите JWT токен так: Bearer {your token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new EmailJsonConverter());
});

var app = builder.Build();

app.UseAuthentication(); // 👈 обязательно ДО UseAuthorization
app.UseAuthorization();

app.MapControllers(); 
app.UseSwagger();
app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
