#!/bin/bash

# Creates two database migrations, one for local development and the other for a production environment

# Ensure a single command line argument was given
if [[ -z "$1" ]]; then
    printf "Incorrect usage, must supply a migration name. Usage: add-migration.sh <migration-name>"
    exit 1
fi

WORKING_DIR="$(cd `dirname $0` && cd ../ && pwd)"

export ASPNETCORE_ENVIRONMENT=Development
# The output-dir is relative to the --project
dotnet ef migrations add --project "$WORKING_DIR/src/Infrastructure/" --context ApplicationDbContextSqlite --startup-project "$WORKING_DIR/src/WebUI" --output-dir "Persistence/Migrations/Sqlite" "$1" --verbose 

export ASPNETCORE_ENVIRONMENT=Production
dotnet ef migrations add --project "$WORKING_DIR/src/Infrastructure/" --context ApplicationDbContextMySql --startup-project "$WORKING_DIR/src/WebUI" --output-dir "Persistence/Migrations/MySql" "$1" --verbose