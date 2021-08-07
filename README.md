# Pre-Requisites

.NET 5 SDK
[dotnet-ef](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

# TODO

[x] Add all entities 
[x] Add entity configuration
[x] Run initial migration
[] Create all commands/queries (with tests along the way)
[] Create all controllers
[] Create business logic :)
[] Add Swagger auto-documenation

## Updating "dotnet-ef"

`dotnet tool update --global dotnet-ef`

## Using "dotnet-outdated"

This is a useful tool for ensuring our dependencies are updated (and even supports autoupdating!)

[Dotnet Outdated Github](https://github.com/dotnet-outdated/dotnet-outdated)

Run `dotnet-outdated`

# Questions

1. How to handle postage and such? Call it a non-cap item? Or attach to a revenue item?
2. How to handle refunds, things getting lost in the mail, etc.