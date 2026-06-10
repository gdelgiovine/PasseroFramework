using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
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


    /// <summary>
    /// Converte una <see cref="QBEColumns"/> in colonne del QueryBuilder e le applica
    /// tramite <see cref="SetColumns"/>. Solo le colonne con <c>UseInQBE = true</c>
    /// vengono incluse, ordinate per <c>OrdinalPosition</c>.
    /// </summary>
    /// <param name="qbeColumns">La collezione sorgente di colonne QBE.</param>
    public void LoadColumnsFromQBE(QBEColumns qbeColumns = null)
    {
        if (qbeColumns == null)
        {
            qbeColumns = this.QBEColumns;
        }   
        SetColumns(ToQueryBuilderColumns(qbeColumns));
    }


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
        QueryBuilderFieldType type,
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
        QueryBuilderFieldType type,
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

    private static object? NormalizeJsonValue(object? value, QueryBuilderFieldType type)
    {
        if (value is JToken token)
        {
            return type switch
            {
                QueryBuilderFieldType.Number when token.Type == JTokenType.Integer || token.Type == JTokenType.Float
                    => token.Value<decimal?>(),

                QueryBuilderFieldType.Boolean when token.Type == JTokenType.Boolean
                    => token.Value<bool>(),

                QueryBuilderFieldType.Date or QueryBuilderFieldType.DateTime
                    when token.Type == JTokenType.Date
                    => token.Value<System.DateTime?>(),

                _ when token.Type == JTokenType.String
                    => token.Value<string>(),

                _ => token.ToString()
            };
        }

        return value;
    }

    internal static List<QueryBuilderOperator> GetDefaultOperators(QueryBuilderFieldType type)
    {
        Func<string, string> L = QueryBuilderLocalizer.GetOperatorText;

        var result = new List<QueryBuilderOperator>
        {
            new QueryBuilderOperator { Key = "equal",     Text = L("op_equal"),     ValueMode = OperatorValueMode.Single },
            new QueryBuilderOperator { Key = "notequal",  Text = L("op_notequal"),  ValueMode = OperatorValueMode.Single },
            new QueryBuilderOperator { Key = "isnull",    Text = L("op_isnull"),    RequiresValue = false, ValueMode = OperatorValueMode.None },
            new QueryBuilderOperator { Key = "isnotnull", Text = L("op_isnotnull"), RequiresValue = false, ValueMode = OperatorValueMode.None }
        };

        if (type == QueryBuilderFieldType.String)
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

        if (type == QueryBuilderFieldType.Number
            || type == QueryBuilderFieldType.Date
            || type == QueryBuilderFieldType.DateTime)
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

    private static QueryBuilderFieldType MapQBEColumnType(QBEColumn qbeCol)
    {
        switch (qbeCol.QBEColumnType)
        {
            case QBEColumnsTypes.CheckBox:
                return QueryBuilderFieldType.Boolean;
            case QBEColumnsTypes.DatePicker:
                return QueryBuilderFieldType.Date;
            case QBEColumnsTypes.DateTimePicker:
                return QueryBuilderFieldType.DateTime;
            case QBEColumnsTypes.ComboBox:
                return QueryBuilderFieldType.Enum;
            default:
                return qbeCol.QBEDbColumn != null
                    ? MapFromDbColumn(qbeCol.QBEDbColumn)
                    : QueryBuilderFieldType.String;
        }
    }

    private static QueryBuilderFieldType MapFromDbColumn(QBEDbColumn dbColumn)
    {
        if (dbColumn.IsBoolean)  return QueryBuilderFieldType.Boolean;
        if (dbColumn.IsNumeric)  return QueryBuilderFieldType.Number;
        if (dbColumn.IsDateTime) return QueryBuilderFieldType.DateTime;
        if (dbColumn.IsDate)     return QueryBuilderFieldType.Date;
        return QueryBuilderFieldType.String;
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
}
