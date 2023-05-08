using BankRepository.BankAppData;
using BankRepository.Data;
using BankRepository.Infrastructure.Profiles;
using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.Services.IndexService;
using BankRepository.Services.TransactionService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BankManagerApp.DropDowns;
using BankRepository.Services.IdentityUserService;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>

    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CashierOnly", policy =>
        policy.RequireRole("Cashier"));
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});


builder.Services.AddRazorPages();

builder.Services.AddTransient<DataInitializer>();
builder.Services.AddTransient<IIndexStatisticsService, IndexStatisticsService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<IIdentityUserService, IdentityUserService>();
builder.Services.AddTransient<ICustomerDropDown, CustomerDropDown>();


builder.Services.AddDbContext<BankAppDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//This includes all MappingProfiles
builder.Services.AddAutoMapper(typeof(CustomerProfile).Assembly);

builder.Services.AddResponseCaching();


var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<DataInitializer>().SeedData();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseNotyf();

app.MapRazorPages();

app.UseResponseCaching();


app.Run();
