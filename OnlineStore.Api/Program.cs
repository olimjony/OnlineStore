using OnlineStore.Infrastructure.Seeders;
using OnlineStore.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using OnlineStore.Api.Swagger;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                policy.WithOrigins("http://localhost:5173")  // React app URL
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<FileUploadOperationFilter>();
    c.CustomSchemaIds(type => type.ToString());
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Web Api",
        Version = "v1.0",
        Description = "API Services.",
        Contact = new OpenApiContact
        {
            Name = "Quiz"
        },

    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                              "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                              "Example: \"Bearer 12345abc-def\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header
        },
        new List<string>()
    }});
});


var key = builder.Configuration["JWT:Key"];

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        RoleClaimType = ClaimTypes.Role
    };
});


builder.Services.AddInfrastructure(builder.Configuration);



var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetService<ISeeder>();
await seeder!.Seed();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("AllowSpecificOrigin");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Run();

