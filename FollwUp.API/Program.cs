using FollwUp.API.Data;
using FollwUp.API.Mappings;
using FollwUp.API.Repositories.Interfaces;
using FollwUp.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FollwUp.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;

// TODO
// Authentication and Authorization

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<PhasesController>();
builder.Services.AddTransient<RolesController>();
builder.Services.AddTransient<InvitationsController>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Follwup API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Injecting FollwupDbContext dependency
builder.Services.AddDbContext<FollwupDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("FollwupConnectionString")));

// Injecting FollwUpAuthDbContext dependency
builder.Services.AddDbContext<FollwupAuthDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("FollwupAuthConnectionString")));

// Injecting ITaskRepository dependency with the implementation SqlTaskRepository
builder.Services.AddScoped<ITaskRepository, SqlTaskRepository>();

// Injecting IPhaseRepository dependency with the implementation SqlPhaseRepository
builder.Services.AddScoped<IPhaseRepository, SqlPhaseRepository>();

// Injecting IInvitationRepository dependency with the implementation SqlInvitationRepository
builder.Services.AddScoped<IInvitationRepository, SqlInvitationRepository>();

// Injecting IRoleRepository dependency with the implementation SqlRoleRepository
builder.Services.AddScoped<IRoleRepository, SqlRoleRepository>();

// Injecting IProfileRepository dependency with the implementation SqlProfileRepository
builder.Services.AddScoped<IProfileRepository, SqlProfileRepository>();

// Injecting ITokenRepository dependency with the implementation TokenRepository
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

// Injecting AutoMapper dependency
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Injecting Identity
builder.Services.AddIdentityCore<IdentityUser>()
    //.AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("FollwUp")
    .AddEntityFrameworkStores<FollwupAuthDbContext>()
    .AddDefaultTokenProviders();

// Setting identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Adding authentication to services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
