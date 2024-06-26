using API.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Context;
using Persistence.Repositories.Interfaces;
using Persistence.Repositories;
using System.Reflection;
using System.Text;
using API.Services.Implements;
using API.Services.Interfaces;
using API.Utils;
using FluentValidation.AspNetCore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Domain.Mails;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var CorsPolicy = "CorsPolicy";
// Add services to the container.

builder.Services.AddDbContext<AuctionDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuctionDB"), sqlServerOptions =>
        sqlServerOptions.EnableRetryOnFailure());
});

//builder.Services.AddHangfire(config => config
//    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
//    .UseSimpleAssemblyNameTypeSerializer()
//    .UseRecommendedSerializerSettings()
//    .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
//builder.Services.AddHangfireServer();

builder.Services.Configure<PaymentSettings>(configuration.GetSection(nameof(PaymentSettings)));


builder.Services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IFirebaseService, FirebaseService>();
builder.Services.AddScoped<IUrlResourceService, UrlResourceService>();
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IUserAuctionService, UserAuctionService>();
builder.Services.AddScoped<IHangFireService, HangFireService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAuctionHistoryService, AuctionHistoryService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ITransferFormService, TransferFormService>();



builder.Services.AddControllers(options => options.Filters.Add<ValidateModelStateFilter>())
    .AddFluentValidation(c => c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{   
    c.SwaggerDoc("v1", new() { Title = "Auction", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Please enter a valid token",
        BearerFormat = "JWT",
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy,
        policy =>
        {
            policy.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "admin_sdk.json")),
    ProjectId = "swp-project-cef68"
});

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "admin_dk.json"));

builder.Services.AddLogging();
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(
        c => {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auction v1");
            c.RoutePrefix = string.Empty;
    });


app.UseCors(CorsPolicy);
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAutoWrapper();
app.MapControllers();
//app.UseHangfireDashboard("/hangfire");
//app.MapHangfireDashboard();

//RecurringJob.AddOrUpdate<IHangFireService>("update-auction-status", x => x.UpdateAuctionStatus(), Cron.Minutely);
//RecurringJob.AddOrUpdate<IHangFireService>("send-mail-auction", x => x.SendMailAuction(), Cron.Minutely);
app.Run();
