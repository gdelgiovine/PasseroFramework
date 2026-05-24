using System.Collections.Generic;
using System.Drawing;
using Wisej.Web;

namespace Passero.Framework.Controls;

internal sealed partial class GroupEditor : UserControl
{
    private QueryBuilderControl? _owner;
    private readonly bool _isRoot;

    internal bool IsRoot => _isRoot;
    internal FlowLayoutPanel ChildrenPanel => _childrenPanel;

    public GroupEditor()
    {
        InitializeComponent();
        WireEvents();
        SyncHeaderHeights();
        UpdateHeight();
    }

    public GroupEditor(QueryBuilderControl owner, int depth, bool isRoot)
    {
        _owner = owner;
        Depth = depth;
        _isRoot = isRoot;

        InitializeComponent();
        WireEvents();
        ApplyRuntimeState();
        SyncHeaderHeights();
        UpdateHeight();
    }

    public int Depth { get; }

    public void AddRule()
    {
        if (_owner is null)
        {
            return;
        }

        var ruleEditor = new RuleEditor(_owner);
        _childrenPanel.Controls.Add(ruleEditor);
    }

    public void AddGroup()
    {
        if (_owner is null)
        {
            return;
        }

        if (Depth >= _owner.MaxGroupDepth)
        {
            return;
        }

        var groupEditor = new GroupEditor(_owner, Depth + 1, false);
        _childrenPanel.Controls.Add(groupEditor);
    }

    public QueryBuilderRuleSet ToRuleSet()
    {
        return new QueryBuilderRuleSet
        {
            Condition = SelectedCondition,
            Rules = ReadChildren()
        };
    }

    public QueryBuilderRuleNode ToRuleNode()
    {
        return new QueryBuilderRuleNode
        {
            Condition = SelectedCondition,
            Rules = ReadChildren()
        };
    }

    public void LoadFromRuleSet(QueryBuilderRuleSet ruleSet)
    {
        if (_owner is null)
        {
            return;
        }

        SelectedCondition = ruleSet.Condition;
        _childrenPanel.Controls.Clear();

        foreach (var node in ruleSet.Rules)
        {
            LoadNode(node);
        }
    }

    private void LoadNode(QueryBuilderRuleNode node)
    {
        if (_owner is null)
        {
            return;
        }

        if (node.IsGroup)
        {
            var group = new GroupEditor(_owner, Depth + 1, false);
            group.SelectedCondition = node.Condition ?? "and";

            foreach (var child in node.Rules ?? new List<QueryBuilderRuleNode>())
            {
                group.LoadNode(child);
            }

            _childrenPanel.Controls.Add(group);
            return;
        }

        var rule = new RuleEditor(_owner);
        rule.LoadFromNode(node);
        _childrenPanel.Controls.Add(rule);
    }

    private void UpdateHeight()
    {
        if (_headerPanel is null || _childrenPanel is null)
        {
            return;
        }

        Height = _headerPanel.Height + _childrenPanel.Height + Padding.Vertical + 2;
    }

    public int GetRequiredWidth()
    {
        var headerRequiredWidth = _deleteButton.Left + _deleteButton.Width + Padding.Horizontal + 2;
        var childRequiredWidth = 0;

        foreach (Control control in _childrenPanel.Controls)
        {
            var width = control switch
            {
                RuleEditor ruleEditor => ruleEditor.GetRequiredWidth(),
                GroupEditor groupEditor => groupEditor.GetRequiredWidth(),
                _ => control.Width
            };

            if (width > childRequiredWidth)
            {
                childRequiredWidth = width;
            }
        }

        //var childrenRequiredWidth = _childrenPanel.Padding.Left + childRequiredWidth + _childrenPanel.Padding.Right;
        var childrenRequiredWidth = childRequiredWidth-4;
        return System.Math.Max(headerRequiredWidth, childrenRequiredWidth);
    }

    private List<QueryBuilderRuleNode> ReadChildren()
    {
        var rules = new List<QueryBuilderRuleNode>();

        foreach (Control control in _childrenPanel.Controls)
        {
            if (control is RuleEditor ruleEditor)
            {
                var rule = ruleEditor.ToRuleNode();
                if (rule != null)
                {
                    rules.Add(rule);
                }
            }
            else if (control is GroupEditor groupEditor)
            {
                var node = groupEditor.ToRuleNode();
                if (_owner?.AllowEmptyGroups == true || node.Rules?.Count > 0)
                {
                    rules.Add(node);
                }
            }
        }

        return rules;
    }

    private string SelectedCondition
    {
        get
        {
            return _conditionComboBox.SelectedItem is ConditionItem item ? item.Key : "and";
        }
        set
        {
            var normalized = string.Equals(value, "or", System.StringComparison.OrdinalIgnoreCase) ? "or" : "and";
            for (var i = 0; i < _conditionComboBox.Items.Count; i++)
            {
                if (_conditionComboBox.Items[i] is ConditionItem item && item.Key == normalized)
                {
                    _conditionComboBox.SelectedIndex = i;
                    return;
                }
            }

            _conditionComboBox.SelectedIndex = 0;
        }
    }

    // Width calculation moved to QueryBuilderUIHelper to be reused by other editors

    private void WireEvents()
    {
        AllowDrag = true;
        _dragHandleButton.AllowDrag = true;

        _conditionComboBox.SelectedIndexChanged += (_, _) => _owner?.NotifyChanged();
        _addRuleButton.Click += (_, _) =>
        {
            AddRule();
            _owner?.NotifyChanged();
        };
        _addGroupButton.Click += (_, _) =>
        {
            AddGroup();
            _owner?.NotifyChanged();
        };
        _deleteButton.Click += (_, _) =>
        {
            Parent?.Controls.Remove(this);
            Dispose();
            _owner?.NotifyChanged();
        };
        _childrenPanel.SizeChanged += (_, _) => UpdateHeight();
        _dragHandleButton.MouseDown += DragHandleButton_MouseDown;
        RegisterDropTarget(this, GroupEditor_DragDrop);
        RegisterDropTarget(_headerPanel, HeaderPanel_DragDrop);
        RegisterDropTarget(_dragHandleButton, HeaderPanel_DragDrop);
        RegisterDropTarget(_conditionComboBox, HeaderPanel_DragDrop);
        RegisterDropTarget(_addRuleButton, HeaderPanel_DragDrop);
        RegisterDropTarget(_addGroupButton, HeaderPanel_DragDrop);
        RegisterDropTarget(_deleteButton, HeaderPanel_DragDrop);
        RegisterDropTarget(_childrenPanel, ChildrenPanel_DragDrop);
    }

    private void RegisterDropTarget(Control control, DragEventHandler dragDropHandler)
    {
        control.AllowDrop = true;
        control.DragEnter += GroupEditor_DragEnter;
        control.DragOver += GroupEditor_DragOver;
        control.DragDrop += dragDropHandler;
    }

    private void ApplyRuntimeState()
    {
        if (_owner is null)
        {
            return;
        }

        _addGroupButton.Enabled = Depth < _owner.MaxGroupDepth;
        _deleteButton.Visible = !_isRoot;
        _dragHandleButton.Visible = !_isRoot;
        QueryBuilderUIHelper.FitComboToContent(_conditionComboBox);
    }

    private void SyncHeaderHeights()
    {
        var controlHeight = _conditionComboBox.Height;
        _headerPanel.Height = controlHeight + 6;
        _dragHandleButton.Height = controlHeight;
        _addRuleButton.Height = controlHeight;
        _addGroupButton.Height = controlHeight;
        _deleteButton.Height = _conditionComboBox.Height;
    }

    internal bool IsDescendantOf(GroupEditor possibleAncestor)
    {
        Control? current = Parent;

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

    internal int GetInsertIndex(Point location, Control draggedControl)
    {
        var insertIndex = 0;

        foreach (Control child in _childrenPanel.Controls)
        {
            if (ReferenceEquals(child, draggedControl))
            {
                continue;
            }

            var midPoint = child.Top + child.Height / 2;
            if (location.Y < midPoint)
            {
                return insertIndex;
            }

            insertIndex++;
        }

        return insertIndex;
    }

    private void DragHandleButton_MouseDown(object? sender, MouseEventArgs e)
    {
        if (_owner is null || _isRoot || e.Button != MouseButtons.Left)
        {
            return;
        }

        _owner.StartDrag(this);
        DoDragDrop(QueryBuilderControl.DragDropToken, DragDropEffects.Move);
    }

    private void GroupEditor_DragEnter(object? sender, DragEventArgs e)
    {
        if (_owner is null)
        {
            return;
        }

        e.Effect = _owner.GetDropEffect(this);
    }

    private void GroupEditor_DragOver(object? sender, DragEventArgs e)
    {
        GroupEditor_DragEnter(sender, e);
    }

    private void GroupEditor_DragDrop(object? sender, DragEventArgs e)
    {
        if (_owner is null)
        {
            return;
        }

        if (_childrenPanel.Controls.Count == 0)
        {
            _owner.MoveDraggedEditor(this, Point.Empty);
            return;
        }

        var point = PointToClient(new Point(e.X, e.Y));
        var insertAfter = point.Y >= Height / 2;
        _owner.MoveDraggedEditorRelativeTo(this, insertAfter);
    }

    private void HeaderPanel_DragDrop(object? sender, DragEventArgs e)
    {
        if (_owner is null)
        {
            return;
        }

        var point = _headerPanel.PointToClient(new Point(e.X, e.Y));
        if (_childrenPanel.Controls.Count == 0 && point.Y >= _headerPanel.Height / 2)
        {
            _owner.MoveDraggedEditor(this, Point.Empty);
            return;
        }

        var insertAfter = point.Y >= _headerPanel.Height / 2;
        _owner.MoveDraggedEditorRelativeTo(this, insertAfter);
    }

    private void ChildrenPanel_DragDrop(object? sender, DragEventArgs e)
    {
        if (_owner is null)
        {
            return;
        }

        var point = _childrenPanel.PointToClient(new Point(e.X, e.Y));
        _owner.MoveDraggedEditor(this, point);
    }

    private sealed class ConditionItem
    {
        public ConditionItem(string key, string text)
        {
            Key = key;
            Text = text;
        }

        public string Key { get; }
        public string Text { get; }

        public override string ToString()
        {
            return Text;
        }
    }
}
