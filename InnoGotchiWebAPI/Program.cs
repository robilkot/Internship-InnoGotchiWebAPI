using InnoGotchiWebAPI.Database;
using InnoGotchiWebAPI.Interfaces;
using InnoGotchiWebAPI.Logic;
using InnoGotchiWebAPI.Middleware;

namespace InnoGotchiWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IInnoGotchiDBService, InnoGotchiDBService>();
            builder.Services.AddScoped<InnoGotchiPetUpdateService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add db service
            builder.Services.AddDbContext<InnoGotchiContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseInnoGotchiExceptionHandler();

            app.MapControllers();

            {

                //app.MapGet("/api/pets", async (InnoGotchiContext db) => await db.Pets.ToListAsync());

                //app.MapGet("/api/pets/{id:Guid}", async (Guid id, InnoGotchiContext db) =>
                //{
                //    Pet? pet = await db.Pets.FirstOrDefaultAsync(u => u.Id == id);

                //    if (pet == null) return Results.NotFound(new { message = "Pet not found" });

                //    return Results.Json(pet);
                //});

                //app.MapDelete("/api/pets/{id:Guid}", async (Guid id, InnoGotchiContext db) =>
                //{
                //    Pet? pet = await db.Pets.FirstOrDefaultAsync(u => u.Id == id);

                //    if (pet == null) return Results.NotFound(new { message = "Pet not found" });

                //    Console.WriteLine($"Tried to delete {pet.Id}");
                //    //db.Pets.Remove(pet);
                //    //await db.SaveChangesAsync();
                //    return Results.Json(pet);
                //});

                //app.Use(async (context, next) =>
                //{
                //    await next.Invoke();
                //    Console.WriteLine($"Path: {context.Request.Path}");
                //});

                //app.Run(async (context) =>
                //{
                //    await context.Response.WriteAsync("Here speaks the terminating middleware");
                //});

            }

            app.Run();
        }
    }
}