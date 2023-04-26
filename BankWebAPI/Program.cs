using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.Services.TransactionService;
using BankRepository.BankAppData;
using BankRepository.Data;
using Microsoft.EntityFrameworkCore;
using BankRepository.Infrastructure.Profiles;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

    // Needed for authorisation ========================================================
    sw.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Needed for authorisation ========================================================
    sw.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });

    // Needed for XML documentation to work
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    sw.IncludeXmlComments(xmlPath);
});





// JWT Token authorisation ======================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddMvc();
builder.Services.AddControllers();

// Adding CORS ========================================================
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adding CORS ========================================================
app.UseCors("AllowAll");

// Needed for JWT Token authorisation
// MUST COME BEFORE 'USEAUTHORIZATION'!!!!
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
