using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BeautifyMe.Areas.Identity.Data;
using BeautifyMe.Services;
using BeautifyMe.Services.Interfaces;
using BeautifyMe;
using BeautifyMe.BeautifyMeDbModels;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BeautifyMeDbContextConnection") ?? throw new InvalidOperationException("Connection string 'BeautifyMeDbContextConnection' not found.");


if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    connectionString = builder.Configuration.GetConnectionString("BeautifyMeDbContextConnectionProd") ?? throw new InvalidOperationException("Connection string 'BeautifyMeDbContextConnection' not found.");
}

builder.Services.AddDbContext<BeautifyMeDbContext>(options =>
    options.UseSqlServer(connectionString));




builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<BeautifyMeDbContext>();

builder.Services.AddDbContext<BeautifyMeContext>(options =>
{
    options.UseSqlServer(connectionString);
}, ServiceLifetime.Scoped);

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    builder.Services.BuildServiceProvider().GetService<BeautifyMeContext>().Database.Migrate();
    builder.Services.BuildServiceProvider().GetService<BeautifyMeDbContext>().Database.Migrate();
}
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IOTPGenerator, OTPGenerator>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IOrderService, OrderService>();
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
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Landing}/{action=Index}/{id?}");


app.Run();
