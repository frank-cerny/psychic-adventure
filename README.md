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

1. Create entity(s) (this includes the EF configuration if required)
2. Create context methods if required (CRUD)
3. Create GraphQL Input Type (map to entities) (similar to a response DTO)
4. Create GraphQL Output Type (map to entities) (similar to a request DTO)
5. Scaffold command/query
6. Scaffold validators
7. Add query/mutation in root query/mutation 
8. Write integration tests for commands/queries
9. Write full system tests for overall GraphQL queries/mutations

# Items of Note

1. Most entities will have to have validators updated to check specifically for null values that align with key constraints
2. Expense Items may have to have the parent id made optional, depending if I want to force parent items to use the expense command to get things created (or just rely on EF)