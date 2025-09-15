using Malte.Clean.Data.Repositories;
using Malte.Clean.Data.Services;
using Malte.Clean.Data.Storage;
using Malte.Clean.Domain.Repositories;
using Malte.Clean.Domain.Services;
using Malte.Clean.Domain.UseCases;
using Malte.Clean.Domain.UseCases.Implementations;
using Malte.Clean.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure JSON options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// Register domain services
builder.Services.AddScoped<ImageValidationService>();

// Register data services
builder.Services.AddScoped<JsonStorageService>();
builder.Services.AddScoped<DataConsistencyService>();

// Register repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Register use cases
builder.Services.AddScoped<IUploadImagesUseCase, UploadImagesUseCase>();
builder.Services.AddScoped<IGetCustomerImagesUseCase, GetCustomerImagesUseCase>();
builder.Services.AddScoped<IDeleteImageUseCase, DeleteImageUseCase>();
builder.Services.AddScoped<IGetCustomerUseCase, GetCustomerUseCase>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Clean Market Customer Images API",
        Version = "v1",
        Description = "RESTful API for managing customer/lead image uploads with Clean Architecture principles",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Clean Market API",
            Email = "support@cleanmarket.com"
        }
    });

    // Include XML comments for better documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Configure for better documentation
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// Global exception handling middleware (should be first)
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
