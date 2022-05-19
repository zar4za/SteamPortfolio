using Microsoft.AspNetCore.Authentication.Cookies;
using SteamPortfolio.Models;
using SteamPortfolio.Services.InventoryRepository;
using SteamPortfolio.Services.Market;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/signin";
    options.LogoutPath = "/logout";
})
.AddSteam(options =>
{
    options.ApplicationKey = builder.Configuration["SteamApiKey"];
});
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoDbRepositorySettings>(builder.Configuration.GetSection("MongoDbRepository"));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IInventoryRepository, MongoDbRepository>();
builder.Services.AddSingleton<IPriceProvider, SkinportParser>();
builder.Services.AddSingleton<IMarketHashNameProvider, SkinportParser>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapFallbackToFile("index.html"); ;
app.Run();
