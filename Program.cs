var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); //activando MVC

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.MapDefaultControllerRoute(); //establece a outra parte da activacion de MVC

app.Run();
