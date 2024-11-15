using LicenseeRecords.WebAPI.Models;
using LicenseeRecords.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var accountsFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Data", "Accounts.json");
var productsFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Data", "Products.json");

builder.Services.AddSingleton(new JsonFileService<Account>(accountsFilePath));
builder.Services.AddSingleton(new JsonFileService<Product>(productsFilePath));
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<ProductService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
