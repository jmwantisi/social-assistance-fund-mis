using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Components;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Seeders;
using socialAssistanceFundMIS.Services;
using SocialAssistanceFundMisMcv.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register and Inject DB Context

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerExpressEdition")));

// Services

builder.Services.AddScoped<ApplicantService>();
builder.Services.AddScoped<ApplicationService>();
builder.Services.AddScoped<AssistanceProgramService>();
builder.Services.AddScoped<DesignationService>();
builder.Services.AddScoped<GeographicLocationService>();
builder.Services.AddScoped<MaritalStatusService>();
builder.Services.AddScoped<OfficialRecordService>();
builder.Services.AddScoped<PhoneNumberTypeService>();
builder.Services.AddScoped<SexService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<LookupService>();
builder.Services.AddScoped<OfficerService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ReportService>();

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
