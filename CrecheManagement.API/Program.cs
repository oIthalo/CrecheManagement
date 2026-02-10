using CrecheManagement.API.Handlers;
using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.HttpClient.CNPJ;
using CrecheManagement.Domain.Interfaces.Encrypter;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Infrastructure.Context;
using CrecheManagement.Infrastructure.Repositories;
using CrecheManagement.Infrastructure.Security;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Services.AddMvc(x => x.Filters.Add(typeof(ExceptionsFilter)));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(RegisterCrecheCommand).Assembly));

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

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ICrechesRepository, CrechesRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();