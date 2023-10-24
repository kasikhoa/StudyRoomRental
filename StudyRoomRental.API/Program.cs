using DentalLabManagement.API.Middlewares;
using Microsoft.EntityFrameworkCore;
using StudyRoomRental.API.Converter;
using StudyRoomRental.API.Extensions;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.DataTier.Models;
using System.Text.Json.Serialization;

namespace StudyRoomRental.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            builder.Services.AddDbContext<StudyRoomRentalContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerDatabase"));
            });

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CorsConstant.PolicyName,
                    policy => { policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod(); });
            });
            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                x.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
            });
            builder.Services.AddUnitOfWork();
            builder.Services.AddServices();
            builder.Services.AddJwtValidation();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddConfigSwagger();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //app.UseHttpsRedirection();
            app.UseCors(CorsConstant.PolicyName);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();


        }
    }
}