using System;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using Wisej.Web;

namespace Passero.Framework.Controls;

public partial class RuleEditor : UserControl
{
    private QueryBuilderControl? _owner;
    private QueryBuilderFieldType _valueType = QueryBuilderFieldType.String;

    public RuleEditor()
    {
        InitializeComponent();
        this._rightPanel.Dock = DockStyle.Fill;
        SetPreferredHeight();
        WireEvents();
        UpdateValueEditorVisibility();
    }

    [DefaultValue(QueryBuilderFieldType.String)]
    public QueryBuilderFieldType ValueType
    {
        get => _valueType;
        set
        {
            if (_valueType != value)
            {
                _valueType = value;
                UpdateValueEditorVisibility();
                LayoutEditorControls();
            }
        }
    }

    public RuleEditor(QueryBuilderControl owner)
    {
        _owner = owner;

        InitializeComponent();
        SetPreferredHeight();
        WireEvents();
        UpdateValueEditorVisibility();

        RebindColumns();

        if (_fieldComboBox.Items.Count > 0)
        {
            _fieldComboBox.SelectedIndex = 0;
        }
    }

    private void SetPreferredHeight()
    {
        Height = _fieldComboBox.Height + 8;
    }

    private void DeleteSelf()
    {
        if (_owner is null)
        {
            return;
        }

        var parent = Parent;
        parent?.Controls.Remove(this);
        _owner.NotifyChanged();
        Dispose();
    }

    public void RebindColumns()
    {
        if (_owner is null)
        {
            return;
        }

        var selectedField = SelectedColumn?.Field;

        _fieldComboBox.BeginUpdate();
        _fieldComboBox.Items.Clear();

        foreach (var column in _owner.Columns)
        {
            _fieldComboBox.Items.Add(column);
        }

        _fieldComboBox.EndUpdate();

        LayoutEditorControls();

        if (!string.IsNullOrWhiteSpace(selectedField))
        {
            for (var i = 0; i < _fieldComboBox.Items.Count; i++)
            {
                if (_fieldComboBox.Items[i] is QueryBuilderColumn column && column.Field == selectedField)
                {
                    _fieldComboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        if (_fieldComboBox.Items.Count > 0 && _fieldComboBox.SelectedIndex < 0)
        {
            _fieldComboBox.SelectedIndex = 0;
        }

        UpdateValueTypeFromSelection();
    }

    public QueryBuilderRuleNode? ToRuleNode()
    {
        var column = SelectedColumn;
        var op = SelectedOperator;

        if (column == null || op == null)
        {
            return null;
        }

        return new QueryBuilderRuleNode
        {
            Field = column.Field,
            Label = column.Label,
            Type = column.Type.ToString().ToLowerInvariant(),
            Operator = op.Key,
            Value = op.RequiresValue ? ReadCurrentValue() : null,
            Value2 = op.ValueMode == OperatorValueMode.Range ? ReadCurrentValue2() : null
        };
    }

    public void LoadFromNode(QueryBuilderRuleNode node)
    {
        SelectColumn(node.Field);
        RebindOperators();
        SelectOperator(node.Operator);
        RecreateValueEditor(node.Value, node.Value2);
    }

    private QueryBuilderColumn? SelectedColumn => _fieldComboBox.SelectedItem as QueryBuilderColumn;

    private QueryBuilderOperator? SelectedOperator => _operatorComboBox.SelectedItem as QueryBuilderOperator;

    private void WireEvents()
    {
        AllowDrag = true;
        _dragHandleButton.AllowDrag = true;

        _fieldComboBox.SelectedIndexChanged += (_, _) =>
        {
            UpdateValueTypeFromSelection();
            RebindOperators();
            RecreateValueEditor(null, null);
            _owner?.NotifyChanged();
        };

        _operatorComboBox.SelectedIndexChanged += (_, _) =>
        {
            RecreateValueEditor(ReadCurrentValue(), ReadCurrentValue2());
            _owner?.NotifyChanged();
        };

        valueString.TextChanged += (_, _) => _owner?.NotifyChanged();
        valueEnum.SelectedIndexChanged += (_, _) => _owner?.NotifyChanged();
        valueNumeric.ValueChanged += (_, _) => _owner?.NotifyChanged();
        valueDateTime.ValueChanged += (_, _) => _owner?.NotifyChanged();
        valueBoolean.CheckedChanged += (_, _) => _owner?.NotifyChanged();
        value2String.TextChanged += (_, _) => _owner?.NotifyChanged();
        value2Enum.SelectedIndexChanged += (_, _) => _owner?.NotifyChanged();
        value2Numeric.ValueChanged += (_, _) => _owner?.NotifyChanged();
        value2DateTime.ValueChanged += (_, _) => _owner?.NotifyChanged();
        value2Boolean.CheckedChanged += (_, _) => _owner?.NotifyChanged();
        _deleteButton.Click += (_, _) => DeleteSelf();
        _dragHandleButton.MouseDown += DragHandleButton_MouseDown;
        RegisterDropTarget(this, RuleEditor_DragDrop);
        RegisterDropTarget(_rightPanel, RightPanel_DragDrop);
        RegisterDropTarget(_dragHandleButton, RightPanel_DragDrop);
        RegisterDropTarget(_fieldComboBox, RightPanel_DragDrop);
        RegisterDropTarget(_operatorComboBox, RightPanel_DragDrop);
        RegisterDropTarget(valueString, RightPanel_DragDrop);
        RegisterDropTarget(valueEnum, RightPanel_DragDrop);
        RegisterDropTarget(valueNumeric, RightPanel_DragDrop);
        RegisterDropTarget(valueDateTime, RightPanel_DragDrop);
        RegisterDropTarget(valueBoolean, RightPanel_DragDrop);
        RegisterDropTarget(value2String, RightPanel_DragDrop);
        RegisterDropTarget(value2Enum, RightPanel_DragDrop);
        RegisterDropTarget(value2Numeric, RightPanel_DragDrop);
        RegisterDropTarget(value2DateTime, RightPanel_DragDrop);
        RegisterDropTarget(value2Boolean, RightPanel_DragDrop);
        RegisterDropTarget(_deleteButton, RightPanel_DragDrop);
    }

    private void RegisterDropTarget(Control control, DragEventHandler dragDropHandler)
    {
        control.AllowDrop = true;
        control.DragEnter += RuleEditor_DragEnter;
        control.DragOver += RuleEditor_DragOver;
        control.DragDrop += dragDropHandler;
    }

    private void UpdateValueTypeFromSelection()
    {
        var column = SelectedColumn;
        ValueType = column?.Type ?? QueryBuilderFieldType.String;
    }

    private void RebindOperators()
    {
        if (_owner is null)
        {
            return;
        }

        var selectedOperator = SelectedOperator?.Key;

        _operatorComboBox.BeginUpdate();
        _operatorComboBox.Items.Clear();

        foreach (var op in _owner.GetOperatorsForColumn(SelectedColumn))
        {
            _operatorComboBox.Items.Add(op);
        }

        _operatorComboBox.EndUpdate();

        if (!string.IsNullOrWhiteSpace(selectedOperator))
        {
            SelectOperator(selectedOperator);
        }

        if (_operatorComboBox.Items.Count > 0 && _operatorComboBox.SelectedIndex < 0)
        {
            _operatorComboBox.SelectedIndex = 0;
        }
    }

    private void SelectColumn(string? field)
    {
        if (string.IsNullOrWhiteSpace(field))
        {
            return;
        }

        for (var i = 0; i < _fieldComboBox.Items.Count; i++)
        {
            if (_fieldComboBox.Items[i] is QueryBuilderColumn column
                && string.Equals(column.Field, field, System.StringComparison.OrdinalIgnoreCase))
            {
                _fieldComboBox.SelectedIndex = i;
                return;
            }
        }
    }

    private void SelectOperator(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        for (var i = 0; i < _operatorComboBox.Items.Count; i++)
        {
            if (_operatorComboBox.Items[i] is QueryBuilderOperator op
                && string.Equals(op.Key, key, System.StringComparison.OrdinalIgnoreCase))
            {
                _operatorComboBox.SelectedIndex = i;
                return;
            }
        }
    }

    private void RecreateValueEditor(object? value, object? value2 = null)
    {
        UpdateValueEditorVisibility();

        var primaryControl = GetPrimaryValueControl();
        if (primaryControl is null)
        {
            return;
        }

        ApplyValue(primaryControl, value);

        var secondaryControl = GetSecondaryValueControl();
        if (secondaryControl is not null)
        {
            ApplyValue(secondaryControl, value2);
        }

        LayoutEditorControls();
    }

    private Control CreateValueControl(QueryBuilderColumn column, object? value)
    {
        if (column.Values.Count > 0)
        {
            var comboBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

            foreach (var item in column.Values)
            {
                comboBox.Items.Add(item);
            }

            SelectLookupValue(comboBox, value);
            comboBox.SelectedIndexChanged += (_, _) => _owner.NotifyChanged();
            return comboBox;
        }

        switch (column.Type)
        {
            case QueryBuilderFieldType.Boolean:
                var booleanCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
                booleanCombo.Items.Add(new QueryBuilderLookupItem { Text = "True", Value = true });
                booleanCombo.Items.Add(new QueryBuilderLookupItem { Text = "False", Value = false });
                SelectLookupValue(booleanCombo, value);
                booleanCombo.SelectedIndexChanged += (_, _) => _owner.NotifyChanged();
                return booleanCombo;

            case QueryBuilderFieldType.Date:
            case QueryBuilderFieldType.DateTime:
                var datePicker = new DateTimePicker
                {
                    Format = column.Type == QueryBuilderFieldType.Date
                        ? DateTimePickerFormat.Short
                        : DateTimePickerFormat.Custom,
                    CustomFormat = "dd/MM/yyyy HH:mm"
                };
                if (TryConvertDateTime(value, out var dateTime))
                {
                    datePicker.Value = dateTime;
                }
                datePicker.ValueChanged += (_, _) => _owner.NotifyChanged();
                return datePicker;

            case QueryBuilderFieldType.Number:
                var numeric = new NumericUpDown
                {
                    DecimalPlaces = 2,
                    Minimum = -1000000000,
                    Maximum = 1000000000
                };
                if (TryConvertDecimal(value, out var number))
                {
                    numeric.Value = System.Math.Min(numeric.Maximum, System.Math.Max(numeric.Minimum, number));
                }
                numeric.ValueChanged += (_, _) => _owner.NotifyChanged();
                return numeric;

            default:
                var textBox = new TextBox { Text = ConvertJsonValueToString(value) };
                textBox.TextChanged += (_, _) => _owner.NotifyChanged();
                return textBox;
        }
    }

    private object? ReadCurrentValue()
    {
        return ReadValueFromControl(GetPrimaryValueControl());
    }

    private object? ReadCurrentValue2()
    {
        return ReadValueFromControl(GetSecondaryValueControl());
    }

    private static object? ReadValueFromControl(Control? control)
    {
        if (control is ComboBox comboBox)
            return comboBox.SelectedItem is QueryBuilderLookupItem item ? item.Value : null;
        if (control is DateTimePicker dateTimePicker)
            return dateTimePicker.Value;
        if (control is NumericUpDown numericUpDown)
            return numericUpDown.Value;
        if (control is CheckBox checkBox)
            return checkBox.Checked;
        if (control is TextBox textBox)
            return textBox.Text;
        return null;
    }

    private void ApplyValue(Control control, object? value)
    {
        switch (control)
        {
            case ComboBox comboBox:
                if (comboBox.Items.Count > 0)
                {
                    SelectLookupValue(comboBox, value);
                }
                else
                {
                    comboBox.Text = ConvertJsonValueToString(value);
                }
                break;
            case DateTimePicker dateTimePicker:
                if (TryConvertDateTime(value, out var dateTime))
                {
                    dateTimePicker.Value = dateTime;
                }
                break;
            case NumericUpDown numericUpDown:
                if (TryConvertDecimal(value, out var number))
                {
                    numericUpDown.Value = System.Math.Min(numericUpDown.Maximum, System.Math.Max(numericUpDown.Minimum, number));
                }
                break;
            case CheckBox checkBox:
                checkBox.Checked = value is bool boolValue && boolValue;
                break;
            case TextBox textBox:
                textBox.Text = ConvertJsonValueToString(value);
                break;
        }
    }

    private void UpdateValueEditorVisibility()
    {
        var column = SelectedColumn;
        var isRange = SelectedOperator?.ValueMode == OperatorValueMode.Range;

        if (SelectedOperator is null || !SelectedOperator.RequiresValue)
        {
            valueString.Visible = false;
            valueEnum.Visible = false;
            valueNumeric.Visible = false;
            valueDateTime.Visible = false;
            valueBoolean.Visible = false;
            value2String.Visible = false;
            value2Enum.Visible = false;
            value2Numeric.Visible = false;
            value2DateTime.Visible = false;
            value2Boolean.Visible = false;
            return;
        }

        var hasLookupValues = column?.Values.Count > 0;

        valueString.Visible = false;
        valueEnum.Visible = false;
        valueNumeric.Visible = false;
        valueDateTime.Visible = false;
        valueBoolean.Visible = false;
        value2String.Visible = false;
        value2Enum.Visible = false;
        value2Numeric.Visible = false;
        value2DateTime.Visible = false;
        value2Boolean.Visible = false;

        var primaryControl = GetControlForValueType(ValueType, false, hasLookupValues);
        var secondaryControl = isRange ? GetControlForValueType(ValueType, true, hasLookupValues) : null;

        if (primaryControl is not null)
        {
            primaryControl.Visible = true;
        }

        if (secondaryControl is not null)
        {
            secondaryControl.Visible = true;
        }

        if (valueEnum.Visible && column is not null)
        {
            valueEnum.BeginUpdate();
            valueEnum.Items.Clear();

            foreach (var item in column.Values)
            {
                valueEnum.Items.Add(item);
            }

            if (valueEnum.Items.Count > 0)
            {
                SelectLookupValue(valueEnum, ReadCurrentValue());
            }

            valueEnum.EndUpdate();
        }

        if (value2Enum.Visible && column is not null)
        {
            value2Enum.BeginUpdate();
            value2Enum.Items.Clear();

            foreach (var item in column.Values)
            {
                value2Enum.Items.Add(item);
            }

            if (value2Enum.Items.Count > 0)
            {
                SelectLookupValue(value2Enum, ReadCurrentValue2());
            }

            value2Enum.EndUpdate();
        }
    }

    private Control? GetPrimaryValueControl()
    {
        return valueString.Visible ? valueString
            : valueEnum.Visible ? valueEnum
            : valueNumeric.Visible ? valueNumeric
            : valueDateTime.Visible ? valueDateTime
            : valueBoolean.Visible ? valueBoolean
            : null;
    }

    private Control? GetSecondaryValueControl()
    {
        return value2String.Visible ? value2String
            : value2Enum.Visible ? value2Enum
            : value2Numeric.Visible ? value2Numeric
            : value2DateTime.Visible ? value2DateTime
            : value2Boolean.Visible ? value2Boolean
            : null;
    }

    private Control? GetControlForValueType(QueryBuilderFieldType type, bool secondary, bool hasLookupValues)
    {
        if (secondary)
        {
            if (hasLookupValues || type == QueryBuilderFieldType.Enum) return value2Enum;

            return type switch
            {
                QueryBuilderFieldType.Boolean => value2Boolean,
                QueryBuilderFieldType.Date or QueryBuilderFieldType.DateTime => value2DateTime,
                QueryBuilderFieldType.Number => value2Numeric,
                _ => value2String
            };
        }

        if (hasLookupValues || type == QueryBuilderFieldType.Enum) return valueEnum;

        return type switch
        {
            QueryBuilderFieldType.Boolean => valueBoolean,
            QueryBuilderFieldType.Date or QueryBuilderFieldType.DateTime => valueDateTime,
            QueryBuilderFieldType.Number => valueNumeric,
            _ => valueString
        };
    }

    private static void SelectLookupValue(ComboBox comboBox, object? value)
    {
        if (comboBox.Items.Count == 0)
        {
            return;
        }

        for (var i = 0; i < comboBox.Items.Count; i++)
        {
            if (comboBox.Items[i] is QueryBuilderLookupItem item && StringEqualsValue(item.Value, value))
            {
                comboBox.SelectedIndex = i;
                return;
            }
        }

        comboBox.SelectedIndex = 0;
    }

    private static bool StringEqualsValue(object? left, object? right)
    {
        return string.Equals(
            ConvertJsonValueToString(left),
            ConvertJsonValueToString(right),
            System.StringComparison.OrdinalIgnoreCase);
    }

    private static string ConvertJsonValueToString(object? value)
    {
        if (value is null) return string.Empty;

        if (value is JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString() ?? string.Empty,
                JsonValueKind.Number => element.ToString(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                _ => element.ToString()
            };
        }

        return System.Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
    }

    private static bool TryConvertDecimal(object? value, out decimal result)
    {
        if (value is decimal decimalValue) { result = decimalValue; return true; }
        if (value is JsonElement element && element.TryGetDecimal(out result)) return true;
        return decimal.TryParse(ConvertJsonValueToString(value), NumberStyles.Any, CultureInfo.InvariantCulture, out result);
    }

    private static bool TryConvertDateTime(object? value, out System.DateTime result)
    {
        if (value is System.DateTime dateTime) { result = dateTime; return true; }
        if (value is JsonElement element && element.TryGetDateTime(out result)) return true;
        return System.DateTime.TryParse(ConvertJsonValueToString(value), CultureInfo.CurrentCulture, DateTimeStyles.None, out result);
    }

    private void LayoutEditorControls()
    {
        if (_rightPanel == null)
        {
            return;
        }

        var x = 0;
        var top = 3;
        var spacing = 8;

        _dragHandleButton.Left = x;
        _dragHandleButton.Top = top;
        _dragHandleButton.Height = _fieldComboBox.Height;
        x += _dragHandleButton.Width + spacing;

        _fieldComboBox.Left = x;
        _fieldComboBox.Top = top;
        x += _fieldComboBox.Width + spacing;

        _operatorComboBox.Left = x;
        _operatorComboBox.Top = top;
        x += _operatorComboBox.Width + spacing;

        var valueControl = GetPrimaryValueControl();
        if (valueControl != null)
        {
            valueControl.Left = x;
            valueControl.Top = top;
            x += valueControl.Width + spacing;
        }

        var value2Control = GetSecondaryValueControl();
        if (value2Control != null)
        {
            value2Control.Left = x;
            value2Control.Top = top;
            x += value2Control.Width + spacing;
        }

        _deleteButton.Left = x;
        _deleteButton.Top = 2;
        _deleteButton.Height = _fieldComboBox.Height;

        _rightPanel.Width = _deleteButton.Left + _deleteButton.Width;
    }

    private void DragHandleButton_MouseDown(object? sender, MouseEventArgs e)
    {
        if (_owner is null || e.Button != MouseButtons.Left)
        {
            return;
        }

        _owner.StartDrag(this);
        DoDragDrop(QueryBuilderControl.DragDropToken, DragDropEffects.Move);
    }

    private void RuleEditor_DragEnter(object? sender, DragEventArgs e)
    {
        if (_owner is null)
        {
            return;
        }

        e.Effect = _owner.GetDropEffect(this);
    }

    private void RuleEditor_DragOver(object? sender, DragEventArgs e)
    {
        RuleEditor_DragEnter(sender, e);
    }

    private void RuleEditor_DragDrop(object? sender, DragEventArgs e)
    {
        if (_owner is null)
        {
            return;
        }

        var point = PointToClient(new Point(e.X, e.Y));
        var insertAfter = point.Y >= Height / 2;
        _owner.MoveDraggedEditorRelativeTo(this, insertAfter);
    }

    private void RightPanel_DragDrop(object? sender, DragEventArgs e)
    {
        if (_owner is null)
        {
            return;
        }

        var point = _rightPanel.PointToClient(new Point(e.X, e.Y));
        var insertAfter = point.Y >= _rightPanel.Height / 2;
        _owner.MoveDraggedEditorRelativeTo(this, insertAfter);
    }

    public int GetRequiredWidth()
    {
        var rightEdge = _rightPanel.Left + _rightPanel.Width;
        return System.Math.Max(_fieldComboBox.Width, rightEdge) + 8;
    }
}
