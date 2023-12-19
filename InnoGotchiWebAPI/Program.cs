using InnoGotchiWebAPI.Database;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Logic;
using InnoGotchiWebAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using InnoGotchiWebAPI.Models.MapperProfiles;

namespace InnoGotchiWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(ControllerProfile));

            builder.Services.AddScoped<InnoGotchiPetUpdateService>();
            builder.Services.AddScoped<IInnoGotchiDBPetService, InnoGotchiDBPetService>();
            builder.Services.AddScoped<IInnoGotchiDBUserService, InnoGotchiDBUserService>();
            builder.Services.AddScoped<IInnoGotchiLoginService, InnoGotchiLoginService>();

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
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // todo: what are issuer and audience for?
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = AppConstants.TokenIssuer,

                    IssuerSigningKey = AppConstants.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };

                // todo: should study this bs through. rn this is just copypasted code from stackoverflow that helped to resolve jwt tokens issues
                //
                //options.Events = new JwtBearerEvents()
                //{
                //    OnChallenge = context =>
                //    {
                //        context.HandleResponse();
                //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //        context.Response.ContentType = "application/json";

                //        // Ensure we always have an error and error description.
                //        if (string.IsNullOrEmpty(context.Error))
                //            context.Error = "invalid_token";
                //        if (string.IsNullOrEmpty(context.ErrorDescription))
                //            context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                //        // Add some extra context for expired tokens.
                //        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                //        {
                //            var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                //            context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                //            context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                //        }

                //        return context.Response.WriteAsync(JsonSerializer.Serialize(new
                //        {
                //            error = context.Error,
                //            error_description = context.ErrorDescription
                //        }));
                //    }
                //};
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
        }
    }
}