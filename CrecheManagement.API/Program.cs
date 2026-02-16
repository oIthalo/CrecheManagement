using System.Text.Json.Serialization;
using CrecheManagement.API.Handlers;
using CrecheManagement.API.Providers;
using CrecheManagement.Domain.Behaviors;
using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.HttpClient.CNPJ;
using CrecheManagement.Domain.Interfaces.Encrypter;
using CrecheManagement.Domain.Interfaces.Providers;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Infrastructure.Context;
using CrecheManagement.Infrastructure.Mappings;
using CrecheManagement.Infrastructure.Repositories;
using CrecheManagement.Infrastructure.Security;
using CrecheManagement.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no formato: {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Services.AddMvc(x => x.Filters.Add(typeof(ExceptionsFilter)));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(RegisterCrecheCommand).Assembly));
builder.Services.AddAutoMapper(x => x.AddProfile(new Mappings()));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(ValidationBehavior<,>).Assembly, includeInternalTypes: true);
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<CustomCNPJRefitHandler>();
builder.Services
    .AddRefitClient<ICNPJRefitClient>()
    .ConfigureHttpClient(x =>
    {
        x.BaseAddress = new Uri(builder.Configuration["External:CNPJ_API:UrlBase"]!);
        x.Timeout = TimeSpan.FromSeconds(20);
    })
    .AddHttpMessageHandler<CustomCNPJRefitHandler>();

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton<ITextEncrypter, BCryptNet>();
builder.Services.AddSingleton<ITokensService, TokensService>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddSingleton<ILoggedUser, LoggedUserService>();
builder.Services.AddSingleton<IImageUploader, ImageUploader>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ICrechesRepository, CrechesRepository>();
builder.Services.AddScoped<IClassroomsRepository, ClassroomsRepository>();
builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
builder.Services.AddScoped<IAttendancesRepository, AttendancesRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();