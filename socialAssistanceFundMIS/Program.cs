using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using socialAssistanceFundMIS.Components;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Seeders;
using socialAssistanceFundMIS.Services.Applicants;
using socialAssistanceFundMIS.Services.ApplicationServices;
using socialAssistanceFundMIS.Services.AssistanceProgramServices;
using socialAssistanceFundMIS.Services.DesignationServices;
using socialAssistanceFundMIS.Services.EmailServices;
using socialAssistanceFundMIS.Services.GeographicLocationServices;
using socialAssistanceFundMIS.Services.LookupServices;
using socialAssistanceFundMIS.Services.MaritalStatusServices;
using socialAssistanceFundMIS.Services.OfficerServices;
using socialAssistanceFundMIS.Services.OfficialRecordServices;
using socialAssistanceFundMIS.Services.PhoneNumberTypeServices;
using socialAssistanceFundMIS.Services.ReportServices;
using socialAssistanceFundMIS.Services.SexServices;
using socialAssistanceFundMIS.Services.StatusServices;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/social-assistance-fund-mis-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting Social Assistance Fund MIS application...");

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    // Add Blazor Bootstrap
    builder.Services.AddBlazorBootstrap();

    // Add HTTP Context Accessor for audit trails
    builder.Services.AddHttpContextAccessor();

    // Add AutoMapper
    builder.Services.AddAutoMapper(typeof(Program));

    // Add FluentValidation
    builder.Services.AddFluentValidationAutoValidation();

    // Configure Database
    ConfigureDatabase(builder);

    // Configure Services
    ConfigureServices(builder);

    // Configure CORS
    ConfigureCors(builder);

    // Configure Authentication & Authorization
    ConfigureAuthentication(builder);

    // Add Swagger/OpenAPI
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "Social Assistance Fund MIS API", 
                Version = "v1",
                Description = "API for Social Assistance Fund Management Information System"
            });
        });
    }

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    ConfigureMiddleware(app);

    // Initialize Database
    await InitializeDatabaseAsync(app);

    // Run the application
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

static void ConfigureDatabase(WebApplicationBuilder builder)
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

    var connectionString = builder.Configuration.GetConnectionString("SQLServerExpressEdition") 
        ?? throw new InvalidOperationException("Connection string 'SQLServerExpressEdition' not found.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
        
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    // Application Services
    builder.Services.AddScoped<IApplicantService, ApplicantService>();
    builder.Services.AddScoped<IApplicationService, ApplicationService>();
    builder.Services.AddScoped<IAssistanceProgramService, AssistanceProgramService>();
    builder.Services.AddScoped<IDesignationService, DesignationService>();
    builder.Services.AddScoped<IGeographicLocationService, GeographicLocationService>();
    builder.Services.AddScoped<IMaritalStatusService, MaritalStatusService>();
    builder.Services.AddScoped<IOfficialRecordService, OfficialRecordService>();
    builder.Services.AddScoped<IPhoneNumberTypeService, PhoneNumberTypeService>();
    builder.Services.AddScoped<ISexService, SexService>();
    builder.Services.AddScoped<IStatusService, StatusService>();
    builder.Services.AddScoped<ILookupService, LookupService>();
    builder.Services.AddScoped<IOfficerService, OfficerService>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IReportService, ReportService>();

    // Add Memory Cache
    builder.Services.AddMemoryCache();

    // Add Response Compression
    builder.Services.AddResponseCompression();

    // Add Health Checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();
}

static void ConfigureCors(WebApplicationBuilder builder)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin", policy =>
        {
            policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });
}

static void ConfigureAuthentication(WebApplicationBuilder builder)
{
    // Add JWT Authentication
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = builder.Configuration["Authentication:Authority"];
            options.Audience = builder.Configuration["Authentication:Audience"];
        });

    // Add Authorization
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdminRole", policy =>
            policy.RequireRole("Admin"));
        
        options.AddPolicy("RequireOfficerRole", policy =>
            policy.RequireRole("Admin", "Officer"));
    });
}

static void ConfigureMiddleware(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }
    else
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Assistance Fund MIS API V1");
            c.RoutePrefix = "api";
        });
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseResponseCompression();
    app.UseCors("AllowSpecificOrigin");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    // Map Health Checks
    app.MapHealthChecks("/health");

    // Map API endpoints
    app.MapControllers();
}

static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();

        logger.LogInformation("Seeding database...");
        DefaultSeeder.Run(context);

        logger.LogInformation("Database initialization completed successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
        throw;
    }
}
