using QuestPDF.Infrastructure;
using RestaurantManagerMVC.Helpers;
using RestaurantManagerMVC.Services;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ExportService>();

builder.Services.AddHttpClient("RestaurantApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
});

var app = builder.Build();

ApiContext.Configure(app.Services.GetRequiredService<IHttpClientFactory>());

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
    pattern: "{controller=Restoran}/{action=Index}/{id?}");

app.Run();
