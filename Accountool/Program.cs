using Accountool.Data;
using Accountool.Models;
using Accountool.Models.DataAccess;
using Accountool.Models.Entities;
using Accountool.Models.Services;
using Accountool.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Globalization;
using Microsoft.Extensions.Options;
using Accountool.Models.Services.Identity;
using Microsoft.Extensions.Hosting;
using Accountool.Models.Services.EmailService;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ��������� Identity
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

        builder.Services.AddControllersWithViews();



        builder.Services.AddScoped<IMeasurementService, MeasurementService>();
        builder.Services.AddScoped<IAIService, AIService>();
        builder.Services.AddScoped<IControlHelperService, ControlHelperService>();
//builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        var emailConfig = builder.Configuration.GetSection("EmailSettings");
        builder.Services.AddTransient<IEmailService>(provider => new EmailService(
            emailConfig["SmtpServer"],
            int.Parse(emailConfig["SmtpPort"]),
            emailConfig["FromAddress"],
            emailConfig["FromAddressTitle"],
            emailConfig["Username"],
            emailConfig["Password"]));

        builder.Services.AddScoped<IRepository<Indication>, Repository<Indication>>();
        builder.Services.AddScoped<IRepository<Schetchik>, Repository<Schetchik>>();
        builder.Services.AddScoped<IRepository<Place>, Repository<Place>>();
        builder.Services.AddScoped<IRepository<Town>, Repository<Town>>();
        builder.Services.AddScoped<IRepository<MeasureType>, Repository<MeasureType>>();
//var emailConfig = builder.Configuration.GetSection("EmailSettings");
//builder.Services.AddTransient<IEmailService>(provider => new EmailService(
//    emailConfig["SmtpServer"],
//    int.Parse(emailConfig["SmtpPort"]),
//    emailConfig["FromAddress"],
//    emailConfig["FromAddressTitle"],
//    emailConfig["Username"],
//    emailConfig["Password"]));

        builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddViewOptions(options =>
            {
                options.HtmlHelperOptions.ClientValidationEnabled = false;
            });
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//        .AddEntityFrameworkStores<ApplicationDbContext>()
//        .AddDefaultTokenProviders();



        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

        builder.Services.AddMvc()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization();
        
        //builder.Services.Configure<RequestLocalizationOptions>(options =>
        //{
        //    var supportedCultures = new[]
        //    {
        //        new CultureInfo("en-US"),
        //        new CultureInfo("fr-FR"),
        //    };

        //    options.DefaultRequestCulture = new RequestCulture("fr-FR");
        //    options.SupportedCultures = supportedCultures;
        //    options.SupportedUICultures = supportedCultures;
        //});


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

        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("fr-FR"),
        };
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("fr-FR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        };

        localizationOptions.RequestCultureProviders.Clear();
        app.UseRequestLocalization(localizationOptions);
        //        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        
        using (var serviceScope = app.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            try
            {
                // Get a reference to the IdentityService
                var identityService = services.GetRequiredService<IIdentityService>();
                // Call the SetDefaultRoles method (you'll need to make it synchronous or use .Wait() or .Result)
                await identityService.SetDefaultRoles();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while setting the default roles.");
            }
        }

        app.Run();
    }
}
