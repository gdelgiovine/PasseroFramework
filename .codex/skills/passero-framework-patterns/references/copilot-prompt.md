# Copilot Prompt

Use this when GitHub Copilot or another assistant cannot load the Codex skill directly.

```text
Follow the canonical Passero Framework patterns from this repository. Treat PasseroDemo.Application, PasseroDemo.Models, PasseroDemo.Repositories, PasseroDemo.ViewModels and PasseroDemo.Views as the source of truth.

Keep the architecture layered:
- Models inherit Passero.Framework.ModelBase and contain Dapper/EF metadata only.
- Repositories are thin rpEntity wrappers over Passero.Framework.Repository<T>.
- ViewModels inherit Passero.Framework.ViewModel<T>, contain domain query/update methods, use parameterized SQL, GetTableName()/Utilities.GetModelTableName<T>(), and avoid UI logic.
- Wisej Views own BindingSource, controls, DataNavigator, QBE/report wiring, form lifecycle and UI events.
- Initialize forms from ConfigurationManager.DBConnections["PasseroDemo"], ORMContextFactory.Create(...), vm.Init(_dbContext), DataBindingMode.BindingSource, vm.BindingSource = bsEntity, DataNavigator.AddViewModel(...), SetActiveViewModel(...), Init(true).

Prefer real PasseroDemo implementations over older generator templates. Use nameof(...) for model properties in bindings, QBE columns and mappings. Do not introduce a different architecture unless explicitly requested.
```
