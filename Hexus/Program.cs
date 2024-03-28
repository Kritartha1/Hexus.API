/*using Adhaar.API.Data;
using Adhaar.API.Helper;

using Adhaar.API.Models.Domain;
using Adhaar.API.Repositories.Implementaion;
using Adhaar.API.Repositories.Interface;
using Adhaar.API.Services;
using Adhaar.API.SMS;*/
/*using Hexus.Mappings;*/
using Hexus.Data;
using Hexus.Mappings;
using Hexus.Models.Domain;
using Hexus.Repositories.Implementation;
using Hexus.Repositories.Interface;
using Hexus.SIgnalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

[ExcludeFromCodeCoverage]
public class Program
{
    public static  void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/Hexus.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Information()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);
        builder.Services.AddSignalR();

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Hexus API",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Hexus Contact",
                    Email = "example@email.com",
                    Url = new Uri("https://example.com/Contact"),
                    /* smtp://live.smtp.mailtrap.io:587*/

                }

            });
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="OAuth2",
                Name=JwtBearerDefaults.AuthenticationScheme,
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
        });

        /*  string accountSid = Keys.SMSAccountIdentification;
          string authToken = Keys.SMSAccountPassword;

          TwilioClient.Init(accountSid, authToken);

          var mssg = MessageResource.CreateAsync(
              body: "This is the ship that made the Kessel Run in fourteen parsecs?",
              from: new Twilio.Types.PhoneNumber(Keys.SMSAccountFrom),
              to: new Twilio.Types.PhoneNumber("+917008196889")
          );*/

       /* builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
*/
        ///////////////////////////////////////////////////////////////////////////////////////////
        builder.Services.Configure<IdentityOptions>(opts => opts.SignIn.RequireConfirmedEmail = true);
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///
       /* builder.Services.AddTransient<IMailService, MailService>();
        builder.Services.AddTransient<IIdentityMessageService, SmsService>();*/

        builder.Services.AddDbContext<HexusApiDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("HexusConnectionString"))
        );

        builder.Services.AddDbContext<HexusApiAuthDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("HexusConnectionString"))
        );

        builder.Services.AddScoped<ITokenRepository, TokenRepository>();
        builder.Services.AddScoped<IDMRepository, DMRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();


        builder.Services.AddSingleton<PresenceTracker>();

        /* builder.Services.AddScoped<IImageRepository, ImageRepository>();*/


        // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

        builder.Services.AddIdentityCore<User>().AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>("Hexus")
            .AddEntityFrameworkStores<HexusApiAuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(5));


        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };

            }
            
            ) ;

       
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(options =>
        {
            options.AllowAnyHeader();
            options.AllowAnyMethod();
            options.AllowCredentials();
            options.WithOrigins("http://localhost:4200");
            /*options.AllowAnyOrigin();*/
            //options.AllowCredentials();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHub<PresenceHub>("hubs/presence");
        app.MapHub<MessageHub>("hubs/message");

        app.Run();


    }
}

