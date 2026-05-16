using Microsoft.EntityFrameworkCore;
using ExamPrepWeb.Data;
using ExamPrepWeb.Data.Repositories;
using ExamPrepWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Настройка базы данных
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Dependency Injection
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

// Blazor Server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Конвейер обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Маршрутизация Blazor
app.MapRazorComponents<ExamPrepWeb.Components.App>()
    .AddInteractiveServerRenderMode();

// Инициализация БД
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();