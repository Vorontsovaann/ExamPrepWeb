# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY ["ExamPrepWeb.csproj", "."]
RUN dotnet restore "ExamPrepWeb.csproj"

# Копируем весь исходный код и собираем
COPY . .
RUN dotnet build "ExamPrepWeb.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "ExamPrepWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Копируем опубликованные файлы из stage publish
COPY --from=publish /app/publish .

# Healthcheck (требование документации)
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "ExamPrepWeb.dll"]