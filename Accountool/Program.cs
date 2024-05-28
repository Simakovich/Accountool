using Accountool.Data;
using Accountool.Models;
using Accountool.Models.DataAccess;
using Accountool.Models.Entities;
using Accountool.Models.Services;
using Accountool.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Настройка Identity
builder.Services.AddIdentity<AspNetUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();



builder.Services.AddScoped<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<IAIService, AIService>();
builder.Services.AddScoped<IControlHelperService, ControlHelperService>();
//builder.Services.AddTransient<IEmailService, EmailService>();
//builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddScoped<IRepository<Indication>, Repository<Indication>>();
builder.Services.AddScoped<IRepository<Schetchik>, Repository<Schetchik>>();
builder.Services.AddScoped<IRepository<Place>, Repository<Place>>();
builder.Services.AddScoped<IRepository<Town>, Repository<Town>>();
builder.Services.AddScoped<IRepository<MeasureType>, Repository<MeasureType>>();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
            new CultureInfo("en-US"),
        };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
var emailConfig = builder.Configuration.GetSection("EmailSettings");
builder.Services.AddTransient<IEmailService>(provider => new EmailService(
    emailConfig["SmtpServer"],
    int.Parse(emailConfig["SmtpPort"]),
    emailConfig["FromAddress"],
    emailConfig["FromAddressTitle"],
    emailConfig["Username"],
    emailConfig["Password"]));

builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = false;
    });
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//        .AddEntityFrameworkStores<ApplicationDbContext>()
//        .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();


//var roleManager = app.Services.GetRequiredService<RoleManager<IdentityRole>>();
//string[] roles = new string[] { "Admin", "User" };
//foreach (var role in roles)
//{
//    if (!await roleManager.RoleExistsAsync(role))
//    {
//        await roleManager.CreateAsync(new IdentityRole(role));
//    }
//}

app.Run();
