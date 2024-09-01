
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using Wisej.Web;

namespace Passero.Framework
{
    public static class ControlsUtilities

    {
        public static bool DataColumnIsNumeric(DataColumn col)
        {
            if (col == null)
                return false;
            // Make this const
            var numericTypes = new[] { typeof(Byte), typeof(Decimal), typeof(Double),
            typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
            typeof(Single), typeof(UInt16), typeof(UInt32), typeof(UInt64)};
            
            return numericTypes.Contains(col.DataType);
        }


        public static void TreeViewLoad(ref TreeView treeView, object parentId, TreeNode parentNode, DataTable dataTable, string IDColumnName, string parentIDColumnName, string textColumnName, string keyColumName = "", string sort = "")
        {

            if (dataTable.Rows.Count == 0)
                return;

            TreeNode childNode;
            string query = "";

            if (keyColumName == "")
                keyColumName = IDColumnName;


            if (parentId != DBNull.Value)
            {
                if (DataColumnIsNumeric(dataTable.Columns[parentIDColumnName]) == true)
                {
                    query = parentIDColumnName + "=" + parentId;
                }
                else
                {
                    query = parentIDColumnName + "='" + parentId + "'";
                }
            }
            else
            {
                if (DataColumnIsNumeric(dataTable.Columns[parentIDColumnName]) == true)
                {
                    query = parentIDColumnName + " is null or " + parentIDColumnName + "=0";
                }
                else
                {
                    query = parentIDColumnName + " is null or " + parentIDColumnName + "=''";
                }

            }


            foreach (DataRow dr in dataTable.Select(query, sort))
            {
                TreeNode t = new TreeNode();
                t.Text = dr[keyColumName].ToString() + " - " + dr[textColumnName].ToString();
                t.Name = dr[IDColumnName].ToString();
                t.Tag = dataTable.Rows.IndexOf(dr);
                if (parentNode == null)
                {
                    treeView.Nodes.Add(t);
                    childNode = t;
                }
                else
                {
                    parentNode.Nodes.Add(t);
                    childNode = t;
                }

                TreeViewLoad(ref treeView, Convert.ToInt32(dr[IDColumnName].ToString()), childNode, dataTable, IDColumnName, parentIDColumnName, textColumnName, keyColumName);
            }

        }

        public static TreeNode GetTreeNodeByName(TreeNodeCollection treeNodes, string nodeName)
        {

            TreeNode[] aFindResult = treeNodes.Find(nodeName, true);
            TreeNode tvnRet = null;

            for (int j = 0; j < aFindResult.Length; j++)
            {
                if (aFindResult[j].Name == nodeName)
                {
                    tvnRet = aFindResult[j];
                    break;
                }
            }

            return tvnRet;

        }
        public static void TreeViewComboBoxLoad(ref TreeViewComboBox treeView, object parentId, TreeNode parentNode, DataTable dataTable, string IDColumnName, string parentIDColumnName, string textColumnName, string keyColumName = "", string sort = "")
        {
            TreeNode childNode;
            string query = "";

            if (keyColumName == "")
                keyColumName = IDColumnName;


            if (parentId != DBNull.Value)
            {
                if (DataColumnIsNumeric(dataTable.Columns[parentIDColumnName]) == true)
                {
                    query = parentIDColumnName + "=" + parentId;
                }
                else
                {
                    query = parentIDColumnName + "='" + parentId + "'";
                }
            }
            else
            {
                if (DataColumnIsNumeric(dataTable.Columns[parentIDColumnName]) == true)
                {
                    query = parentIDColumnName + " is null or " + parentIDColumnName + "=0";
                }
                else
                {
                    query = parentIDColumnName + " is null or " + parentIDColumnName + "=''";
                }

            }

            foreach (DataRow dr in dataTable.Select(query, sort))
            {
                TreeNode t = new TreeNode();
                t.Text = dr[keyColumName].ToString() + " - " + dr[textColumnName].ToString();
                t.Name = dr[IDColumnName].ToString();
                t.Tag = dataTable.Rows.IndexOf(dr);
                if (parentNode == null)
                {
                    treeView.Nodes.Add(t);
                    childNode = t;
                }
                else
                {
                    parentNode.Nodes.Add(t);
                    childNode = t;
                }

                TreeViewComboBoxLoad(ref treeView, Convert.ToInt32(dr[IDColumnName].ToString()), childNode, dataTable, IDColumnName, parentIDColumnName, textColumnName, keyColumName);
            }

        }

    




        public static void CenterOnParentControl(Control control)
        {

            control.CenterToParent();
            control.Anchor = AnchorStyles.None; 
        }
        public static  DataGridViewColumn GetFirstVisibleColumnForDataGridView(DataGridView DataGridView)
        {
            foreach (DataGridViewColumn c in DataGridView.Columns)
            {
                if (c.Visible == true)
                {
                    return c;
                }
            }
            return null;
        }

        public static DataGridViewColumn GetFirstVisibleColumnForDataGridViewRow(DataGridViewRow DataGridViewRow)
        {
            foreach (DataGridViewCell c in DataGridViewRow.Cells)
            {
                if (c.Visible == true)
                {
                    return c.OwningColumn;
                }
            }
            return null;
        }

        public static DataGridViewColumn GetFirstEditableColumnForDataGridView(DataGridView DataGridView)
        {
            foreach (DataGridViewColumn c in DataGridView.Columns)
            {
                if (c.ReadOnly == false)
                {
                    return c;
                }
            }
            return null;
        }
        public static DataGridViewColumn GetFirstEditableColumnForDataGridViewRow(DataGridViewRow DataGridViewRow)
        {
            foreach (DataGridViewCell c in DataGridViewRow.Cells)
            {
                if (c.ReadOnly == false)
                {
                    return c.OwningColumn;
                }
            }
            return null;
        }

        public static Control GetDataRepeaterItemControl(DataRepeaterItem DataRepeaterItem, Control TemplateControl)
        {
            if (DataRepeaterItem.Controls.ContainsKey(TemplateControl.Name))
            {
                return DataRepeaterItem.Controls[TemplateControl.Name];
            }
            else
            {
                return null;
            }
        }

        public static Control GetDataRepeaterCurrentItemControl(DataRepeater DataRepeater, Control TemplateControl)
        {
            if (DataRepeater.CurrentItem.Controls.ContainsKey(TemplateControl.Name))
            {
                return DataRepeater.CurrentItem.Controls[TemplateControl.Name];
            }
            else
            {
                return null;
            }
        }
        public static object GetDataRepeaterItemControlProperty(DataRepeaterItem DataRepeaterItem, Control TemplateControl, string PropertyName)
        {
            object Value = null;
            if (DataRepeaterItem.Controls.ContainsKey(TemplateControl.Name))
            {
                Value = ReflectionHelper.GetPropertyValue(DataRepeaterItem.Controls[TemplateControl.Name], PropertyName);
            }
            return Value;
        }

        public static bool SetDataRepeaterItemControlProperty(DataRepeaterItem DataRepeaterItem, Control TemplateControl, string PropertyName, object Value)
        {
            bool esito = false;
            if (DataRepeaterItem.Controls.ContainsKey(TemplateControl.Name))
            {
                ReflectionHelper.CallByName(DataRepeaterItem.Controls[TemplateControl.Name], PropertyName, CallType.Set, Value);
                //ReflectionHelper.SetPropertyValue(ref DataRepeaterItem.Controls[TemplateControl.Name], PropertyName, Value);
                esito = true;
            }
            return esito;
        }
        public static   Control GetDataRepeaterControl(DataRepeater DataRepeater, Control TemplateControl, int Index = -1)
        {
            if (Index == -1)
            {
                if (DataRepeater.CurrentItem.Controls.ContainsKey(TemplateControl.Name))
                {
                    return DataRepeater.CurrentItem.Controls[TemplateControl.Name];
                }
            }
            else
            {
                int _index = DataRepeater.CurrentItemIndex;
                Control _control = null;
                DataRepeater.CurrentItemIndex = Index;
                if (DataRepeater.CurrentItem.Controls.ContainsKey(TemplateControl.Name))
                {
                    _control = DataRepeater.CurrentItem.Controls[TemplateControl.Name];
                }
                DataRepeater.CurrentItemIndex = _index;
                return _control;
            }

            return null;
        }



        public static Dictionary<string, DataBindControl> CreatePasseroBindingFromBindingSource(Form Form, BindingSource BindingSource)
        {
            
            if (Form == null)
                return null;

            
            if (BindingSource == null)
                return null;

            Dictionary <string ,DataBindControl > ctls = new Dictionary<string ,DataBindControl>();

            foreach (Control control in Form.Controls)
            {
                if (control.HasDataBindings)
                {
                    foreach (Binding binding in control.DataBindings)
                    {
                        if (binding.DataSource == BindingSource)
                        {
                            DataBindControl ctl=new DataBindControl ();
                            //this.AddControl(item, binding.PropertyName, binding.BindingMemberInfo.BindingField, this.bindingBehaviour);
                            ctl.Control = control;
                            ctl.ControlPropertyName = binding.PropertyName;
                            ctl.ModelPropertyName = binding.BindingMemberInfo.BindingField;
                            ctl.BindingBehaviour = BindingBehaviour.SelectInsertUpdate;
                            string Key= control.Name + "|" + binding .PropertyName;
                            ctls[Key] = ctl;
                        }
                    }
                }
            }
            if (ctls.Count == 0)
                return null;

            return ctls;

        }


        public static Dictionary<string, Control> BindingSourceControls(Form Form, BindingSource BindingSource = null)
        {
            
            if (Form == null)
                return null;

            if (BindingSource == null)
                return null;

            Dictionary<string, Control> ctls = new Dictionary<string, Control>();
            foreach (Control item in Form.Controls)
            {
                if (item.HasDataBindings)
                {
                    foreach (Binding binding in item.DataBindings)
                    {
                        if (binding.DataSource == BindingSource)
                        {
                            ctls.Add(item.Name + "." + binding.PropertyName, item);
                        }
                    }
                }
            }

            if (ctls.Count == 0)
                return null;

            return ctls;    

        }
        public static int ClearBindingSourceControls(Wisej.Web.Form Form, BindingSource BindingSource, Type T = null )
        {
            int writedproperties = 0;
            
            if (Form != null && BindingSource != null)
            {
                if (T==null)
                    T = Passero .Framework .ReflectionHelper .GetListType ( BindingSource .List  );  

                object Model = Activator .CreateInstance ( T );

                foreach (Control item in Form.Controls)
                {
                    if (item.HasDataBindings)
                    {
                        foreach (Binding binding in item.DataBindings)
                        {
                            if (binding.DataSource == BindingSource)
                            {
                                switch (item.GetType().FullName)
                                {
                                    case "Passero.Framework.Controls.DbLookUpTextBox":
                                        Interaction.CallByName(item, "Value", CallType.Set, null);
                                        //Interaction.CallByName(item, "Text", CallType.Set, "");
                                        break;

                                    default:
                                        //if (binding.PropertyName.Trim().Equals("Text"))
                                        //{
                                        //    Interaction.CallByName(item, binding.PropertyName, CallType.Set, "");
                                        //    break;
                                        //}

                                        //object x = Passero.Framework.ReflectionHelper.GetPropertyValue(item, binding.PropertyName);
                                        //if (Passero.Framework.Utilities.IsNumericType(x.GetType()))
                                        //{
                                        //    Interaction.CallByName(item, binding.PropertyName, CallType.Set, 0);
                                        //}

                                        //if (Passero.Framework.Utilities.IsStringType(x.GetType()))
                                        //{
                                        //    Interaction.CallByName(item, binding.PropertyName, CallType.Set, "");
                                        //}

                                        //if (Passero.Framework.Utilities.IsBooleanType(x.GetType()))
                                        //{
                                        //    Interaction.CallByName(item, binding.PropertyName, CallType.Set, false);
                                        //}
                                        //if (Passero.Framework.Utilities.IsDateTimeType(x.GetType()))
                                        //{
                                        //    Interaction.CallByName(item, binding.PropertyName, CallType.Set, 0);
                                        //}

                                        break;
                                    }   
                                }
                            }
                        }
                    }
                }
            
            return writedproperties;
        }


        public static void ShowForm(Application.FormCollection FormCollection, string FormName)
        {

            var form = FormCollection[FormName];

            if (form is null == false)
            {

                if (form.IsMdiChild == false)
                {
                    if (form.WindowState == FormWindowState.Minimized)
                        form.WindowState = FormWindowState.Normal;
                    form.Activate();
                }
                else
                {
                    form.Activate();
                }




            }
        }

        public static Form GetExistingForm(Application.FormCollection FormCollection, string FormName)
        {

            var form = FormCollection[FormName];
            return form;

        }
        public static Page GetExistingPage(Application.PageCollection PageCollection, string PageName)
        {

            var page = PageCollection[PageName];
            return page;

        }

        public static bool FormExist(Application.FormCollection FormCollection, string FormName, bool ShowIfExist = true)
        {

            var form = FormCollection[FormName];

            if (form is null == false)
            {
                if (form.WindowState == FormWindowState.Minimized)
                    form.WindowState = FormWindowState.Normal;
                if (ShowIfExist)
                {
                    form.Activate();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DataGridRowMoveUp(DataGridView dgv, DataGridViewColumn dgvc)
        {
            if (dgv.RowCount <= 0)
                return;

            if (dgv.SelectedRows.Count <= 0)
                return;

            // var index = dgv.SelectedCells[0].OwningRow.Index;
            int index = dgv.SelectedRows[0].Index;

            if (index == 0)
                return;

            var rows = dgv.Rows;
            var prevRow = rows[index - 1];
            rows.Remove(prevRow);
            prevRow.Frozen = false;
            rows.Insert(index, prevRow);
            dgv.ClearSelection();
            dgv.Rows[index - 1].Selected = true;
            // dgv.Rows[index - 1][dgvc].Value = index;
            for (int i = 0, loopTo = dgv.Rows.Count - 1; i <= loopTo; i++)
                dgv.Rows[i][dgvc].Value = i + 1;
        }

        public static void DataGridRowMoveDown(DataGridView dgv, DataGridViewColumn dgvc)
        {
            if (dgv.RowCount <= 0)
                return;

            if (dgv.SelectedRows.Count <= 0)
                return;

            int rowCount = dgv.Rows.Count;
            int index = dgv.SelectedRows[0].Index;

            if (index == rowCount - 1)
                return; // Here used 1 instead of 2

            var rows = dgv.Rows;
            var nextRow = rows[index + 1];
            rows.Remove(nextRow);
            nextRow.Frozen = false;
            rows.Insert(index, nextRow);
            dgv.ClearSelection();
            dgv.Rows[index + 1].Selected = true;
            for (int i = 0, loopTo = dgv.Rows.Count - 1; i <= loopTo; i++)
                dgv.Rows[i][dgvc].Value = i + 1;
        }


        public static Model GetComboBoxSelectedItemData<Model>(ComboBox ComboBox) where Model : class
        {

            if (ComboBox is null)
            {
                return null;
            }

            if (ComboBox.SelectedItem is null)
            {
                return null;
            }

            return (Model)ComboBox.SelectedItem;


        }
        public static object GetComboBoxSelectedItemDataProperty<Model>(ComboBox ComboBox, string PropertyName, object DefaultValue = null) where Model : class
        {


            if (ComboBox is null)
            {
                return DefaultValue;
            }

            if (ComboBox.SelectedItem is null)
            {
                return DefaultValue;
            }
            if (string.IsNullOrEmpty(PropertyName.Trim()))
            {
                return DefaultValue;
            }

            var value = Interaction.CallByName(ComboBox.SelectedItem, PropertyName, CallType.Get);
            if (value is null)
            {
                value = DefaultValue;
            }

            return value;

        }

        public static void CenterControlToParent(Control Control)
        {

            if (Control is null)
            {
                return;
            }

            if (Control.Parent is null)
            {
                return;
            }

            Control.Anchor = AnchorStyles.None;
            Control.Location = new System.Drawing.Point((Control.Parent.ClientSize.Width - Control.Width) / 2, (Control.Parent.ClientSize.Height - Control.Height) / 2);

        }


        public static void CenterControlToForm(Control Control, Form Form)
        {

            if (Control is null)
            {
                return;
            }

            if (Form is null)
            {
                return;
            }


            Control.Location = new System.Drawing.Point((Form.ClientSize.Width - Control.Width) / 2, (Form.ClientSize.Height - Control.Height) / 2);


        }
        public static void FitControlToParent(Control Control, Margins Margins = null)
        {

            if (Control is null)
            {
                return;
            }

            if (Control.Parent is null)
            {
                return;
            }

            if (Margins == null)
            {
                Margins = new Margins(0, 0, 0, 0);
            }
            int HeaderSize = 0;
            if (Utilities.ObjectPropertyExist(Control.Parent, "ShowHeader"))
            {
                if (Utilities.ObjectPropertyExist(Control.Parent, "HeaderSize"))
                {
                    HeaderSize = Conversions.ToInteger(Interaction.CallByName(Control.Parent, "HeaderSize", CallType.Get, null));
                }
            }

            Control.Anchor = default;
            Control.Dock = DockStyle.None;

            Control.Top = Margins.Top + HeaderSize;
            Control.Left = Margins.Left;
            Control.Width = Control.Parent.ClientSize.Width - Margins.Right - Margins.Left;
            Control.Height = Control.Parent.ClientSize.Height - HeaderSize - Margins.Top - Margins.Bottom;

        }
        public static void SetControlsLocation(Control Control, Control ReferenceControl, int MinHeight, int MaxHeight, int MinWidth, int MaxWidth, int Offset, Control Parent)
        {

            int W = 0;
            int L = 0;
            int H = 0;
            int T = 0;
            if (ReferenceControl is not null)
            {
                H = ReferenceControl.Height;
                T = ReferenceControl.Top;
            }

            if (Parent is not null)
            {
                W = Parent.Width;
                if (ReferenceControl.Left + ReferenceControl.Width + Control.Width < W)
                {
                    L = ReferenceControl.Left + ReferenceControl.Width;
                }
                else
                {
                    T = T + H + Offset;
                }
            }
            else
            {
                T = T + H + Offset;
            }

            Control.Left = L;
            Control.Top = T;


        }


        public static void InvokeOnResize(Control Control)
        {
            typeof(Control).GetMethod("OnResize", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Control, new object[] { EventArgs.Empty });
        }

        public static void InvokeOnResize(Page Page)
        {
            typeof(Page).GetMethod("OnResize", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Page, new object[] { EventArgs.Empty });
        }

        public static void InvokeOnResize(Form Form)
        {
            typeof(Form).GetMethod("OnResize", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Form, new object[] { EventArgs.Empty });
        }

        public static void InvokeOnResize(Desktop Desktop)
        {
            typeof(Desktop).GetMethod("OnResize", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Desktop, new object[] { EventArgs.Empty });
        }

        public static int GetTableRowsHeight(TableLayoutPanel Table)
        {

            int Height = 0;
            int[] RowHeights = Table.GetRowHeights();
            for (int i = 0, loopTo = Table.RowCount; i <= loopTo; i++)
                Height = Height + RowHeights[i];

            return Height;

        }

        public static void SetPanelPosition(ref FlowLayoutPanel Panel, int MinWidth, int MaxWidth, Control ReferenceControl, Control HeightReferenceControl = null, int LocationOffset = 5, int HeightOffset = 5)
        {


            int W = 0;
            int L = 0;
            int H = 0;
            int T = 0;
            if (ReferenceControl is not null)
            {
                H = ReferenceControl.Height;
                T = ReferenceControl.Top;
            }

            if (MaxWidth > 0)
            {
                if (Panel.Width > MaxWidth)
                {
                    Panel.Width = MaxWidth;
                }
            }

            if (MinWidth > 0)
            {
                if (Panel.Width < MinWidth)
                {
                    Panel.Width = MinWidth;
                }
            }


            if (Panel.Parent is not null)
            {
                W = Panel.Parent.Width;
                if (ReferenceControl is not null)
                {
                    if (ReferenceControl.Left + ReferenceControl.Width + Panel.Width < W)
                    {
                        L = ReferenceControl.Left + ReferenceControl.Width + LocationOffset;
                    }
                    else
                    {
                        T = T + H + LocationOffset;
                    }
                }
                else
                {
                    T = T + H + LocationOffset;
                }
            }
            // If ((ReferenceControl.Left + ReferenceControl.Width) + Panel.Width) < W Then
            // L = ReferenceControl.Left + ReferenceControl.Width + LocationOffset
            // Else
            // T = T + H + LocationOffset
            // End If
            else
            {
                T = T + H + LocationOffset;
            }

            Panel.Left = L;
            Panel.Top = T;
            if (HeightReferenceControl is not null)
            {
                Panel.Height = HeightReferenceControl.Top + HeightReferenceControl.Height + Panel.HeaderSize + HeightOffset;
            }


        }

        public static void SetTablePosition(ref TableLayoutPanel Table, int MinWidth, int MaxWidth, Control ReferenceControl, Control HeightReferenceControl = null, int LocationOffset = 5, int HeightOffset = 5)
        {

            int W = 0;
            int L = 0;
            int H = 0;
            int T = 0;
            if (ReferenceControl is not null)
            {
                H = ReferenceControl.Height;
                T = ReferenceControl.Top;
            }

            if (MaxWidth > 0)
            {
                if (Table.Width > MaxWidth)
                {
                    Table.Width = MaxWidth;
                }
            }

            if (MinWidth > 0)
            {
                if (Table.Width < MinWidth)
                {
                    Table.Width = MinWidth;
                }
            }


            if (Table.Parent is not null)
            {
                W = Table.Parent.Width;
                if (ReferenceControl is not null)
                {
                    if (ReferenceControl.Left + ReferenceControl.Width + Table.Width < W)
                    {
                        L = ReferenceControl.Left + ReferenceControl.Width + LocationOffset;
                    }
                    else
                    {
                        T = T + H + LocationOffset;
                    }
                }
                else
                {
                    T = T + H + LocationOffset;
                }
            }
            else
            {
                T = T + H + LocationOffset;
            }

            Table.Left = L;
            Table.Top = T;
            if (HeightReferenceControl is not null)
            {
                Table.Height = HeightReferenceControl.Top + HeightReferenceControl.Height + Table.HeaderSize + HeightOffset;
            }


        }


        public static void SetPanelHeight(ref FlowLayoutPanel Panel, Control ReferenceControl, int Offset = 5)
        {
            Panel.Height = ReferenceControl.Top + ReferenceControl.Height + Panel.HeaderSize + Offset;
        }



    }

}
