using Microsoft.OpenApi.Models;
using SharedKernel.CurrentUser;
using SharedKernel.DbInitializer;
using User.Adapters.Inbound.Api.Controllers;
using User.Adapters.Inbound.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(UserController).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Casino User API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "'Bearer {your JWT token}'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddHttpContextAccessor();
//builder.Services.AddAuthenticationInjection(builder.Configuration);
//builder.Services.AddAuthorizationInjection(builder.Configuration);
//builder.Services.AddApplication();
//builder.Services.AddApiInjection(builder.Configuration);
//builder.Services.AddCommonBoundInjection(builder.Configuration);
//builder.Services.AddPostgresAdapter(builder.Configuration);
//builder.Services.AddNotificationInjection(builder.Configuration);
//builder.Services.AddRedisStorage(builder.Configuration);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializers = scope.ServiceProvider.GetServices<IDbInitializer>();
    foreach (var initializer in initializers)
    {
        await initializer.InitializeAsync(scope.ServiceProvider);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Casino User API v1");
        options.RoutePrefix = "swagger";
        options.ConfigObject.AdditionalItems["persistAuthorization"] = true;
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();