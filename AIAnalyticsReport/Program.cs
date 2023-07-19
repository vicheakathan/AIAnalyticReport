global using AnalyticsReport.Manager;
global using AnalyticsReport.Model;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(config.GetSection("ConnectionStrings").GetValue<string>("CentralDb")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddAuthentication();
builder.Services.AddScoped<ChatManager>();
builder.Services.AddScoped<DashboardManager>();
builder.Services.AddScoped<OAuthManager>();
builder.Services.AddScoped<ReportManager>();
builder.Services.AddScoped<GoogleManager>();
builder.Services.AddScoped<DinexMachineLearningManager>();
builder.Services.AddApplicationInsightsTelemetry();
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "wwwroot/assets/client_secret.json");
builder.Services.AddCors(opption =>
{
    opption.AddDefaultPolicy(config =>
    {
        config.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
