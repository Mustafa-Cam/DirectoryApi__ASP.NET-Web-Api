using Contracts;
using DirectoryApi.Data;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Text;



//var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
//try
//{
var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(),"/nlog.config"));
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<DataDpContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryverysceret.....")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddCors(options => options.AddPolicy(name: "DirectoryOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
        }));
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

    //builder.Logging.ClearProviders();
    //builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
      
    app.UseCors("DirectoryOrigins");

    app.UseHttpsRedirection();
    app.UseAuthentication();;

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
//}
//catch(Exception ex)
//{
//    logger.Error(ex);
//    throw (ex);
//}
//finally
//{
//    NLog.LogManager.Shutdown();
//}