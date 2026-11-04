# Passero Framework — Model runtime characterization

This test project uses the **production Passero.Framework assembly** via ProjectReference.
It does not change production code or require a live database.

Run on Windows with .NET 8 SDK and repository dependencies restored:

```powershell
dotnet test tests/Passero.Framework.Model.Tests/Passero.Framework.Model.Tests.csproj -c Release
```

The tests characterize:
- ModelBase event ordering and equality suppression
- automatic properties versus SetProperty notifications
- computed properties
- model cloning
- Dapper.Contrib explicit and composite key metadata
- computed-column filtering and table naming

Passing tests establish only these observable behaviors; they do not establish
runtime correctness of database access, ORM adapters, security or Wisej UI.

Known risks outside the immediate scope: hard-coded SqlKata assembly reference in
the core .csproj, divergent key-attribute interpretation, WriteAttribute filtering,
and runtime use of ValidateModelClass.

Do not modify production code as part of characterization. Document a failing
test first, then propose corrective work in a separate pull request.
