using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.Services.TransactionService;
using BankRepository.BankAppData;
using BankRepository.Data;
using Microsoft.EntityFrameworkCore;
using BankRepository.Infrastructure.Profiles;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BankAppDataContext>(options =>
    options.UseSqlServer(connectionString));


// Add services to the container.
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddAutoMapper(typeof(CustomerProfile).Assembly);

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Added general description for Swagger
builder.Services.AddSwaggerGen(sw =>
{
    sw.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1.0",
        Title = "Blue Ridge Bank Web API",
        Description = @"API for retrieving customer and account transaction info to mobile app.",
        Contact = new OpenApiContact
        {
            Name = "Frederick Wennborg",
            Email = "frederick.wennborg@gmail.com",
        },
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    sw.IncludeXmlComments(xmlPath);

});





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
