using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using Reporta_Colombo_Back.Services;
using Reporta_Colombo_Back.Services.Interfaces;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var cultureInfo = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IDenunciaService, DenunciaService>();

string credentialPath = Environment.GetEnvironmentVariable("FIREBASE_CONFIG_PATH")
                        ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reporta-colombo-firebase-adminsdk-fbsvc-a4c58aa6c7.json");

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

builder.Services.AddSingleton(s => FirestoreDb.Create("reporta-colombo"));

builder.Services.AddSingleton(s => StorageClient.Create());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsDevelopment",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500",
                                "https://caiovogl.github.io")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    options.AddPolicy("CorsConfig",
        policy =>
        {
            policy.WithOrigins("https://caiovogl.github.io")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("CorsDevelopment");
    app.MapOpenApi();
}

app.UseCors("CorsConfig");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
