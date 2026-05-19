using ExamPrepWeb.Data;
using ExamPrepWeb.Data.Repositories;
using ExamPrepWeb.Services;
using Microsoft.EntityFrameworkCore;
using ExamPrepWeb.Components;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Подключение БД (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Регистрация РЕПОЗИТОРИЯ
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

// 3. Регистрация сервисов (DI)
builder.Services.AddScoped<ICourseService, CourseService>();

// 4. Поддержка API контроллеров
builder.Services.AddControllers();

// 5. РЕГИСТРАЦИЯ FLUENTVALIDATION
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 6. Blazor Server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 7. Логирование
builder.Services.AddLogging();

var app = builder.Build();

// 8. Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// 9. Маршрутизация API
app.MapControllers();

// 10. Маршрутизация Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 11. Применение миграций / создание БД
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Используем Migrate для обновления БД
    if (app.Environment.IsDevelopment())
    {
        context.Database.EnsureCreated();
    }
    else
    {
        context.Database.Migrate();
    }
}

app.Run();