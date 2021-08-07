#!/bin/bash

# Creates an idempotent database script that can be used to bring any database up to the current version
# Date: 8/7/2021

WORKING_DIR="$(cd `dirname $0` && cd ../ && pwd)"

dotnet ef migrations script --output "$WORKING_DIR/src/Infrastructure/Persistence/DbUpdateScript.sql" --idempotent --project "$WORKING_DIR/src/Infrastructure" --startup-project "$WORKING_DIR/src/WebUI" --context ApplicationDbContextMySql --verbose