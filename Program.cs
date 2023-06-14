using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using APICatalogo.Data;
using APICatalogo.Mapper;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>(); 
builder.Services.AddDbContext<Context>(
  options =>
  options.UseMySql(
      builder.Configuration.GetConnectionString("DefaultConnection"),
      ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
  )
);

//Configurações para usar Autenticação com JWT
var JWTKey = Encoding.ASCII.GetBytes(builder.Configuration["JWTKey"]);
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(JWTKey),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

  
builder.Services.AddControllers().AddJsonOptions(options =>
  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
  
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    {   Title = "APICatalogo", 
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Matheus Siqueira",
            Email = "matheussiqueira.work@gmail.com"
        } 
    });

    var xmlFile = "APICatalogo.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT", 
        In = ParameterLocation.Header,
        Description = "Header de autorização JWT usando o esquema Bearer.\r\n\r\nInforme o 'Bearer'[espaço] e o seu token.\r\n\r\nExemplo: \'Bearer asdlfajsdfasdf\'" 
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
}).CreateMapper());

builder.Services.AddCors();

builder.Services.AddApiVersioning(options => 
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true; 
});

builder.Services.AddVersionedApiExplorer( c =>
{
    c.GroupNameFormat = "'v'VVV";
    c.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

app.UseCors(policy => 
policy.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "APICatalogo");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
