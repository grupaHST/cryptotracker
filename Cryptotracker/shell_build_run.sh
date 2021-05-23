#!/bin/sh

# Skrypt służący do automatycznego budowania aplikacji

PROJ_NAME=Cryptotracker

dotnet build "./$PROJ_NAME/$PROJ_NAME.csproj"
dotnet run --project "./$PROJ_NAME/$PROJ_NAME.csproj"
dotnet clean
