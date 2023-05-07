using Exam;
using Exam.BLL;
using Exam.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JwtDemoApi",
        Version = "v1"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Iveskite validu JWT tokena",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                        new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
                                        Type = ReferenceType.SecurityScheme,
                                                Id = "Bearer"
                                }
                        },
                        new string[] {}
                }
        });
});

builder.Services.AddDbContext<UserRegistrationDbContext>(options =>
        options.UseSqlServer($"Server=localhost\\MSSQLSERVER05;Database=Accounts;Trusted_Connection=True;"));

builder.Services.AddScoped<IJwtRepository, JwtRepository>();
builder.Services.AddScoped<IUserInformationDbRepository, UserInformationDbRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin",
            builder => {
                builder
                            .WithOrigins("http://127.0.0.1:5500")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
            });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();