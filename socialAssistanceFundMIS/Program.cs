using Microsoft.EntityFrameworkCore;
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

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register and Inject DB Context

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerExpressEdition")));

// Services

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

var app = builder.Build();

// Run database migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Apply pending migrations
    DefaultSeeder.Run(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
