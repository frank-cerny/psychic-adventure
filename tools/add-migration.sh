#!/bin/bash

# Creates two database migrations, one for local development and the other for a production environment

# Ensure a single command line argument was given
if [[ -z "$1" ]]; then
    printf "Incorrect usage, must supply a migration name. Usage: add-migration.sh <migration-name>"
    exit 1
fi

WORKING_DIR="$(cd `dirname $0` && cd ../ && pwd)"

export DOTNET_ENVIRONMENT=Development
dotnet-ef migrations add --project "$WORKING_DIR/src/Infrastructure" --context ApplicationDbContextSqlite --startup-project "$WORKING_DIR/src/WebUI" --output-dir "$WORKING_DIR/Infrastructure/Persistence/Migrations/Sqlite" "$1"

export DOTNET_ENVIRONMENT=Production