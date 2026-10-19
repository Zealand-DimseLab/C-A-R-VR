namespace cardataapi;
using System.Security.Claims;
using Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using CARDataLib;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // string? cardatadbsqlite = builder.Configuration.GetConnectionString("SqliteDb");
        // if(cardatadbsqlite == null)
        // {
        //     throw new Exception("Databasefejl");
        // }
        string? cardatadb = builder.Configuration.GetConnectionString("Default");
        if(cardatadb == null)
        {
            throw new Exception("Databasefejl");
        }
        builder.Services.AddSingleton<ByteHandlerService>();
        builder.Services.AddSingleton<CarDataChunkRepository>(new CarDataChunkRepository(cardatadb));
        // builder.Services.AddSingleton<CarDataSqliteChunkRepository>(new CarDataSqliteChunkRepository(cardatadbsqlite));
        builder.Services.AddSingleton<LoginService>(new LoginService(cardatadb));
        builder.Services.AddSingleton<GetTestPersonData>(new GetTestPersonData(cardatadb));
        // builder.Services.AddSingleton<FileHandlerService>();
        // builder.Services.AddSingleton<StringHandlerService>();
        // builder.Services.AddSingleton<CarDataMssqlRepository>(new CarDataMssqlRepository(cardatadb));
        // builder.Services.AddSingleton<CarDataChunkRepository>(new CarDataChunkRepository(cardatadb));

        // builder.WebHost.UseUrls("https://0.0.0.0:5000");

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o =>
                    {
                    o.Cookie.Name = "cardata.auth";
                    o.Cookie.HttpOnly = true;
                    o.Cookie.SameSite = SameSiteMode.Lax;
                    o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // use Always in production (https)
                    o.ExpireTimeSpan = TimeSpan.FromHours(8);
                    o.SlidingExpiration = true;

                    // return 401/403 instead of redirecting to a login page
                    o.Events.OnRedirectToLogin = ctx => { ctx.Response.StatusCode = 401; return Task.CompletedTask; };
                    o.Events.OnRedirectToAccessDenied = ctx => { ctx.Response.StatusCode = 403; return Task.CompletedTask; };
                    });
        builder.Services.AddAuthorization();


        builder.Services.AddCors(options =>
                {
                options.AddPolicy("AllowOnlyGetPost",
                        builder => builder.AllowAnyOrigin()
                        .WithMethods("GET", "POST")
                        .AllowAnyHeader());
                });
        var app = builder.Build();
        app.UseCors("AllowOnlyGetPost");

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();        

        // app.Run("https://0.0.0.0:5001");
    }
}
