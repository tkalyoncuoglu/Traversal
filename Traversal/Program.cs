using BusinessLayer.Container;
using DataAccessLayer.Concrete;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EntityLayer.Concrete;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Traversal.CQRS.Handlers.DestinationHandlers;
using Traversal.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(x =>
{
    x.ClearProviders();
    x.SetMinimumLevel(LogLevel.Debug);
    x.AddDebug();
});

builder.Services.AddControllersWithViews();
builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddDbContext<Context>();



builder.Services.AddIdentity<AppUser, AppRole>().
    AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider).
    AddEntityFrameworkStores<Context>().AddErrorDescriber<CustomIdentityValidator>();



builder.Services.AddAutoMapper(typeof(Program));
builder.Services.CustomValidator();
builder.Services.AddLocalization(opt =>
{
    opt.ResourcesPath = "Resources";
});

builder.Services.AddHttpClient();
builder.Services.ConfigureApplicationCookie(_ =>
{
    _.LoginPath = new PathString("/Login/SignIn");
	_.AccessDeniedPath = new PathString("/ErrorPage/AccessDenied/");
	_.LogoutPath = new PathString("/Login/Logout");
   

});

ILoggerFactory loggerFactory = new LoggerFactory();
var path = Directory.GetCurrentDirectory();
loggerFactory.AddFile($"{path}\\Logs\\Log1.txt");

builder.Services.AddMvcCore(config =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
}).AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotations();

builder.Services.AddScoped<Context>();
builder.Services.ContainerDependencies();
builder.Services.AddScoped<GetAllDestinationsQueryHandler>();
builder.Services.AddScoped<GetDestinationByIDQueryHandler>();
builder.Services.AddScoped<CreateDestinationCommandHandler>();
builder.Services.AddScoped<DeleteDestinationCommandHandler>();
builder.Services.AddScoped<UpdateDestinationCommandHandler>();

builder.Services.AddMediatR(typeof(Program));



var app = builder.Build();
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    if (!await roleManager.RoleExistsAsync("Member"))
    {
        await roleManager.CreateAsync(new AppRole("Member"));
    }

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new AppRole("Admin"));
    }
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404/", "?code{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();
var supportedCultures = new[] { "en", "fr", "es", "gr", "tr", "de" };
var localizationOptions= new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[4]).AddSupportedCultures(supportedCultures).AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.Run();
