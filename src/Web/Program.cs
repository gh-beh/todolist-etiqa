using Swashbuckle.AspNetCore.SwaggerUI;
using TodoListWebApi.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger(opts =>
{
    opts.RouteTemplate = "openapi/{documentName}.{extension:regex(^(json|ya?ml)$)}";
    
    app.UseSwaggerUI(opts =>
    {
        opts.DisplayOperationId();
        opts.RoutePrefix = "openapi";
        opts.ConfigObject.Urls = [new UrlDescriptor
        {
            Name = "v1",
            Url = "/openapi/v1.json"
        }];
    });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });


app.MapEndpoints();

app.Run();

public partial class Program
{
}
