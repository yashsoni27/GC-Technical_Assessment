using LicenseeRecords.WebAPI.Services;
using LicenseeRecords.WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

string jsonFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Data", "Accounts.json");
builder.Services.AddScoped<JsonFileService<Account>>(provider =>
    new JsonFileService<Account>(jsonFilePath));
string productsJsonPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Data", "Products.json");
builder.Services.AddScoped<JsonFileService<Product>>(provider =>
    new JsonFileService<Product>(productsJsonPath));

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
