using LMS.BAL.Interfaces;
using LMS.BAL.Services;
using LMS.Components.Entities;
using LMS.DAL.Interfaces;
using LMS.DAL.Repositories;
using LMS.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ----------------- Services Configuration -----------------

builder.Services.AddControllers();

// Database connection
builder.Services.AddDbContext<MyDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString")),
    ServiceLifetime.Transient);

// Swagger + JWT Security for Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Trucks API Services",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\nExample: \"Bearer {your token}\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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

// Memory cache
builder.Services.AddMemoryCache();

// JSON null-handling
builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull);

// Form options for large uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

// JWT Token Setup
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp", policy =>
    {
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
});

// Dependency Injection
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserMgmtService, UserMgmtService>();
builder.Services.AddScoped<IModuleMgmtService, ModuleMgmtService>();
builder.Services.AddScoped<IUserMgmtRepo, UserMgmtRepo>();
builder.Services.AddScoped<IModuleMgmtRepo, ModuleMgmtRepo>();
builder.Services.AddScoped<IMasterMgmtService, MasterMgmtService>();
builder.Services.AddScoped<IMasterMgmtRepo, MasterMgmtRepo>();
builder.Services.AddScoped<ICourseMgmtRepo, CourseMgmtRepo>();
builder.Services.AddScoped<ICourseMgmtService, CourseMgmtService>();
builder.Services.AddScoped<ILeadMgmtRepo, LeadMgmtRepo>();
builder.Services.AddScoped<ILeadMgmtService, LeadMgmtService>();
builder.Services.AddScoped<ICommonDDRepo, CommonDDRepo>();
builder.Services.AddScoped<IInstructorRepo, InstructorRepo>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IFeeCollectionRepository, FeeCollectionRepository>();
builder.Services.AddScoped<IFeeReceiptRepository, FeeReceiptRepository>();
builder.Services.AddScoped<IClassScheduleRepo, ClassScheduleRepo>();
builder.Services.AddScoped<ICourseBatchRepo, CourseBatchRepo>();

// ----------------- Application Pipeline -----------------

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trucks WEB API");
    c.DocumentTitle = "CRM API";
    c.DocExpansion(DocExpansion.List);
});

// Serve static files from wwwroot
app.UseStaticFiles();


// Middleware order
app.UseCors("corsapp");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Routing to controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
