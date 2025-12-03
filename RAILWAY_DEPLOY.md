# Railway Deployment Guide

## Проблема
Railway не может автоматически определить тип приложения и показывает ошибку:
```
Script start.sh not found. Railpack could not determine how to build the app
```

## Решение

Были созданы конфигурационные файлы для деплоя на Railway:

### 1. nixpacks.toml (основной метод)
Файл настроен для .NET 10.0 с правильными командами сборки и запуска.

### 2. Dockerfile (альтернативный метод)
Multi-stage Dockerfile для оптимизированной сборки приложения.

## Инструкция по деплою на Railway

### Шаг 1: Создайте PostgreSQL базу данных
1. В Railway dashboard нажмите "New" → "Database" → "PostgreSQL"
2. Railway автоматически создаст переменную окружения `DATABASE_URL`

### Шаг 2: Создайте сервис для приложения
1. Нажмите "New" → "GitHub Repo" → выберите ваш репозиторий
2. Railway автоматически обнаружит `nixpacks.toml` или `Dockerfile`

### Шаг 3: Настройте переменные окружения
В настройках вашего сервиса добавьте:

#### Обязательные переменные:
```bash
# Railway автоматически предоставляет DATABASE_URL, но убедитесь что она есть
DATABASE_URL=<автоматически из PostgreSQL сервиса>

# Порт (если нужно переопределить)
PORT=5092

# Окружение
ASPNETCORE_ENVIRONMENT=Production
```

#### Опциональные переменные:
```bash
# Если нужно отключить HTTPS редирект
ASPNETCORE_URLS=http://+:5092

# Для настройки логирования
Logging__LogLevel__Default=Information
Logging__LogLevel__Microsoft.AspNetCore=Warning
```

### Шаг 4: Подключите базу данных к сервису
1. Откройте настройки вашего приложения
2. Во вкладке "Variables" нажмите "Reference" → выберите PostgreSQL сервис
3. Выберите переменную `DATABASE_URL`

### Шаг 5: Deploy
1. Railway автоматически начнет деплой после push в GitHub
2. Следите за логами в разделе "Deployments"

## Важные моменты

### Автоматические миграции
Приложение автоматически применяет миграции при старте (Program.cs:65-81):
```csharp
await context.Database.MigrateAsync();
```

### Учетные данные по умолчанию
При первом запуске создается администратор:
- Email: admin@bryx.com
- Password: admin123

**ВАЖНО:** Смените пароль после первого входа!

### SSL/TLS для PostgreSQL
Program.cs настроен для работы с Railway PostgreSQL через SSL:
```csharp
connectionString = $"...;SSL Mode=Require;Trust Server Certificate=true";
```

## Выбор метода деплоя

### Метод 1: Nixpacks (рекомендуется)
Railway автоматически использует `nixpacks.toml` если он есть в корне проекта.

**Преимущества:**
- Быстрее
- Меньше потребление ресурсов
- Автоматическое определение зависимостей

### Метод 2: Dockerfile
Если nixpacks не работает, можно использовать Dockerfile.

**Как переключиться:**
1. В Railway dashboard → Service Settings → Build
2. Выберите "Docker" вместо "Nixpacks"
3. Укажите путь к Dockerfile (обычно `/Dockerfile`)

## Проверка деплоя

### 1. Проверьте логи сборки
```
Railway Dashboard → Deployments → View Logs
```

Ищите:
```
✅ Миграции базы данных применены успешно
Создан пользователь администратора: admin@bryx.com / admin123
```

### 2. Проверьте здоровье приложения
```bash
curl https://your-app.railway.app/
```

### 3. Проверьте базу данных
В Railway dashboard откройте PostgreSQL сервис → Data → выполните:
```sql
SELECT * FROM "Categories";
SELECT * FROM "Products";
```

## Troubleshooting

### Ошибка: "Could not find a part of the path"
**Причина:** Неправильные пути в nixpacks.toml
**Решение:** Проверьте что используются одинарные кавычки для путей с пробелами

### Ошибка: "Unable to connect to database"
**Причина:** DATABASE_URL не настроена
**Решение:** Проверьте что PostgreSQL сервис подключен к приложению

### Ошибка: "Port already in use"
**Причина:** Конфликт портов
**Решение:** Railway автоматически назначает PORT через переменную окружения

### Приложение падает при старте
**Проверьте:**
1. Логи деплоя на наличие ошибок компиляции
2. Переменную DATABASE_URL корректна
3. Миграции применились успешно

## Полезные команды Railway CLI

```bash
# Установка CLI
npm install -g @railway/cli

# Логин
railway login

# Просмотр логов
railway logs

# Подключение к shell
railway shell

# Просмотр переменных
railway variables

# Локальный запуск с Railway переменными
railway run dotnet run
```

## Мониторинг

Railway предоставляет:
- CPU и Memory usage
- Request metrics
- Deployment history
- Логи в реальном времени

Доступно в разделе "Metrics" вашего сервиса.

## Масштабирование

Railway автоматически масштабирует приложение, но вы можете настроить:
- Memory limits
- CPU limits
- Restart policy

В разделе Settings → Resources

## Бэкапы базы данных

Railway не делает автоматические бэкапы на бесплатном плане.

**Рекомендации:**
1. Используйте Railway Pro для автоматических бэкапов
2. Настройте ручные бэкапы через cron job
3. Экспортируйте данные периодически через `pg_dump`

## Стоимость

Railway предоставляет $5 бесплатно каждый месяц.

**Примерная стоимость для этого приложения:**
- PostgreSQL: ~$2-3/месяц
- Web сервис: ~$2-3/месяц
- **Итого:** $4-6/месяц (умещается в бесплатный лимит)
