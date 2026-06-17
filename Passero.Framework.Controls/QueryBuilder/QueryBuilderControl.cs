using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Wisej.Web;

namespace Passero.Framework.Controls;

public partial class QueryBuilderControl : UserControl
{
    private readonly JsonSerializerSettings _jsonSettings;
    private GroupEditor? _rootEditor;
    private Control? _draggedEditor;
    internal const string DragDropToken = "QueryBuilderEditorMove";

    public QueryBuilderControl()
    {
        InitializeComponent();

        Columns = new BindingList<QueryBuilderColumn>();
        MaxGroupDepth = 5;
        AllowEmptyGroups = false;

        _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        
        CreateEmptyRoot();
    }

    private object _viewModelInstance;
    public Type ViewModelType { get; private set; }

    public string DefaultSQLQuery { get; set; } = string.Empty;
    public string SQLQuery { get; set; } = string.Empty;
    public Dapper.DynamicParameters DefaultSQLQueryParameters { get; set; } = new Dapper.DynamicParameters();
    public Dapper.DynamicParameters Parameters { get; set; } = new Dapper.DynamicParameters();
    /// <summary>
    /// Imposta il ViewModel in modo type-safe senza rendere il controllo generico.
    /// Mantiene internamente l'istanza e registra il tipo in ViewModelType.
    /// </summary>
    public void SetViewModel<TViewModel>(TViewModel viewModel, IDbConnection DbConnection)
    {
        _viewModelInstance = viewModel;
        ViewModelType = typeof(TViewModel);
        this.QBEColumns.Parent = this;

        this.DefaultSQLQuery =
            Passero.Framework.ReflectionHelper.GetPropertyValue(viewModel, "DefaultSQLQuery")?.ToString() ?? string.Empty;

        this.DefaultSQLQueryParameters =
            Passero.Framework.ReflectionHelper.GetPropertyValue(viewModel, "DefaultSQLQueryParameters") as Dapper.DynamicParameters
            ?? new Dapper.DynamicParameters();
        // eventuale inizializzazione basata sul ViewModel
    }

    /// <summary>
    /// Restituisce l'istanza del ViewModel castata al tipo richiesto.
    /// </summary>
    public TViewModel GetViewModel<TViewModel>()
    {
        return (TViewModel)_viewModelInstance;

    }

    /// <summary>
    /// Accesso non tipizzato all'istanza del ViewModel (weakly-typed, utile per reflection).
    /// </summary>
    public object ViewModel => _viewModelInstance;

       
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BindingList<QueryBuilderColumn> Columns { get; }

    [DefaultValue(5)]
    public int MaxGroupDepth { get; set; }

    [DefaultValue(false)]
    public bool AllowEmptyGroups { get; set; }

    public event EventHandler<QueryBuilderChangedEventArgs>? RulesChanged;
    public event EventHandler<QueryBuilderRequestEventArgs>? SaveQueryRequest;
    public event EventHandler<QueryBuilderRequestEventArgs>? LoadQueryRequest;

    public void SetColumns(IEnumerable<QueryBuilderColumn> columns)
    {
        Columns.Clear();

        foreach (var column in columns)
        {
            if (column.Operators.Count == 0)
            {
                column.Operators.AddRange(GetDefaultOperators(column.Type));
            }

            Columns.Add(column);
        }

        RebindAllRuleEditors();
        RaiseRulesChanged();
    }

    public QueryBuilderRuleSet GetRules()
    {
        return _rootEditor?.ToRuleSet() ?? new QueryBuilderRuleSet();
    }

    public string GetRulesJson()
    {
        return JsonConvert.SerializeObject(GetRules(), _jsonSettings);
    }

    public void LoadRulesJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            CreateEmptyRoot();
            return;
        }

        var rules = JsonConvert.DeserializeObject<QueryBuilderRuleSet>(json, _jsonSettings)
                    ?? new QueryBuilderRuleSet();

        LoadRules(rules);
    }

    public void LoadRules(QueryBuilderRuleSet rules)
    {
        flowLayoutPanelRoot.Controls.Clear();

        _rootEditor = new GroupEditor(this, 0, true);
        _rootEditor.LoadFromRuleSet(rules);
        flowLayoutPanelRoot.Controls.Add(_rootEditor);

        ResizeEditors();
        RaiseRulesChanged();
    }

    public void ClearRules()
    {
        CreateEmptyRoot();
    }

    public QueryBuilderSqlResult GetParameterizedSqlWhere()
    {
        var result = new QueryBuilderSqlResult();
        var rules = GetRules();

        var mergedParameters = new Dapper.DynamicParameters();
        var usedParameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        CopyDynamicParameters(this.DefaultSQLQueryParameters, mergedParameters, usedParameterNames);

        var filterParameters = new Dictionary<string, object?>();
        var index = 0;
        var filterClause = BuildSqlForGroup(
            rules.Condition,
            rules.Rules,
            filterParameters,
            ref index,
            usedParameterNames);

        foreach (var parameter in filterParameters)
        {
            mergedParameters.Add(parameter.Key, parameter.Value);
        }

        this.SQLQuery = ComposeSqlQuery(this.DefaultSQLQuery, filterClause);
        this.Parameters = mergedParameters;

        result.WhereClause = filterClause;
        result.Parameters = new Dictionary<string, object?>();
        CopyDynamicParametersToDictionary(this.DefaultSQLQueryParameters, result.Parameters);

        foreach (var parameter in filterParameters)
        {
            result.Parameters[parameter.Key] = parameter.Value;
        }

        return result;
    }

    internal QueryBuilderColumn? FindColumn(string? field)
    {
        if (string.IsNullOrWhiteSpace(field))
        {
            return null;
        }

        return Columns.FirstOrDefault(c => string.Equals(c.Field, field, StringComparison.OrdinalIgnoreCase));
    }

    internal IReadOnlyList<QueryBuilderOperator> GetOperatorsForColumn(QueryBuilderColumn? column)
    {
        if (column == null)
        {
            return Array.Empty<QueryBuilderOperator>();
        }

        return column.Operators.Count > 0 ? column.Operators : GetDefaultOperators(column.Type);
    }

    internal void NotifyChanged()
    {
        ResizeEditors();
        RaiseRulesChanged();
    }

    internal void StartDrag(Control editor)
    {
        _draggedEditor = editor;
    }

    internal void EndDrag()
    {
        _draggedEditor = null;
    }

    internal DragDropEffects GetDropEffect(GroupEditor targetGroup)
    {
        return CanDropOn(targetGroup) ? DragDropEffects.Move : DragDropEffects.None;
    }

    internal DragDropEffects GetDropEffect(Control targetControl)
    {
        return CanDropRelativeTo(targetControl) ? DragDropEffects.Move : DragDropEffects.None;
    }

    internal bool MoveDraggedEditor(GroupEditor targetGroup, Point location)
    {
        if (_draggedEditor is null || !CanDropOn(targetGroup))
        {
            return false;
        }

        var insertIndex = targetGroup.GetInsertIndex(location, _draggedEditor);
        return MoveDraggedEditor(targetGroup.ChildrenPanel, insertIndex);
    }

    internal bool MoveDraggedEditorRelativeTo(Control targetControl, bool insertAfter)
    {
        if (_draggedEditor is null || !CanDropRelativeTo(targetControl))
        {
            return false;
        }

        if (targetControl.Parent is not FlowLayoutPanel targetPanel)
        {
            return false;
        }

        var insertIndex = 0;

        foreach (Control control in targetPanel.Controls)
        {
            if (ReferenceEquals(control, _draggedEditor))
            {
                continue;
            }

            if (ReferenceEquals(control, targetControl))
            {
                if (insertAfter)
                {
                    insertIndex++;
                }

                break;
            }

            insertIndex++;
        }

        return MoveDraggedEditor(targetPanel, insertIndex);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        ResizeEditors();
    }

    private void ToolStripButtonAddRule_Click(object? sender, EventArgs e)
    {
        _rootEditor?.AddRule();
        NotifyChanged();
    }

    private void ToolStripButtonAddGroup_Click(object? sender, EventArgs e)
    {
        _rootEditor?.AddGroup();
        NotifyChanged();
    }

    private void ToolStripButtonImportJson_Click(object? sender, EventArgs e)
    {
        var args = new QueryBuilderRequestEventArgs();
        LoadQueryRequest?.Invoke(this, args);

        if (args.Handled)
        {
            return;
        }

        var form = new JsonEditorForm("Import JSON", GetRulesJson(), true);
        form.ShowDialog((f, result) =>
        {
            if (result == DialogResult.OK)
            {
                LoadRulesJson(form.JsonText);
            }

            form.Dispose();
        });
    }

    private void ToolStripButtonExportJson_Click(object? sender, EventArgs e)
    {
        var args = new QueryBuilderRequestEventArgs();
        SaveQueryRequest?.Invoke(this, args);

        if (args.Handled)
        {
            return;
        }

        var form = new JsonEditorForm("Export JSON", GetRulesJson(), false);
        form.ShowDialog((f, result) =>
        {
            form.Dispose();
        });
    }

    private void CreateEmptyRoot()
    {
        flowLayoutPanelRoot.Controls.Clear();

        _rootEditor = new GroupEditor(this, 0, true);
        flowLayoutPanelRoot.Controls.Add(_rootEditor);

        ResizeEditors();
        RaiseRulesChanged();
    }

    private void RaiseRulesChanged()
    {
        RulesChanged?.Invoke(this, new QueryBuilderChangedEventArgs(GetRules()));
    }

    private void RebindAllRuleEditors()
    {
        foreach (var editor in EnumerateControls(this).OfType<RuleEditor>())
        {
            editor.RebindColumns();
        }
    }

    private void ResizeEditors()
    {
        var maxRequiredWidth = 0;

        foreach (var group in EnumerateControls(flowLayoutPanelRoot).OfType<GroupEditor>())
        {
            if (group.Parent is not FlowLayoutPanel parentPanel)
            {
                continue;
            }

            var groupWidth = System.Math.Max(0, parentPanel.ClientSize.Width - parentPanel.Padding.Horizontal - group.Margin.Horizontal);
            group.Width = groupWidth;

            var requiredWidth = System.Math.Max(groupWidth, group.GetRequiredWidth());

            if (requiredWidth > maxRequiredWidth)
            {
                maxRequiredWidth = requiredWidth;
            }
        }

        foreach (var ruleEditor in EnumerateControls(flowLayoutPanelRoot).OfType<RuleEditor>())
        {
            if (ruleEditor.Parent is not FlowLayoutPanel parentPanel)
            {
                continue;
            }

            var ruleWidth = System.Math.Max(0, parentPanel.ClientSize.Width - parentPanel.Padding.Horizontal - ruleEditor.Margin.Horizontal);
            ruleEditor.Width = ruleWidth;
        }

        flowLayoutPanelRoot.AutoScrollMinSize = new System.Drawing.Size(maxRequiredWidth + flowLayoutPanelRoot.Padding.Horizontal, 0);
    }

    private bool CanDropOn(GroupEditor targetGroup)
    {
        if (_draggedEditor is null)
        {
            return false;
        }

        if (ReferenceEquals(_draggedEditor, targetGroup))
        {
            return false;
        }

        if (_draggedEditor is GroupEditor draggedGroup)
        {
            if (draggedGroup.IsRoot)
            {
                return false;
            }

            if (targetGroup.IsDescendantOf(draggedGroup))
            {
                return false;
            }
        }

        return true;
    }

    private bool CanDropRelativeTo(Control targetControl)
    {
        if (_draggedEditor is null)
        {
            return false;
        }

        if (ReferenceEquals(_draggedEditor, targetControl))
        {
            return false;
        }

        if (_draggedEditor is GroupEditor draggedGroup)
        {
            if (draggedGroup.IsRoot)
            {
                return false;
            }

            if (IsDescendantOf(targetControl, draggedGroup))
            {
                return false;
            }
        }

        return targetControl.Parent is FlowLayoutPanel;
    }

    private bool MoveDraggedEditor(FlowLayoutPanel targetPanel, int insertIndex)
    {
        if (_draggedEditor is null)
        {
            return false;
        }

        if (_draggedEditor.Parent is not Control sourceParent)
        {
            return false;
        }

        sourceParent.SuspendLayout();

        if (!ReferenceEquals(sourceParent, targetPanel))
        {
            targetPanel.SuspendLayout();
        }

        try
        {
            sourceParent.Controls.Remove(_draggedEditor);
            targetPanel.Controls.Add(_draggedEditor);

            if (targetPanel.Controls.Count > 0)
            {
                var childIndex = Math.Min(insertIndex, targetPanel.Controls.Count - 1);
                targetPanel.Controls.SetChildIndex(_draggedEditor, childIndex);
            }
        }
        finally
        {
            if (!ReferenceEquals(sourceParent, targetPanel))
            {
                targetPanel.ResumeLayout();
            }

            sourceParent.ResumeLayout();
        }

        EndDrag();
        NotifyChanged();
        return true;
    }

    private static bool IsDescendantOf(Control control, Control possibleAncestor)
    {
        var current = control.Parent;

        while (current != null)
        {
            if (ReferenceEquals(current, possibleAncestor))
            {
                return true;
            }

            current = current.Parent;
        }

        return false;
    }

    private string BuildSqlForGroup(
        string condition,
        List<QueryBuilderRuleNode> nodes,
        Dictionary<string, object?> parameters,
        ref int index,
        ISet<string> usedParameterNames)
    {
        var parts = new List<string>();
        var glue = string.Equals(condition, "or", StringComparison.OrdinalIgnoreCase) ? " OR " : " AND ";

        foreach (var node in nodes)
        {
            if (node.IsGroup && node.Rules is not null)
            {
                var nested = BuildSqlForGroup(node.Condition ?? "and", node.Rules, parameters, ref index, usedParameterNames);
                if (!string.IsNullOrWhiteSpace(nested))
                {
                    parts.Add("(" + nested + ")");
                }
            }
            else
            {
                var sql = BuildSqlForRule(node, parameters, ref index, usedParameterNames);
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    parts.Add(sql);
                }
            }
        }

        if (parts.Count == 0)
        {
            return string.Empty;
        }

        return "(" + string.Join(glue, parts) + ")";
    }

    private string BuildSqlForRule(
        QueryBuilderRuleNode node,
        Dictionary<string, object?> parameters,
        ref int index,
        ISet<string> usedParameterNames)
    {
        var column = FindColumn(node.Field);
        if (column == null)
        {
            return string.Empty;
        }

        var sqlField = string.IsNullOrWhiteSpace(column.SqlFieldName) ? column.Field : column.SqlFieldName!;
        var op = (node.Operator ?? "equal").ToLowerInvariant();

        switch (op)
        {
            case "equal":
                return string.Format("{0} = {1}", sqlField, AddParameter(parameters, column.Type, node.Value, ref index, usedParameterNames));
            case "notequal":
                return string.Format("{0} <> {1}", sqlField, AddParameter(parameters, column.Type, node.Value, ref index, usedParameterNames));
            case "greaterthan":
                return string.Format("{0} > {1}", sqlField, AddParameter(parameters, column.Type, node.Value, ref index, usedParameterNames));
            case "greaterthanorequal":
                return string.Format("{0} >= {1}", sqlField, AddParameter(parameters, column.Type, node.Value, ref index, usedParameterNames));
            case "lessthan":
                return string.Format("{0} < {1}", sqlField, AddParameter(parameters, column.Type, node.Value, ref index, usedParameterNames));
            case "lessthanorequal":
                return string.Format("{0} <= {1}", sqlField, AddParameter(parameters, column.Type, node.Value, ref index, usedParameterNames));
            case "contains":
                return string.Format("{0} LIKE {1}", sqlField, AddParameter(parameters, column.Type, "%" + Convert.ToString(node.Value, CultureInfo.InvariantCulture) + "%", ref index, usedParameterNames));
            case "startswith":
                return string.Format("{0} LIKE {1}", sqlField, AddParameter(parameters, column.Type, Convert.ToString(node.Value, CultureInfo.InvariantCulture) + "%", ref index, usedParameterNames));
            case "endswith":
                return string.Format("{0} LIKE {1}", sqlField, AddParameter(parameters, column.Type, "%" + Convert.ToString(node.Value, CultureInfo.InvariantCulture), ref index, usedParameterNames));
            case "isnull":
                return string.Format("{0} IS NULL", sqlField);
            case "isnotnull":
                return string.Format("{0} IS NOT NULL", sqlField);
            case "isempty":
                return string.Format("({0} IS NULL OR {0} = '')", sqlField);
            case "isnotempty":
                return string.Format("({0} IS NOT NULL AND {0} <> '')", sqlField);
            case "between":
                return string.Format("{0} BETWEEN {1} AND {2}",
                    sqlField,
                    AddParameter(parameters, column.Type, node.Value, ref index, usedParameterNames),
                    AddParameter(parameters, column.Type, node.Value2, ref index, usedParameterNames));
            case "notbetween":
                return string.Format("{0} NOT BETWEEN {1} AND {2}",
                    sqlField,
                    AddParameter(parameters, column.Type, node.Value, ref index, usedParameterNames),
                    AddParameter(parameters, column.Type, node.Value2, ref index, usedParameterNames));
            case "in":
            case "notin":
                return BuildInClause(sqlField, op, node.Value, column.Type, parameters, ref index, usedParameterNames);
            default:
                return string.Format("{0} = {1}", sqlField, AddParameter(parameters, column.Type, node.Value, ref index, usedParameterNames));
        }
    }

    private string BuildInClause(
        string sqlField,
        string op,
        object? value,
        QueryGridFieldType type,
        Dictionary<string, object?> parameters,
        ref int index,
        ISet<string> usedParameterNames)
    {
        var raw = Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        var parts = raw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var paramNames = new List<string>();

        foreach (var part in parts)
        {
            paramNames.Add(AddParameter(parameters, type, part.Trim(), ref index, usedParameterNames));
        }

        if (paramNames.Count == 0)
        {
            return "1=0";
        }

        var keyword = op == "notin" ? "NOT IN" : "IN";
        return string.Format("{0} {1} ({2})", sqlField, keyword, string.Join(", ", paramNames));
    }

    private static string AddParameter(
        Dictionary<string, object?> parameters,
        QueryGridFieldType type,
        object? value,
        ref int index,
        ISet<string> usedParameterNames)
    {
        string name;

        do
        {
            name = "@p" + index.ToString(CultureInfo.InvariantCulture);
            index++;
        }
        while (!usedParameterNames.Add(NormalizeParameterName(name)));

        parameters[name] = NormalizeJsonValue(value, type);
        return name;
    }

    private static object? NormalizeJsonValue(object? value, QueryGridFieldType type)
    {
        if (value is JToken token)
        {
            return type switch
            {
                QueryGridFieldType.Number when token.Type == JTokenType.Integer || token.Type == JTokenType.Float
                    => token.Value<decimal?>(),

                QueryGridFieldType.Boolean when token.Type == JTokenType.Boolean
                    => token.Value<bool>(),

                QueryGridFieldType.Date or QueryGridFieldType.DateTime
                    when token.Type == JTokenType.Date
                    => token.Value<System.DateTime?>(),

                _ when token.Type == JTokenType.String
                    => token.Value<string>(),

                _ => token.ToString()
            };
        }

        return value;
    }

    internal static List<QueryBuilderOperator> GetDefaultOperators(QueryGridFieldType type)
    {
        Func<string, string> L = QueryBuilderLocalizer.GetOperatorText;

        var result = new List<QueryBuilderOperator>
        {
            new QueryBuilderOperator { Key = "equal",     Text = L("op_equal"),     ValueMode = OperatorValueMode.Single },
            new QueryBuilderOperator { Key = "notequal",  Text = L("op_notequal"),  ValueMode = OperatorValueMode.Single },
            new QueryBuilderOperator { Key = "isnull",    Text = L("op_isnull"),    RequiresValue = false, ValueMode = OperatorValueMode.None },
            new QueryBuilderOperator { Key = "isnotnull", Text = L("op_isnotnull"), RequiresValue = false, ValueMode = OperatorValueMode.None }
        };

        if (type == QueryGridFieldType.String)
        {
            result.AddRange(new[]
            {
                new QueryBuilderOperator { Key = "contains",   Text = L("op_contains"),   ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "startswith", Text = L("op_startswith"), ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "endswith",   Text = L("op_endswith"),   ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "isempty",    Text = L("op_isempty"),    RequiresValue = false, ValueMode = OperatorValueMode.None },
                new QueryBuilderOperator { Key = "isnotempty", Text = L("op_isnotempty"), RequiresValue = false, ValueMode = OperatorValueMode.None },
                new QueryBuilderOperator { Key = "between",    Text = L("op_between"),    ValueMode = OperatorValueMode.Range },
                new QueryBuilderOperator { Key = "notbetween", Text = L("op_notbetween"), ValueMode = OperatorValueMode.Range },
                new QueryBuilderOperator { Key = "in",         Text = L("op_in"),         ValueMode = OperatorValueMode.List },
                new QueryBuilderOperator { Key = "notin",      Text = L("op_notin"),      ValueMode = OperatorValueMode.List },

                new QueryBuilderOperator { Key = "greaterthan",        Text = L("op_greaterthan"),        ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "greaterthanorequal", Text = L("op_greaterthanorequal"), ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "lessthan",           Text = L("op_lessthan"),           ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "lessthanorequal",    Text = L("op_lessthanorequal"),    ValueMode = OperatorValueMode.Single }
            });

            //if (ShowStringComparisonOperators)
            //{
            //    result.AddRange(new[]
            //    {
            //        new QueryBuilderOperator { Key = "greaterthan",        Text = L("op_greaterthan"),        ValueMode = OperatorValueMode.Single },
            //        new QueryBuilderOperator { Key = "greaterthanorequal", Text = L("op_greaterthanorequal"), ValueMode = OperatorValueMode.Single },
            //        new QueryBuilderOperator { Key = "lessthan",           Text = L("op_lessthan"),           ValueMode = OperatorValueMode.Single },
            //        new QueryBuilderOperator { Key = "lessthanorequal",    Text = L("op_lessthanorequal"),    ValueMode = OperatorValueMode.Single }
            //    });
            //}
        }

        if (type == QueryGridFieldType.Number
            || type == QueryGridFieldType.Date
            || type == QueryGridFieldType.DateTime)
        {
            result.AddRange(new[]
            {
                new QueryBuilderOperator { Key = "greaterthan",        Text = L("op_greaterthan"),        ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "greaterthanorequal", Text = L("op_greaterthanorequal"), ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "lessthan",           Text = L("op_lessthan"),           ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "lessthanorequal",    Text = L("op_lessthanorequal"),    ValueMode = OperatorValueMode.Single },
                new QueryBuilderOperator { Key = "between",            Text = L("op_between"),            ValueMode = OperatorValueMode.Range },
                new QueryBuilderOperator { Key = "notbetween",         Text = L("op_notbetween"),         ValueMode = OperatorValueMode.Range },
                new QueryBuilderOperator { Key = "in",                 Text = L("op_in"),                 ValueMode = OperatorValueMode.List },
                new QueryBuilderOperator { Key = "notin",              Text = L("op_notin"),              ValueMode = OperatorValueMode.List }
            });
        }

        return result;
    }

    private static IEnumerable<Control> EnumerateControls(Control root)
    {
        foreach (Control child in root.Controls)
        {
            yield return child;

            foreach (var descendant in EnumerateControls(child))
            {
                yield return descendant;
            }
        }
    }

    private void ToolBar_ButtonClick(object? sender, ToolBarButtonClickEventArgs e)
    {
        if (e.Button == toolBarButtonAddRule)
        {
            _rootEditor?.AddRule();
            NotifyChanged();
        }
        else if (e.Button == toolBarButtonAddGroup)
        {
            _rootEditor?.AddGroup();
            NotifyChanged();
        }
        else if (e.Button == toolBarButtonImportJson)
        {
            ToolStripButtonImportJson_Click(sender, EventArgs.Empty);
        }
        else if (e.Button == toolBarButtonExportJson)
        {
            ToolStripButtonExportJson_Click(sender, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Imposta la collezione <see cref="QBEColumns"/> e popola automaticamente
    /// le colonne del QueryBuilder. Solo le colonne con <c>UseInQBE = true</c> vengono incluse.
    /// </summary>
    public QBEColumns QBEColumns
    {
        get => _qbeColumns;
        set
        {
            _qbeColumns = value;
            if (_qbeColumns != null)
            {
                SetColumns(ToQueryBuilderColumns(_qbeColumns));
            }
        }
    }

    private QBEColumns _qbeColumns = new QBEColumns();

    /// <summary>
    /// Converte una <see cref="QBEColumns"/> in una lista di <see cref="QueryBuilderColumn"/>
    /// pronta per essere passata a <see cref="SetColumns"/>.
    /// Solo le colonne con <c>UseInQBE = true</c> vengono incluse, ordinate per <c>OrdinalPosition</c>.
    /// </summary>
    /// 

    /// <summary>
    /// Converte una <see cref="QBEColumns"/> in una lista di <see cref="QueryBuilderColumn"/>
    /// pronta per essere passata a <see cref="SetColumns"/>.
    /// Solo le colonne con <c>UseInQBE = true</c> vengono incluse, ordinate per <c>OrdinalPosition</c>.
    /// Se una colonna ha <see cref="QBEColumn.QBEValue"/> non null o vuoto, viene aggiunta una
    /// condizione al gruppo di default.
    /// </summary>
    public static IEnumerable<QueryBuilderColumn> ToQueryBuilderColumns(QBEColumns qbeColumns)
    {
        if (qbeColumns == null)
        {
            return Enumerable.Empty<QueryBuilderColumn>();
        }

        var result = new List<QueryBuilderColumn>();

        foreach (var entry in qbeColumns)
        {
            var qbeCol = entry.Value;

            if (!qbeCol.UseInQBE)
            {
                continue;
            }

            result.Add(new QueryBuilderColumn
            {
                Field = qbeCol.DbColumnName,
                Label = string.IsNullOrWhiteSpace(qbeCol.FriendlyName)
                    ? qbeCol.DbColumnName
                    : qbeCol.FriendlyName,
                SqlFieldName = qbeCol.DbColumnName,
                Type = MapQBEColumnType(qbeCol)
            });
        }

        result.Sort((a, b) =>
        {
            var posA = qbeColumns.ContainsKey(a.Field.ToUpperInvariant())
                ? qbeColumns[a.Field.ToUpperInvariant()].OrdinalPosition : 0;
            var posB = qbeColumns.ContainsKey(b.Field.ToUpperInvariant())
                ? qbeColumns[b.Field.ToUpperInvariant()].OrdinalPosition : 0;
            return posA.CompareTo(posB);
        });

        return result;
    }

    /// <summary>
    /// Carica le colonne da QBEColumns nel QueryBuilder e aggiunge automaticamente
    /// condizioni al gruppo di default per le colonne che hanno un <see cref="QBEColumn.QBEValue"/>
    /// non null o vuoto.
    /// </summary>
    /// <param name="qbeColumns">La collezione sorgente di colonne QBE. Se null, usa <see cref="QBEColumns"/>.</param>
    /// 
    /// <summary>
    /// Carica le colonne da QBEColumns nel QueryBuilder e aggiunge automaticamente
    /// condizioni al gruppo di default per le colonne che hanno un <see cref="QBEColumn.QBEValue"/>
    /// non null o vuoto.
    /// </summary>
    /// <param name="qbeColumns">La collezione sorgente di colonne QBE. Se null, usa <see cref="QBEColumns"/>.</param>
    public void LoadColumnsAndRulesFromQBE(QBEColumns qbeColumns = null)
    {
        if (qbeColumns == null)
        {
            qbeColumns = this.QBEColumns;
        }

        SetColumns(ToQueryBuilderColumns(qbeColumns));

        // Aggiunge regole di default per le colonne con QBEValue non vuoto
        if (_rootEditor != null)
        {
            foreach (var entry in qbeColumns)
            {
                var qbeCol = entry.Value;

                if (!qbeCol.UseInQBE)
                {
                    continue;
                }

                var qbeValue = qbeCol.QBEValue?.ToString();
                if (string.IsNullOrWhiteSpace(qbeValue))
                {
                    continue;
                }

                // Crea e aggiunge la regola al gruppo di default
                var ruleNode = new QueryBuilderRuleNode
                {
                    Field = qbeCol.DbColumnName,
                    Operator = "equal",
                    Value = qbeValue
                };

                _rootEditor.AddRuleToGroup(ruleNode);
            }

            ResizeEditors();
            RaiseRulesChanged();
        }
    }
    public void LoadColumnsAndRulesFromQBE_OLD(QBEColumns qbeColumns = null)
    {
        if (qbeColumns == null)
        {
            qbeColumns = this.QBEColumns;
        }

        SetColumns(ToQueryBuilderColumns(qbeColumns));

        // Aggiunge regole di default per le colonne con QBEValue non vuoto
        if (_rootEditor != null)
        {
            foreach (var entry in qbeColumns)
            {
                var qbeCol = entry.Value;

                if (!qbeCol.UseInQBE)
                {
                    continue;
                }

                var qbeValue = qbeCol.QBEValue?.ToString();
                if (string.IsNullOrWhiteSpace(qbeValue))
                {
                    continue;
                }

                // Crea una regola per questa colonna nel gruppo di default
                var rule = new QueryBuilderRuleNode
                {
                    Field = qbeCol.DbColumnName,
                    Operator = "equal",
                    Value = qbeValue
                };

                _rootEditor.AddRuleToGroup(rule);
            }

            NotifyChanged();
        }
    }

    public static IEnumerable<QueryBuilderColumn> ToQueryBuilderColumns_OLD(QBEColumns qbeColumns)
    {
        if (qbeColumns == null)
        {
            return Enumerable.Empty<QueryBuilderColumn>();
        }

        var result = new List<QueryBuilderColumn>();

        foreach (var entry in qbeColumns)
        {
            var qbeCol = entry.Value;

            if (!qbeCol.UseInQBE)
            {
                continue;
            }

            result.Add(new QueryBuilderColumn
            {
                Field = qbeCol.DbColumnName,
                Label = string.IsNullOrWhiteSpace(qbeCol.FriendlyName)
                    ? qbeCol.DbColumnName
                    : qbeCol.FriendlyName,
                SqlFieldName = qbeCol.DbColumnName,
                Type = MapQBEColumnType(qbeCol)
            });
        }

        result.Sort((a, b) =>
        {
            var posA = qbeColumns.ContainsKey(a.Field.ToUpperInvariant())
                ? qbeColumns[a.Field.ToUpperInvariant()].OrdinalPosition : 0;
            var posB = qbeColumns.ContainsKey(b.Field.ToUpperInvariant())
                ? qbeColumns[b.Field.ToUpperInvariant()].OrdinalPosition : 0;
            return posA.CompareTo(posB);
        });

        return result;
    }

    private static QueryGridFieldType MapQBEColumnType(QBEColumn qbeCol)
    {
        switch (qbeCol.QBEColumnType)
        {
            case QBEColumnsTypes.CheckBox:
                return QueryGridFieldType.Boolean;
            case QBEColumnsTypes.DatePicker:
                return QueryGridFieldType.Date;
            case QBEColumnsTypes.DateTimePicker:
                return QueryGridFieldType.DateTime;
            case QBEColumnsTypes.ComboBox:
                return QueryGridFieldType.Enum;
            default:
                return qbeCol.QBEDbColumn != null
                    ? MapFromDbColumn(qbeCol.QBEDbColumn)
                    : QueryGridFieldType.String;
        }
    }

    private static QueryGridFieldType MapFromDbColumn(QBEDbColumn dbColumn)
    {
        if (dbColumn.IsBoolean)  return QueryGridFieldType.Boolean;
        if (dbColumn.IsNumeric)  return QueryGridFieldType.Number;
        if (dbColumn.IsDateTime) return QueryGridFieldType.DateTime;
        if (dbColumn.IsDate)     return QueryGridFieldType.Date;
        return QueryGridFieldType.String;
    }

    private static void CopyDynamicParameters(
        Dapper.DynamicParameters source,
        Dapper.DynamicParameters destination,
        ISet<string> usedParameterNames)
    {
        if (source == null)
        {
            return;
        }

        foreach (var parameterName in source.ParameterNames)
        {
            destination.Add(parameterName, source.Get<object>(parameterName));
            usedParameterNames.Add(NormalizeParameterName(parameterName));
        }
    }

    /// <summary>
    /// Converte un dizionario di oggetti in DynamicParameters di Dapper.
    /// </summary>
    /// <param name="dictionary">Il dizionario con i parametri.</param>
    /// <returns>Un oggetto DynamicParameters contenente i valori del dizionario.</returns>
    public static Dapper.DynamicParameters DictionaryToDynamicParameters(Dictionary<string, object?> dictionary)
    {
        if (dictionary == null)
        {
            return new Dapper.DynamicParameters();
        }

        var parameters = new Dapper.DynamicParameters();

        foreach (var kvp in dictionary)
        {
            if (string.IsNullOrWhiteSpace(kvp.Key))
            {
                continue;
            }

            parameters.Add(kvp.Key, kvp.Value);
        }

        return parameters;
    }

    private static void CopyDynamicParametersToDictionary(
        Dapper.DynamicParameters source,
        Dictionary<string, object?> destination)
    {
        if (source == null)
        {
            return;
        }

        foreach (var parameterName in source.ParameterNames)
        {
            destination[parameterName] = source.Get<object>(parameterName);
        }
    }

    private static string NormalizeParameterName(string parameterName)
    {
        return string.IsNullOrWhiteSpace(parameterName)
            ? string.Empty
            : parameterName.TrimStart('@', ':', '?');
    }

    private static string ComposeSqlQuery(string defaultSqlQuery, string filterClause)
    {
        defaultSqlQuery = defaultSqlQuery?.Trim() ?? string.Empty;
        filterClause = filterClause?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(defaultSqlQuery))
        {
            return filterClause;
        }

        if (string.IsNullOrWhiteSpace(filterClause))
        {
            return defaultSqlQuery;
        }

        return defaultSqlQuery.IndexOf(" where ", StringComparison.OrdinalIgnoreCase) >= 0
            ? $"{defaultSqlQuery} AND {filterClause}"
            : $"{defaultSqlQuery} WHERE {filterClause}";
    }

    private bool RaiseSaveQueryRequest()
    {
        var args = new QueryBuilderRequestEventArgs();
        SaveQueryRequest?.Invoke(this, args);
        return args.Handled;
    }

    private bool RaiseLoadQueryRequest()
    {
        var args = new QueryBuilderRequestEventArgs();
        LoadQueryRequest?.Invoke(this, args);
        return args.Handled;
    }

    /// <summary>
    /// Estrae la clausola WHERE da <see cref="SQLQuery"/>, la parsea usando <see cref="SQLQueryParameters"/>,
    /// e carica il risultato come query corrente nel QueryBuilder.
    /// </summary>
    /// <remarks>
    /// Questo metodo è utile per ricaricare una query precedentemente salvata.
    /// Se l'estrazione o il parsing della WHERE fallisce, il QueryBuilder viene svuotato.
    /// Se <see cref="Columns"/> è vuoto, prova a caricare le colonne da <see cref="QBEColumns"/>.
    /// </remarks>
    public void LoadQueryFromSql(string SQLQuery, Dapper.DynamicParameters Parameters)
    {
        if (string.IsNullOrWhiteSpace(SQLQuery))
        {
            ClearRules();
            return;
        }

        // Se le colonne del QueryBuilder sono vuote, carica da QBEColumns
        if (Columns.Count == 0 && QBEColumns.Count > 0)
        {
            LoadColumnsAndRulesFromQBE(QBEColumns);
        }

        var whereClause = ExtractWhereClause(SQLQuery);

        if (string.IsNullOrWhiteSpace(whereClause))
        {
            ClearRules();
            return;
        }
        this.SQLQuery = SQLQuery;
        this.Parameters = Parameters;

        var ruleSet = ParseSqlToRuleSet(whereClause, Parameters);
        LoadRules(ruleSet);
        
    }

    /// <summary>
    /// Estrae la clausola WHERE da una query SQL completa.
    /// </summary>
    /// <param name="sqlQuery">La query SQL completa.</param>
    /// <returns>La clausola WHERE senza il prefisso WHERE, oppure una stringa vuota se non trovata.</returns>
    private string ExtractWhereClause(string sqlQuery)
    {
        if (string.IsNullOrWhiteSpace(sqlQuery))
        {
            return string.Empty;
        }

        var upperQuery = sqlQuery.ToUpperInvariant();
        var whereIndex = upperQuery.IndexOf(" WHERE ");

        if (whereIndex < 0)
        {
            return string.Empty;
        }

        var whereClause = sqlQuery.Substring(whereIndex + 7).Trim();

        // Rimuovi eventuali clausole ORDER BY, GROUP BY, HAVING, etc. che potrebbero seguire
        var orderByIndex = whereClause.ToUpperInvariant().IndexOf(" ORDER BY ");
        if (orderByIndex >= 0)
        {
            whereClause = whereClause.Substring(0, orderByIndex).Trim();
        }

        var groupByIndex = whereClause.ToUpperInvariant().IndexOf(" GROUP BY ");
        if (groupByIndex >= 0)
        {
            whereClause = whereClause.Substring(0, groupByIndex).Trim();
        }

        var havingIndex = whereClause.ToUpperInvariant().IndexOf(" HAVING ");
        if (havingIndex >= 0)
        {
            whereClause = whereClause.Substring(0, havingIndex).Trim();
        }

        return whereClause;
    }

    /// <summary>
    /// Ricostruisce un <see cref="QueryBuilderRuleSet"/> partendo da una clausola WHERE SQL e dai parametri.
    /// Tenta di parsare l'SQL e associare i parametri alle condizioni per rigenerare la struttura
    /// delle regole del QueryBuilder.
    /// </summary>
    /// <param name="whereClause">La clausola WHERE SQL (senza il prefisso WHERE).</param>
    /// <param name="parameters">I parametri Dapper associati alla query.</param>
    /// <returns>Un <see cref="QueryBuilderRuleSet"/> ricostruito, oppure un set vuoto se il parsing fallisce.</returns>
    public QueryBuilderRuleSet ParseSqlToRuleSet(string whereClause, Dapper.DynamicParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(whereClause) || parameters == null)
        {
            return new QueryBuilderRuleSet();
        }

        try
        {
            var ruleSet = new QueryBuilderRuleSet();
            ruleSet.Rules = ParseSqlExpression(whereClause.Trim(), parameters);
            return ruleSet;
        }
        catch
        {
            // Se il parsing fallisce, restituisci un set vuoto
            return new QueryBuilderRuleSet();
        }
    }

    private List<QueryBuilderRuleNode> ParseSqlExpression(string expression, Dapper.DynamicParameters parameters)
    {
        var rules = new List<QueryBuilderRuleNode>();

        if (string.IsNullOrWhiteSpace(expression))
        {
            return rules;
        }

        expression = expression.Trim();

        // Rimuovi parentesi esterne bilanciate
        while (expression.StartsWith("(") && expression.EndsWith(")"))
        {
            var innerContent = expression.Substring(1, expression.Length - 2).Trim();
            if (!IsBalancedParenthesis(innerContent))
            {
                break;
            }
            expression = innerContent;
        }

        if (string.IsNullOrWhiteSpace(expression))
        {
            return rules;
        }

        // Dividi per AND/OR al livello di profondità 0
        var parts = SplitByTopLevelSeparator(expression);

        if (parts.Count == 0)
        {
            return rules;
        }

        // Tutte le parti vengono parsate come singole regole
        foreach (var part in parts)
        {
            var trimmedPart = part.Trim();

            if (string.IsNullOrWhiteSpace(trimmedPart))
            {
                continue;
            }

            // Rimuovi parentesi anche dalle singole parti
            while (trimmedPart.StartsWith("(") && trimmedPart.EndsWith(")"))
            {
                var innerContent = trimmedPart.Substring(1, trimmedPart.Length - 2).Trim();
                if (!IsBalancedParenthesis(innerContent))
                {
                    break;
                }
                trimmedPart = innerContent;
            }

            // Prova a parsare come singola regola
            var rule = ParseSqlRule(trimmedPart, parameters);
            if (rule != null)
            {
                rules.Add(rule);
            }
            else if (trimmedPart.ToUpperInvariant().Contains(" AND ") || 
                     trimmedPart.ToUpperInvariant().Contains(" OR "))
            {
                // Se contiene ancora AND/OR, ricorsione
                var nestedRules = ParseSqlExpression(trimmedPart, parameters);
                rules.AddRange(nestedRules);
            }
        }

        return rules;
    }

    private QueryBuilderRuleNode? ParseSqlRule(string ruleSql, Dapper.DynamicParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(ruleSql))
        {
            return null;
        }

        ruleSql = ruleSql.Trim();

        // Rimuovi parentesi esterne bilanciate
        while (ruleSql.StartsWith("(") && ruleSql.EndsWith(")"))
        {
            var innerContent = ruleSql.Substring(1, ruleSql.Length - 2).Trim();
            if (!IsBalancedParenthesis(innerContent))
            {
                break;
            }
            ruleSql = innerContent;
        }

        if (string.IsNullOrWhiteSpace(ruleSql))
        {
            return null;
        }

        var ruleSql_Upper = ruleSql.ToUpperInvariant();

        // Gestisci IS NULL / IS NOT NULL
        if (ruleSql_Upper.EndsWith(" IS NULL"))
        {
            var fieldName = ruleSql.Substring(0, ruleSql.Length - 8).Trim();
            var column = FindColumnByName(fieldName);
            if (column != null)
            {
                return new QueryBuilderRuleNode
                {
                    Field = column.Field,
                    Label = column.Label,
                    Type = column.Type.ToString().ToLowerInvariant(),
                    Operator = "isnull",
                    Value = null
                };
            }
        }

        if (ruleSql_Upper.EndsWith(" IS NOT NULL"))
        {
            var fieldName = ruleSql.Substring(0, ruleSql.Length - 12).Trim();
            var column = FindColumnByName(fieldName);
            if (column != null)
            {
                return new QueryBuilderRuleNode
                {
                    Field = column.Field,
                    Label = column.Label,
                    Type = column.Type.ToString().ToLowerInvariant(),
                    Operator = "isnotnull",
                    Value = null
                };
            }
        }

        // Gestisci LIKE (incluso UPPER/LOWER wrap)
        if (ruleSql_Upper.Contains(" LIKE "))
        {
            var likeIdx = ruleSql_Upper.IndexOf(" LIKE ");
            var fieldPart = ruleSql.Substring(0, likeIdx).Trim();
            var paramPart = ruleSql.Substring(likeIdx + 6).Trim();

            // Estrai il nome del campo anche se è wrapped in UPPER() o LOWER()
            var fieldName = ExtractFieldName(fieldPart);
            var paramName = ExtractParameterName(paramPart);

            var column = FindColumnByName(fieldName);

            if (column != null && !string.IsNullOrWhiteSpace(paramName) && 
                parameters.ParameterNames.Contains(paramName.TrimStart('@')))
            {
                var paramValue = parameters.Get<object>(paramName.TrimStart('@'))?.ToString() ?? "";
                var op = "contains";

                if (paramValue.StartsWith("%") && paramValue.EndsWith("%"))
                {
                    op = "contains";
                    paramValue = paramValue.Substring(1, paramValue.Length - 2);
                }
                else if (paramValue.StartsWith("%"))
                {
                    op = "endswith";
                    paramValue = paramValue.Substring(1);
                }
                else if (paramValue.EndsWith("%"))
                {
                    op = "startswith";
                    paramValue = paramValue.Substring(0, paramValue.Length - 1);
                }

                return new QueryBuilderRuleNode
                {
                    Field = column.Field,
                    Label = column.Label,
                    Type = column.Type.ToString().ToLowerInvariant(),
                    Operator = op,
                    Value = paramValue
                };
            }
        }

        // Gestisci IN e NOT IN
        if (ruleSql_Upper.Contains(" IN (") || ruleSql_Upper.Contains(" NOT IN ("))
        {
            var isIn = ruleSql_Upper.Contains(" IN (");
            var keyword = isIn ? " IN (" : " NOT IN (";
            var inIdx = ruleSql_Upper.IndexOf(keyword);

            if (inIdx >= 0)
            {
                var closeParenIdx = ruleSql.IndexOf(')', inIdx);

                if (closeParenIdx > inIdx)
                {
                    var fieldName = ruleSql.Substring(0, inIdx).Trim();
                    var inParamsPart = ruleSql.Substring(inIdx + keyword.Length, closeParenIdx - inIdx - keyword.Length);
                    var column = FindColumnByName(fieldName);

                    if (column != null)
                    {
                        var paramNames = inParamsPart.Split(',');
                        var values = new List<object?>();

                        foreach (var paramName in paramNames)
                        {
                            var cleanName = paramName.Trim().TrimStart('@');
                            if (!string.IsNullOrWhiteSpace(cleanName) && parameters.ParameterNames.Contains(cleanName))
                            {
                                values.Add(parameters.Get<object>(cleanName));
                            }
                        }

                        if (values.Count > 0)
                        {
                            return new QueryBuilderRuleNode
                            {
                                Field = column.Field,
                                Label = column.Label,
                                Type = column.Type.ToString().ToLowerInvariant(),
                                Operator = isIn ? "in" : "notin",
                                Value = string.Join(", ", values)
                            };
                        }
                    }
                }
            }
        }

        // Gestisci operatori standard: =, <>, <=, >=, <, >
        // ORDINE IMPORTANTE: controllare prima i doppi caratteri (<=, >=, <>)
        var operatorMatches = new[]
        {
            new { Op = "<>", Key = "notequal" },
            new { Op = ">=", Key = "greaterthanorequal" },
            new { Op = "<=", Key = "lessthanorequal" },
            new { Op = "=", Key = "equal" },
            new { Op = ">", Key = "greaterthan" },
            new { Op = "<", Key = "lessthan" }
        };

        foreach (var opMatch in operatorMatches)
        {
            var idx = ruleSql_Upper.IndexOf(opMatch.Op);
            if (idx >= 0)
            {
                var fieldName = ruleSql.Substring(0, idx).Trim();
                var paramName = ruleSql.Substring(idx + opMatch.Op.Length).Trim();
                var column = FindColumnByName(fieldName);

                if (column != null && 
                    !string.IsNullOrWhiteSpace(paramName) &&
                    parameters.ParameterNames.Contains(paramName.TrimStart('@')))
                {
                    return new QueryBuilderRuleNode
                    {
                        Field = column.Field,
                        Label = column.Label,
                        Type = column.Type.ToString().ToLowerInvariant(),
                        Operator = opMatch.Key,
                        Value = parameters.Get<object>(paramName.TrimStart('@'))
                    };
                }
            }
        }

        return null;
    }

    private string ExtractFieldName(string text)
    {
        text = text.Trim();
        var upper = text.ToUpperInvariant();

        // Rimuovi UPPER(...) o LOWER(...)
        if (upper.StartsWith("UPPER(") && text.EndsWith(")"))
        {
            text = text.Substring(6, text.Length - 7).Trim();
        }
        else if (upper.StartsWith("LOWER(") && text.EndsWith(")"))
        {
            text = text.Substring(6, text.Length - 7).Trim();
        }

        return text;
    }

    private string ExtractParameterName(string text)
    {
        text = text.Trim();
        var upper = text.ToUpperInvariant();

        // Rimuovi UPPER(...) o LOWER(...)
        if (upper.StartsWith("UPPER(") && text.EndsWith(")"))
        {
            text = text.Substring(6, text.Length - 7).Trim();
        }
        else if (upper.StartsWith("LOWER(") && text.EndsWith(")"))
        {
            text = text.Substring(6, text.Length - 7).Trim();
        }

        return text;
    }

    private List<string> SplitByTopLevelSeparator(string expression)
    {
        var parts = new List<string>();
        var current = new System.Text.StringBuilder();
        var depth = 0;
        var i = 0;

        while (i < expression.Length)
        {
            if (expression[i] == '(')
            {
                depth++;
                current.Append(expression[i]);
                i++;
            }
            else if (expression[i] == ')')
            {
                depth--;
                current.Append(expression[i]);
                i++;
            }
            else if (depth == 0)
            {
                // Verifica AND
                if (i + 5 <= expression.Length && 
                    expression.Substring(i, 5).Equals(" AND ", StringComparison.OrdinalIgnoreCase))
                {
                    if (current.Length > 0)
                    {
                        parts.Add(current.ToString());
                        current.Clear();
                    }
                    i += 5;
                    continue;
                }

                // Verifica OR
                if (i + 4 <= expression.Length && 
                    expression.Substring(i, 4).Equals(" OR ", StringComparison.OrdinalIgnoreCase))
                {
                    if (current.Length > 0)
                    {
                        parts.Add(current.ToString());
                        current.Clear();
                    }
                    i += 4;
                    continue;
                }

                current.Append(expression[i]);
                i++;
            }
            else
            {
                current.Append(expression[i]);
                i++;
            }
        }

        if (current.Length > 0)
        {
            parts.Add(current.ToString());
        }

        return parts;
    }

    private bool IsBalancedParenthesis(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return true;
        }

        var depth = 0;
        foreach (var ch in expression)
        {
            if (ch == '(')
            {
                depth++;
            }
            else if (ch == ')')
            {
                depth--;
            }

            if (depth < 0)
            {
                return false;
            }
        }

        return depth == 0;
    }

    private string DetermineMainCondition(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return "and";
        }

        var upperExpr = expression.ToUpperInvariant();
        var depth = 0;

        for (int i = 0; i < upperExpr.Length; i++)
        {
            if (upperExpr[i] == '(')
            {
                depth++;
            }
            else if (upperExpr[i] == ')')
            {
                depth--;
            }
            else if (depth == 0)
            {
                if (i + 4 <= upperExpr.Length && upperExpr.Substring(i, 4) == " OR ")
                {
                    return "or";
                }
            }
        }

        return "and";
    }

    private QueryBuilderColumn? FindColumnByName(string sqlFieldName)
    {
        if (string.IsNullOrWhiteSpace(sqlFieldName))
        {
            return null;
        }

        sqlFieldName = sqlFieldName.Trim();

        // Rimuovi parentesi quadre (sintassi SQL Server)
        if (sqlFieldName.StartsWith("[") && sqlFieldName.EndsWith("]"))
        {
            sqlFieldName = sqlFieldName.Substring(1, sqlFieldName.Length - 2);
        }

        // Prova prima a cercare per Field (match esatto)
        foreach (var column in Columns)
        {
            if (string.Equals(column.Field, sqlFieldName, StringComparison.OrdinalIgnoreCase))
            {
                return column;
            }
        }

        // Prova a cercare per SqlFieldName
        foreach (var column in Columns)
        {
            if (!string.IsNullOrWhiteSpace(column.SqlFieldName) &&
                string.Equals(column.SqlFieldName, sqlFieldName, StringComparison.OrdinalIgnoreCase))
            {
                return column;
            }
        }

        // Ultima risorsa: cerca per una sottostringa (utile se il campo è "dbo.Authors.au_id")
        foreach (var column in Columns)
        {
            if (column.Field.EndsWith(sqlFieldName, StringComparison.OrdinalIgnoreCase))
            {
                return column;
            }
        }

        return null;
    }
}
