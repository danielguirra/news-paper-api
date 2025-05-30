#!/bin/bash
set -e

# Lista de variáveis obrigatórias
: "${ConnectionStrings__DefaultConnection:?Missing ConnectionStrings__DefaultConnection}"
: "${ASPNETCORE_ENVIRONMENT:?Missing ASPNETCORE_ENVIRONMENT}"

exec dotnet ApiBostil.dll
