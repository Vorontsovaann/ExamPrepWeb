# 🎓 ExamPrepWeb — Система управления курсами подготовки к ЕГЭ

**Курсовой проект** по дисциплине *«Кроссплатформенная среда исполнения программного обеспечения»*

---

## Информация об авторе
| Параметр | Значение |
|----------|----------|
| **ФИО** | Дробжева Анна Дмитриевна |
| **Группа** | ББСО-01-24 |
| **Дисциплина** | Кроссплатформенная среда исполнения ПО |
| **Год** | 2024–2025 |

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

Открыть: https://localhost:5035 или http://localhost:5035

Запуск через Docker (Production)

# Собрать и запустить контейнер
docker-compose up -d

# Просмотр логов
docker-compose logs -f

# Остановка
docker-compose down

Открыть: http://localhost:8018

! Примечание: Данные SQLite сохраняются в Docker-volume sqlite_data, поэтому записи студентов и курсы не пропадают при перезапуске контейнера.

```

--- 

## 🗂 Структура проекта

ExamPrepWeb/
├── Components/              # Blazor-интерфейс
│   ├── Api/                 # REST-контроллеры
│   ├── Layout/              # Шаблоны (MainLayout, NavMenu)
│   └── Pages/               # Страницы (Index, Courses, Enrollment)
├── Data/                    # Слой доступа к данным
│   ├── Repositories/        # Паттерн Repository
│   ├── Migrations/          # Миграции EF Core
│   └── AppDbContext.cs      # Контекст БД + Fluent API конфигурация
├── Models/                  # EF-сущности и DTO
├── Services/                # Бизнес-логика (CourseService)
├── Validators/              # Правила FluentValidation
├── wwwroot/                 # Статика (CSS, JS, изображения)
├── .editorconfig            # Правила оформления кода
├── Dockerfile               # Multi-stage сборка
├── docker-compose.yml       # Оркестрация контейнера
└── README.md                # Документация

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