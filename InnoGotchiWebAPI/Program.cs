using InnoGotchiWebAPI.Database;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Logic;
using InnoGotchiWebAPI.Middleware;
using InnoGotchiWebAPI.Models.MapperProfiles;
using InnoGotchiWebAPI.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .CreateLogger();

builder.Services.AddAutoMapper(typeof(ControllerProfile));

builder.Services.AddScoped<PetUpdateService>();
builder.Services.AddScoped<IDBService, DBService>();
builder.Services.AddScoped<ILoginService, LoginService>();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();
builder.Services.AddTransient<IConfiguration>(provider => configuration);

builder.Services.Configure<LoginOptions>(options => configuration.GetSection(LoginOptions.Position).Bind(options));
builder.Services.Configure<DBOptions>(options => configuration.GetSection(DBOptions.Position).Bind(options));
builder.Services.Configure<LogicOptions>(options => configuration.GetSection(LogicOptions.Position).Bind(options));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var loginOptions = new LoginOptions();
    configuration.GetSection(LoginOptions.Position).Bind(loginOptions);

    var tokenSecretKey = configuration["InnoGotchi:TokenSecretKey"]!;
    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecretKey));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidIssuer = loginOptions.TokenIssuer,

        IssuerSigningKey = symmetricSecurityKey,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

builder.Services.AddDbContext<InnoGotchiContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseInnoGotchiExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();