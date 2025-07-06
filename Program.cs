using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductsAPI.Data;
using ProductsAPI.Services;
using ProductsAPI.Validator;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

//Configure EF Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register services and interfaces
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ProductRequestValidator>();

builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "v1",
        Description = "Product Details",
        Contact = new OpenApiContact
        {
            Name = "Abhilash Madhyastha",
            Email = "abhilashnrmadhyastha98@gmail.com"
        }
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Logging.ClearProviders();

builder.Host.UseNLog();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
        c.RoutePrefix = "swagger"; 
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();