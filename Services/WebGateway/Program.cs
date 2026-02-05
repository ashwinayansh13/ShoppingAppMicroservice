using WebGateway.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecks();

// Configure HTTP clients for microservices
builder.Services.AddHttpClient<IGalleryServiceClient, GalleryServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:GalleryService"] ?? "http://localhost:5001");
});

builder.Services.AddHttpClient<ICheckoutServiceClient, CheckoutServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:CheckoutService"] ?? "http://localhost:5002");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHealthChecks("/health");
app.MapGet("/health/ready", () => Results.Ok("Ready"));

app.Run();
