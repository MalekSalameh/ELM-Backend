using ELM;
using ELM.Application.Mappers;
using ELM.Infrastructure.Interface;
using ELM.Infrastructure.Repository;
using System.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value;
builder.Services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookMapper, BookMapper>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
