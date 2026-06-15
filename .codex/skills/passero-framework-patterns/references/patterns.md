# Passero Framework Canonical Patterns

These patterns come from `PasseroDemo.Application`, `PasseroDemo.Models`, `PasseroDemo.Repositories`, `PasseroDemo.ViewModels`, and `PasseroDemo.Views`. Prefer real demo files over older generator templates.

## Project Roles

- `PasseroDemo.Models`: database-shaped entities. Examples: `Author`, `Title`, `Titleauthor`, `Publisher`.
- `PasseroDemo.Repositories`: typed repository names and extension points. Example: `rpAuthor : Repository<Models.Author>`.
- `PasseroDemo.ViewModels`: domain methods around `ViewModel<T>`. Examples: `vmAuthor`, `vmTitle`, `vmPublisher`.
- `PasseroDemo.Views`: Wisej forms and UI orchestration. Examples: `frmAuthors`, `frmTitle`, `frmPasseroBaseView`.
- `PasseroDemo.Application`: login, configuration file loading, session configuration, MDI window and navigation.

## Naming

- Model: `Entity`, singular PascalCase, in `PasseroDemo.Models`.
- Repository: `rpEntity`, in `PasseroDemo.Repositories`.
- ViewModel: `vmEntity`, in `PasseroDemo.ViewModels`.
- Form: `frmEntity` or purpose-specific `frmEntityList`, in `PasseroDemo.Views`.
- BindingSource: usually `bsEntities`, with `DataSource = typeof(PasseroDemo.Models.Entity)`.
- Controls: model-aware names are common, such as `txt_au_id`, `cmb_pub_id`, `dgv_TitleAuthors`.

## Models

Canonical model shape:

```csharp
[BusinessAttributes.SystemName("ERP")]
[Table("authors")]
public class Author : Passero.Framework.ModelBase
{
    [ExplicitKey]
    [System.ComponentModel.DataAnnotations.Key]
    [ColumnMapping("au_id")]
    public string? au_id { get; set; }

    [Computed]
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string? au_fullname => $"{au_fname?.Trim()} {au_lname?.Trim()}";
}
```

Rules:

- Inherit `Passero.Framework.ModelBase`.
- Add `[BusinessAttributes.SystemName("ERP")]` for demo ERP entities.
- Add Dapper table attributes with the database table name; for EF compatibility, add `System.ComponentModel.DataAnnotations.Schema.Table` where needed.
- Mark natural keys with `[ExplicitKey]`; also add `[Key]` for EF/DataAnnotations compatibility.
- For composite keys, use multiple `[ExplicitKey]` and `[Key]` properties, with `[Column(Order = n)]` when present in the demo.
- Use nullable types where database columns can be null.
- Use `[ColumnMapping("db_column")]` only when property names differ from database columns.
- Use `[Computed]` and `[NotMapped]` for derived display properties.
- Keep data access out of models.

## Repositories

Canonical repository shape:

```csharp
namespace PasseroDemo.Repositories
{
    public class rpAuthor : Passero.Framework.Repository<Models.Author>
    {
        public rpAuthor() { }
    }
}
```

Rules:

- Keep repositories thin unless there is real repository-specific behavior.
- Prefer domain query methods in ViewModels rather than bloating repositories.
- Preserve the `rp` prefix convention.

## ViewModels

Canonical ViewModel shape:

```csharp
public class vmAuthor : Passero.Framework.ViewModel<Models.Author>
{
    public vmAuthor() { }

    public vmAuthor(IPasseroDbContext dbContext)
        : base(dbContext) { }

    public Models.Author GetAuthor(string au_id)
    {
        return this.Repository.DbConnection.Query<Models.Author>(
            $"SELECT * FROM {this.Repository.GetTableName()} WHERE au_id=@au_id",
            new { au_id }).Single();
    }
}
```

Rules:

- Inherit `Passero.Framework.ViewModel<T>`.
- Include a default constructor.
- Include an `IPasseroDbContext` constructor when the ViewModel will be initialized from `ORMContextFactory`.
- Use inherited methods (`GetItem`, `GetItems`, `GetAllItems`, `UpdateItem`, async variants) where possible.
- Return `ExecutionResult.Value` only after checking the calling context can tolerate failures; for UI workflows, configure `ErrorNotificationMessageBox` and `ErrorNotificationMode`.
- Use parameterized Dapper queries. Do not concatenate user input into SQL.
- Use `this.GetTableName()`, `this.Repository.GetTableName()`, or `Utilities.GetModelTableName<T>()` for table names.
- Keep UI control manipulation out of ViewModels.

## View Initialization

Canonical form setup from `frmAuthors` and `frmTitle`:

```csharp
public Passero.Framework.ConfigurationManager ConfigurationManager =
    new Passero.Framework.ConfigurationManager();

private IDbConnection DbConnection;
private Passero.Framework.Base.IPasseroDbContext _dbContext;
public ViewModels.vmAuthor vmAuthor = new ViewModels.vmAuthor();

public void Init()
{
    this.DbConnection = ConfigurationManager.DBConnections["PasseroDemo"];

    var ormType = Passero.Framework.Base.ORMType.Dapper;
    _dbContext = Passero.Framework.Base.ORMContextFactory.Create(
        ormType,
        DbConnection.ConnectionString,
        new[] { typeof(Models.Author) });

    vmAuthor.Init(_dbContext);
    vmAuthor.DataBindingMode = Passero.Framework.DataBindingMode.BindingSource;
    vmAuthor.BindingSource = this.bsAuthors;

    ControlsUtilities.SetPrimaryKeyControlsReadOnly<Models.Author>(bsAuthors);

    this.Accelerators = this.dataNavigator1.GetAccelerators();
    this.dataNavigator1.AddViewModel(this.vmAuthor, "Authors", null, null);
    this.dataNavigator1.SetActiveViewModel(this.vmAuthor);
    this.dataNavigator1.Init(true);
}
```

Rules:

- Call `Init()` from the form load event.
- Get connections from `ConfigurationManager.DBConnections["PasseroDemo"]`.
- Create a context with `ORMContextFactory.Create(ORMType, connectionString, entityTypes)`.
- Initialize every ViewModel with the shared `_dbContext` for the form.
- Set `DataBindingMode = BindingSource` and assign the matching BindingSource for designer-bound controls.
- For primary keys, set controls read-only after binding; switch to read-write in add-new request handlers if the key is user-entered.
- Add each ViewModel to `DataNavigator`; pass a grid for child/detail ViewModels.
- For multi-tab or master-detail forms, call `SetActiveViewModel(...)` when the active tab/context changes.
- Dispose form-owned resources in `FormClosed` when a form owns grids, binding sources or `_dbContext`.

## Wisej Designer Binding

Canonical designer binding pattern:

```csharp
this.bsAuthors.DataSource = typeof(PasseroDemo.Models.Author);
this.txt_au_id.DataBindings.Add(
    new Wisej.Web.Binding("Text", this.bsAuthors, "au_id", true,
        Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
```

Rules:

- Bind control properties to model property names through the form `BindingSource`.
- Use label text on Wisej controls for visible field labels.
- Use appropriate input modes and validation messages for typed fields, such as email.
- Keep designer-generated layout in `.Designer.cs`; place behavior in `.cs`.
- Do not edit generated designer sections unless the task explicitly requires UI layout changes.

## DataNavigator

Rules:

- Use `AddViewModel(viewModel, caption)` for single-model forms.
- Use `AddViewModel(viewModel, caption, dataGridView)` for grid-backed detail collections.
- Use `SetActiveViewModel(viewModel)` to choose the target of navigation, save, delete, find and add operations.
- Set `ManageNavigation`, `ManageChanges`, and `UseUpdateEx` intentionally for complex forms.
- Delegate simple actions to DataNavigator helpers:
  - `ViewModel_MoveFirstItem`, `ViewModel_MoveNextItem`, `ViewModel_MovePreviousItem`, `ViewModel_MoveLastItem`
  - `ViewModel_UdpateItem` (spelling as implemented)
  - `ViewModel_UndoChanges`
  - `DataGrid_AddNew`, `DataGrid_Delete`, `DataGrid_Save`
- Use request events such as `eDeleteRequest` or `eSaveRequest` to validate/cancel actions.

## QBE Forms

Canonical search/QBE pattern:

```csharp
QBEForm<Models.Author> qbe = new QBEForm<Models.Author>(_dbContext);
qbe.QBEColumns.Add(nameof(Models.Author.au_id), "Author Id", "", "", true, true, 20);
qbe.QBEColumns.Add(nameof(Models.Author.au_fname), "First Name", "", "", true, true, 20);
qbe.QBEResultMode = QBEResultMode.MultipleRowsSQLQuery;
qbe.Owner = this;
qbe.SetFocusControlAfterClose = this.txt_au_id;
qbe.SetTargetViewModel(this.vmAuthor, () => { this.dataNavigator1.Init(true); });
qbe.QBEModelPropertiesMapping.Add(nameof(Models.Author.au_id), nameof(Models.Author.au_id));
qbe.AutoLoadData = true;
qbe.ShowQBE();
```

Rules:

- Use `QBEForm<T>(_dbContext)` when the form already uses a context; use a connection only for older/simple forms.
- Define QBE columns with `nameof(Model.Property)`.
- Select `QBEResultMode` according to the target:
  - `BoundControls`: write selected values directly to controls/cells.
  - `MultipleRowsSQLQuery`: update target ViewModel query/results.
- Use `SetTargetViewModel(viewModel, callback)` when QBE updates a ViewModel.
- Use `QBEModelPropertiesMapping` when the QBE result must map properties back to the target model/query.
- Use `QBEBoundControls` for direct control or grid-cell fill scenarios.

## Reports

Canonical SSRS report manager pattern:

```csharp
var reportManager = new Passero.Framework.SSRSReports.ReportManager();
reportManager.QBEReports.Add("REPORT1", @"C:\Reports\REPORT1.RDL", "REPORT UNO");
reportManager.QBEReports["REPORT1"].AddDataSet<Models.Author>(
    "DataSet1", this.vmAuthor.Repository.DbConnection);
reportManager.DefaultReport = reportManager.QBEReports["REPORT1"];
reportManager.QBEColumns.AddForReport("REPORT1", nameof(Models.Author.au_id), "", "");
reportManager.SetFocusControlAfterClose = this.txt_au_id;
reportManager.CallBackAction = () => { this.dataNavigator1.Init(true); };
reportManager.ShowQBEReport();
```

Rules:

- Add reports by stable keys such as `REPORT1`.
- Add datasets with typed model and the active ViewModel repository connection.
- Add report QBE columns with `nameof`.
- Set focus and callback behavior after closing the report/QBE dialog.

## Application Bootstrap

Rules:

- `Program.Main` shows `LoginPage`.
- `LoginPage` stores session values with `ConfigurationManager.AddSessionConfigurationKeyValue(...)`, then shows `MainPage`.
- `MainPage.Init()` reads `PasseroDemo.ini`, stores `DBConnectionString` in session configuration, creates `SqlConnection`, and adds it to `ConfigurationManager.DBConnections` under `"PasseroDemo"`.
- Navigation opens one MDI child form per menu item with `ControlsUtilities.FormExist<T>(name)`, assigns `ConfigurationManager`, sets `MdiParent`, shows and activates it.
- `Startup.Main` configures Wisej and optional pre-Wisej command routing.

## Review Checklist

- Does the change keep model, ViewModel and view responsibilities separate?
- Are SQL values parameterized?
- Are table and property names derived with framework helpers or `nameof`?
- Is the ViewModel initialized before binding/navigation is used?
- Is `BindingSource` configured when designer bindings exist?
- Is `DataNavigator` active ViewModel correct for the selected tab/grid?
- Are QBE/report callbacks refreshing `DataNavigator` where needed?
- Are `_dbContext`, binding sources and grids released when the form owns them?
- Did the change avoid rewriting generated designer code unnecessarily?
