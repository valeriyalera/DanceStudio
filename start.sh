#!/bin/bash
echo "Запускаю сервер..."
# Прибираємо &, щоб бачити помилки в терміналі
dotnet run --project DanceStudio.Infrastructure --launch-profile http
