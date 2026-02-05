using CdbRadar.Application.DependencyInjection;
using CdbRadar.Application.Mapping;
using CdbRadar.Repository.Abstractions;
using CdbRadar.Repository.DependencyInjection;
using CdbRadar.WebApi.DependencyInjection;
using Mapster;
using MapsterMapper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Minha API .NET 10",
        Version = "v1"
    });
});


builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddRepositories();
builder.Services.AddApplication();

builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();
MappingModule.RegisterAllMappings();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
