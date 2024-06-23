using FollwUp.API.Data;
using FollwUp.API.Mappings;
using FollwUp.API.Repositories.Interfaces;
using FollwUp.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FollwUp.API.Controllers;

// TODO
// Authentication and Authorization
// Validate

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
});

// Injecting FollwupDbContext dependency
builder.Services.AddDbContext<FollwupDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("FollwupConnectionString")));

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

// Injecting AutoMapper dependency
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
