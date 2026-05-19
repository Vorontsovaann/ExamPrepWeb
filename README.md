# 🎓 ExamPrepWeb — Система управления курсами подготовки к ЕГЭ

**Курсовой проект** по дисциплине *«Кроссплатформенная среда исполнения программного обеспечения»*

---

## Информация об авторе
| Параметр | Значение |
|----------|----------|
| **ФИО** | Дробжева Анна Дмитриевна |
| **Группа** | ББСО-01-24 |
| **Дисциплина** | Кроссплатформенная среда исполнения ПО |
| **Год** | 2026 |

---

## Описание проекта
**ExamPrepWeb** — современное веб-приложение для просмотра каталога образовательных курсов и онлайн-записи студентов. Проект демонстрирует навыки разработки полнофункциональных .NET-приложений с применением архитектурных паттернов, контейнеризации и строгой валидации данных.

### Ключевые возможности
- **Каталог курсов** с сортировкой, фильтрацией и отображением метаданных (цена, дата, преподаватель)
- **Онлайн-запись** через пользовательскую форму с многоуровневой валидацией
- **Защита целостности данных**: проверка уникальности email, запрет дублирования записей на один курс
- **Персистентность**: хранение данных в SQLite с использованием EF Core CodeFirst + Fluent API
- **Готовность к продакшену**: Multi-stage Docker-сборка, `docker-compose` с volumes и healthcheck
- **Наблюдаемость**: структурированное логирование, обработка исключений, информативные сообщения об ошибках

---

## 🛠 Технологический стек
| Категория | Технологии |
|-----------|------------|
| **Платформа** | .NET 8, ASP.NET Core |
| **UI** | Blazor Server, Razor Components, JS Interop |
| **ORM / БД** | Entity Framework Core 8, SQLite |
| **Валидация** | FluentValidation, DataAnnotations |
| **Архитектура** | Repository Pattern, Dependency Injection (`IServiceCollection`), Service Layer |
| **Контейнеризация** | Docker, Docker Compose (volumes, healthchecks) |
| **Качество кода** | `.editorconfig`, XML-документация, StyleCop-совместимый стиль |

---

## 📦 Предварительные требования
Перед запуском убедитесь, что установлены:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) + Docker Compose
- [Git](https://git-scm.com/downloads)
- IDE: Visual Studio 2022+, JetBrains Rider или VS Code с расширением C# Dev Kit

---

## 🚀 Быстрый запуск

### 🖥 Локальный запуск (Development)
```bash
# 1. Клонировать репозиторий
git clone https://github.com/Vorontsovaann/ExamPrepWeb.git
cd ExamPrepWeb/ExamPrepWeb

# 2. Восстановить зависимости
dotnet restore

# 3. Применить миграции базы данных
dotnet ef database update

# 4. Запустить приложение
dotnet run
```

Открыть: https://localhost:5035 или http://localhost:5035

# Запуск через Docker (Production)

```bash
# 1. Собрать и запустить контейнер
docker-compose up -d --build

# 2. Проверить статус контейнера
docker-compose ps

# 3. Просмотр логов в реальном времени
docker-compose logs -f

# 4. Остановить приложение
docker-compose down

# 5. Остановить и удалить volume (база данных)
docker-compose down -v
```

Открыть: http://localhost:8018

! Примечание: Данные SQLite сохраняются в Docker-volume sqlite_data, поэтому записи студентов и курсы не пропадают при перезапуске контейнера.

--- 

## 🗂 Структура проекта
```
ExamPrepWeb/
│
├── 📁 Components/                 # Blazor-интерфейс
│   ├── 📁 Api/                    # REST API контроллеры
│   │   └── EnrollmentController.cs
│   ├── 📁 Layout/                 # Шаблоны страниц
│   │   ├── MainLayout.razor
│   │   ├── MainLayout.razor.css
│   │   ├── NavMenu.razor
│   │   └── NavMenu.razor.css
│   ├── 📁 Pages/                  # Страницы приложения
│   │   ├── Index.razor           # Главная
│   │   ├── Courses.razor         # Каталог курсов
│   │   └── Enrollment.razor      # Форма записи
│   ├── App.razor                 # Корневой компонент
│   ├── Routes.razor              # Конфигурация роутинга
│   └── _Imports.razor            # Глобальные using-директивы
│
├── 📁 Data/                       # Слой доступа к данным
│   ├── 📁 Repositories/          # Паттерн Repository
│   │   ├── ICourseRepository.cs
│   │   └── CourseRepository.cs
│   ├── 📁 Migrations/            # Миграции EF Core
│   │   ├── 20260516174158_InitialCreate.cs
│   │   ├── 20260516174158_InitialCreate.Designer.cs
│   │   └── AppDbContextModelSnapshot.cs
│   └── AppDbContext.cs           # Контекст БД + Fluent API
│
├── 📁 Models/                     # EF-сущности и DTO
│   ├── Student.cs
│   ├── Course.cs
│   ├── Enrollment.cs
│   └── EnrollmentRequest.cs
│   
│
├──  Services/                   # Бизнес-логика
│   ├── ICourseService.cs
│   └── CourseService.cs
│
├── 📁 Validators/                 # Правила FluentValidation
│   └── EnrollmentRequestValidator.cs
│
├──  wwwroot/                    # Статические файлы (CSS, JS, images)
│
├── 📄 .editorconfig              # Правила оформления кода
├── 📄 .gitignore                 # Исключения для Git
├── 📄 .dockerignore              # Исключения для Docker
├── 📄 appsettings.json           # Конфигурация приложения
├── 📄 appsettings.Development.json
├──  Dockerfile                 # Multi-stage сборка
├── 📄 docker-compose.yml         # Оркестрация контейнеров
├── 📄 ExamPrepWeb.csproj         # Файл проекта .NET
└──  README.md                  # Документация
```
---

## Схема архитектуры

```
┌─────────────────────────────────────────┐
│            Клиент (Браузер)             │
│  ┌─────────────────────────────────┐    │
│  │  Blazor Server Components       │    │
│  │  • Index.razor                  │    │
│  │  • Courses.razor                │    │
│  │  • Enrollment.razor             │    │
│  └─────────────────────────────────┘    │
└─────────────────────────────────────────┘
                 │ SignalR (WebSocket)
                 ▼
┌─────────────────────────────────────────┐
│         ASP.NET Core Server             │
│  ┌─────────────────────────────────┐    │
│  │  Controllers (API)              │    │
│  │  • EnrollmentController         │    │
│  └─────────────────────────────────┘    │
│  ┌─────────────────────────────────┐    │
│  │  Services (Business Logic)      │    │
│  │  • CourseService                │    │
│  └─────────────────────────────────┘    │
│  ┌─────────────────────────────────┐    │
│  │  Repositories (Data Access)     │    │
│  │  • CourseRepository             │    │
│  └─────────────────────────────────┘    │
│  ┌─────────────────────────────────┐    │
│  │  EF Core + SQLite               │    │
│  │  • AppDbContext                 │    │
│  │  • Migrations                   │    │
│  └─────────────────────────────────┘    │
└────────────────┬────────────────────────
                 │
                 ▼
┌─────────────────────────────────────────┐
│         База данных (SQLite)            │
│  ┌─────────────────────────────────┐    │
│  │  Tables:                        │    │
│  │  • Students                     │    │
│  │  • Courses                      │    │
│  │  • Enrollments                  │    │
│  └─────────────────────────────────┘    │
└─────────────────────────────────────────┘
```
---

## База данных и миграции
Проект использует подход EF Core CodeFirst. Структура БД описана в моделях (Student, Course, Enrollment) и настраивается через Fluent API в AppDbContext.cs

# Команды для работы с миграциями

```bash
# Создать новую миграцию
dotnet ef migrations add <ИмяМиграции> -o Data/Migrations

# Применить миграции к БД
dotnet ef database update

# Откатить последнюю миграцию
dotnet ef migrations remove
```

---

## API Endpoints

POST /api/enrollment — Запись студента на курс

# Request Body:
```json
{
  "fio": "Иванов Иван Иванович",
  "tel": "+7 (999) 000-00-00",
  "email": "student@example.com",
  "courseId": 1
}
```

# Response (Success):
```json 
{
  "success": true,
  "message": "Спасибо, Иван! Ваша заявка на курс принята."
}
```

# Response (Error):
```json
{
  "success": false,
  "message": "Вы уже записаны на этот курс"
}
```