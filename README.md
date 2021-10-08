# Pre-Requisites

.NET 5 SDK
[dotnet-ef](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

## Updating "dotnet-ef"

`dotnet tool update --global dotnet-ef`

## Using "dotnet-outdated"

This is a useful tool for ensuring our dependencies are updated (and even supports autoupdating!)

[Dotnet Outdated Github](https://github.com/dotnet-outdated/dotnet-outdated)

Run `dotnet-outdated`

# Questions

1. How to handle postage and such? Call it a non-cap item? Or attach to a revenue item?
2. How to handle refunds, things getting lost in the mail, etc.

# Process: Creating a new Input/Output Set (with GraphQL)

1. Create entity(s) (mapped with Entity Framework)
2. Create GraphQL Input Type (map to entities) (similar to a response DTO)
3. Create GraphQL Output Type (map to entities) (similar to a request DTO)
4. Scaffold command/query
5. Scaffold validators
6. Add query/mutation in root query/mutation 
7. Write integration tests for commands/queries
8. Write full system tests for overall GraphQL queries/mutations