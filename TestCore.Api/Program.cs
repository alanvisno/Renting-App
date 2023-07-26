using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TestCore.Api.Exceptions.Middleware;
using TestCore.Api.Services;
using TestCore.Data;
using TestCore.Data.Entities;
using TestCore.Security.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddSqlServer<IdentityDataContext>(builder.Configuration.GetConnectionString("Default"))
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityDataContext>();
//builder.Services
//    .AddSqlServer<CoreDataContext>(builder.Configuration.GetConnectionString("Default"));
builder.Services
    .AddDbContext<CoreDataContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.ConfigureIOC();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Renting App - V1",
            Version = "v1",
            Description = "Testing"
        }
     );

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseGlobalExceptionHandler();

var appInitializer = app.Services.GetRequiredService<IAppInitializer>();

try
{
    await appInitializer.Initialize(app);
}
catch (Exception ex)
{
    _ = ex;
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

appInitializer.SetTokenMapping(app, builder);
appInitializer.SetUserDataMapping(app);

app.Run();

public partial class Program { }