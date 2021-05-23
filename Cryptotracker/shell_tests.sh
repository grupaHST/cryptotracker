#!/bin/sh

# skrypt służący do automatycznego wykonanania testów jednostkowych

PROJ_NAME=CryptotrackerTests

dotnet test "./$PROJ_NAME/$PROJ_NAME.csproj"

echo "Wykonywanie testów zakończone"
read -p "Naciśnij dowolny klawisz aby kontynuować ..."

dotnet clean