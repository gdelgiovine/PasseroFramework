---
name: passero-framework-patterns
description: Apply canonical Passero Framework application patterns derived from the PasseroDemo folders in this repository. Use when implementing, reviewing, generating, or refactoring PasseroDemo-style C# code for Models, Repositories, ViewModels, Wisej Views/Forms, DataNavigator, BindingSource, QBE forms, report managers, ORMContextFactory, configuration/session bootstrap, or GitHub Copilot/Codex prompts for this codebase.
---

# Passero Framework Patterns

## Workflow

Use the `PasseroDemo.*` projects as the canonical implementation source. Treat generated templates as secondary guidance for naming only when they conflict with real demo code.

Before editing or generating code:

1. Read the nearby implementation in the matching `PasseroDemo.*` project.
2. Read `references/patterns.md` for the relevant layer.
3. Preserve the existing layering:
   - `PasseroDemo.Models`: annotated POCO model classes.
   - `PasseroDemo.Repositories`: thin typed repositories.
   - `PasseroDemo.ViewModels`: typed orchestration over `Passero.Framework.ViewModel<T>`.
   - `PasseroDemo.Views`: Wisej forms, binding, navigation, QBE, reporting.
   - `PasseroDemo.Application`: login, configuration, session values, MDI navigation.
4. Keep changes local to the requested feature. Do not rewrite canonical demo patterns into a different architecture.

## Layer Rules

- Model classes inherit `Passero.Framework.ModelBase`, use Dapper/EF attributes for table, key, computed and column mapping metadata, and avoid data-access logic.
- Repository classes are usually empty wrappers over `Passero.Framework.Repository<T>` named `rpEntity`.
- ViewModel classes inherit `Passero.Framework.ViewModel<T>` and expose domain-specific query/update methods. Use `GetTableName()` or `Utilities.GetModelTableName<T>()` instead of hardcoding table names when possible.
- Views own Wisej controls, `BindingSource`, `DataNavigator`, QBE/report wiring, form lifecycle and UI-specific event handling.
- Initialize data access through `ConfigurationManager.DBConnections["PasseroDemo"]`, `ORMContextFactory.Create(...)`, then `vm.Init(_dbContext)`.
- Prefer `DataBindingMode.BindingSource` and assign the form `BindingSource` to the ViewModel when the form uses designer bindings.
- Use `DataNavigator.AddViewModel(...)`, `SetActiveViewModel(...)`, and `Init(true)` to connect navigation and CRUD workflows.

## References

- Read `references/patterns.md` for canonical examples and layer-specific checklists.
- Read `references/copilot-prompt.md` when the user wants to paste guidance into GitHub Copilot or another AI coding tool that cannot load this skill directly.
