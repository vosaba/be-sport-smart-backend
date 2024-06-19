using Bss.Api.Data;
using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Bss.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "BeSportSmart API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<IBeSportSmartDbContext, BeSportSmartDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration["ConnectionStrings:DefaultConnection"],
        options => options.SetPostgresVersion(new Version(11, 8)));
});

builder.Services.AddScoped<IRepository>(x => x.GetService<BeSportSmartDbContext>() ?? throw new NullReferenceException(nameof(BeSportSmartDbContext)));
builder.Services.AddScoped<IPortfolioRepository>(x => x.GetService<BeSportSmartDbContext>() ?? throw new NullReferenceException(nameof(BeSportSmartDbContext)));
builder.Services.AddScoped<IInputRepository>(x => x.GetService<BeSportSmartDbContext>() ?? throw new NullReferenceException(nameof(BeSportSmartDbContext)));
builder.Services.AddScoped<IScoreProviderRepository>(x => x.GetService<BeSportSmartDbContext>() ?? throw new NullReferenceException(nameof(BeSportSmartDbContext)));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<BeSportSmartDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IFMPService, FMPService>();
builder.Services.AddScoped<IFormulaService, FormulaService>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddSingleton<IEvaluationEngine, EvaluationEngine>();

builder.Services.AddHttpClient<IFMPService, FMPService>();

builder.Services.AddMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (true || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
     .WithOrigins("https://localhost:44351")
     .AllowAnyMethod()
     .AllowAnyHeader()
     .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

