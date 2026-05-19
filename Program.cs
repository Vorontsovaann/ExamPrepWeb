using ExamPrepWeb.Data;
using ExamPrepWeb.Data.Repositories;
using ExamPrepWeb.Services;
using Microsoft.EntityFrameworkCore;
using ExamPrepWeb.Components;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. DI: Репозиторий
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

// 3. DI: Сервис
builder.Services.AddScoped<ICourseService, CourseService>();

// 4. API Controllers
builder.Services.AddControllers();

// 5. FluentValidation
builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.DisableDataAnnotationsValidation = true;
});
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 6. Blazor Server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// 7. Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// 8. Routing
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 9. Миграции + Seed (ВСЕГДА используем Migrate!)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Применяем миграции (вместо EnsureCreated)
    context.Database.Migrate();
    
    // Заполняем БД, если она пустая
    AppDbContext.SeedData(context);
}

app.Run();