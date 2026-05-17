using ExamPrepWeb.Data;
using ExamPrepWeb.Services;
using Microsoft.EntityFrameworkCore;
using ExamPrepWeb.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Подключение БД (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Регистрируем Сервисы
builder.Services.AddScoped<ICourseService, CourseService>();

// 3. Добавляем поддержку API контроллеров
builder.Services.AddControllers();

// 4. Blazor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 5. Добавляем логирование
builder.Services.AddLogging();

var app = builder.Build();

// 6. Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// 7. Map Controllers (для API)
app.MapControllers();

// 8. Map Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 9. Применяем миграции и сидим данные при запуске
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    if (app.Environment.IsDevelopment())
    {
        context.Database.EnsureCreated(); // Только для разработки!
    }
    else
    {
        context.Database.Migrate(); // Для продакшена
    }
}

app.Run();