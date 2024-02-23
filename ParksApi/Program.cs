using ParksApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ParksApiContext>(
                    dbContextOptions => dbContextOptions
                    .UseMySql(
                        builder.Configuration["ConnectionStrings:DefaultConnection"], 
                        ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                        )
                    )
                );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
  // Specify the default API version
    options.DefaultApiVersion = new ApiVersion(1, 0);
  // Specify if the API version should be included in the request
    options.AssumeDefaultVersionWhenUnspecified = true;
  // Specify the format of the API version in the response header
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(), new HeaderApiVersionReader("x-api-version"), new MediaTypeApiVersionReader("x-api-version"));
});


builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else 
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();