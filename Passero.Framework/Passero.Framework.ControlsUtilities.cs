
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using Wisej.Web;

namespace Passero.Framework
{

    /// <summary>
    /// Classe per contenere informazioni sui ColSpan delle celle
    /// </summary>
    public class DataGridViewCellSpanInfo
    {
        public int StartColumnIndex { get; set; }
        public int ColSpan { get; set; }
        public int EndColumnIndex { get; set; }
        public List<int> CoveredColumns { get; set; } = new List<int>();
    }


    /// <summary>
    /// 
    /// </summary>
    public static class ControlsUtilities

    {

        /// <summary>
        /// Verifica se una cella specifica è coperta da un'altra cella con ColSpan > 1
        /// </summary>
        /// <param name="cell">La cella da verificare</param>
        /// <returns>True se la cella è coperta, False altrimenti</returns>
        public static bool DataGridViewIsCellCoveredByColSpan(DataGridViewCell cell)
        {
            if (cell == null || cell.OwningRow == null || cell.OwningColumn == null)
                return false;

            DataGridViewRow row = cell.OwningRow;
            int columnIndex = cell.ColumnIndex;

            // Verifica se l'indice della colonna è valido
            if (columnIndex < 0 || columnIndex >= row.Cells.Count)
                return false;

            // Verifica le celle precedenti nella stessa riga
            for (int i = 0; i < columnIndex; i++)
            {
                DataGridViewCell previousCell = row.Cells[i];
                int colSpan = previousCell.Style.ColSpan;

                // Se la cella precedente ha un ColSpan che si estende fino alla cella corrente
                if (colSpan > 1 && (i + colSpan) > columnIndex)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifica se una cella specifica è coperta da un'altra cella con ColSpan > 1
        /// </summary>
        /// <param name="row">La riga da verificare</param>
        /// <param name="columnName">Il nome della colonna da verificare</param>
        /// <returns>True se la cella è coperta, False altrimenti</returns>
        public static bool DataGridViewIsCellCoveredByColSpan(DataGridViewRow row, string columnName)
        {
            if (row == null || string.IsNullOrEmpty(columnName))
                return false;

            // Trova l'indice della colonna dal nome
            int columnIndex = -1;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (row.Cells[i].OwningColumn.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    columnIndex = i;
                    break;
                }
            }

            // Se la colonna non è stata trovata, restituisci false
            if (columnIndex < 0 || columnIndex >= row.Cells.Count)
                return false;

            // Verifica le celle precedenti nella stessa riga
            for (int i = 0; i < columnIndex; i++)
            {
                DataGridViewCell cell = row.Cells[i];
                int colSpan = cell.Style.ColSpan;

                // Se la cella precedente ha un ColSpan che si estende fino alla cella corrente
                if (colSpan > 1 && (i + colSpan) > columnIndex)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifica se una cella specifica è coperta da un'altra cella con ColSpan > 1
        /// </summary>
        /// <param name="row">La riga da verificare</param>
        /// <param name="columnIndex">L'indice della colonna da verificare</param>
        /// <returns>True se la cella è coperta, False altrimenti</returns>
        public static bool DataGridViewIsCellCoveredByColSpan(DataGridViewRow row, int columnIndex)
        {
            if (row == null || columnIndex < 0 || columnIndex >= row.Cells.Count)
                return false;

            // Verifica le celle precedenti nella stessa riga
            for (int i = 0; i < columnIndex; i++)
            {
                DataGridViewCell cell = row.Cells[i];
                int colSpan = cell.Style.ColSpan;

                // Se la cella precedente ha un ColSpan che si estende fino alla cella corrente
                if (colSpan > 1 && (i + colSpan) > columnIndex)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Ottiene una lista di tutte le celle coperte da ColSpan in una riga
        /// </summary>
        /// <param name="row">La riga da analizzare</param>
        /// <returns>Lista degli indici delle colonne coperte</returns>
        public static List<int> DataGridViewGetCoveredCellsInRow(DataGridViewRow row)
        {
            List<int> coveredCells = new List<int>();

            if (row == null)
                return coveredCells;

            for (int i = 0; i < row.Cells.Count; i++)
            {
                DataGridViewCell cell = row.Cells[i];
                int colSpan = cell.Style.ColSpan;

                // Se la cella ha ColSpan > 1, aggiungi tutte le celle coperte
                if (colSpan > 1)
                {
                    for (int j = i + 1; j < i + colSpan && j < row.Cells.Count; j++)
                    {
                        coveredCells.Add(j);
                    }
                }
            }

            return coveredCells;
        }

        /// <summary>
        /// Verifica se ci sono celle coperte in una riga e restituisce informazioni dettagliate
        /// </summary>
        /// <param name="row">La riga da verificare</param>
        /// <returns>Dizionario con informazioni sulle celle coperte</returns>
        public static Dictionary<int, DataGridViewCellSpanInfo> GetRowCellSpanInfo(DataGridViewRow row)
        {
            Dictionary<int, DataGridViewCellSpanInfo> spanInfo = new Dictionary<int, DataGridViewCellSpanInfo>();

            if (row == null)
                return spanInfo;

            for (int i = 0; i < row.Cells.Count; i++)
            {
                DataGridViewCell cell = row.Cells[i];
                int colSpan = cell.Style.ColSpan;

                if (colSpan > 1)
                {
                    var info = new DataGridViewCellSpanInfo
                    {
                        StartColumnIndex = i,
                        ColSpan = colSpan,
                        EndColumnIndex = i + colSpan - 1,
                        CoveredColumns = new List<int>()
                    };

                    // Aggiungi gli indici delle colonne coperte
                    for (int j = i + 1; j < i + colSpan && j < row.Cells.Count; j++)
                    {
                        info.CoveredColumns.Add(j);
                    }

                    spanInfo[i] = info;
                }
            }

            return spanInfo;
        }

        /// <summary>
        /// Trova la cella "master" che copre una specifica colonna
        /// </summary>
        /// <param name="row">La riga da verificare</param>
        /// <param name="columnIndex">L'indice della colonna</param>
        /// <returns>L'indice della cella master o -1 se non trovata</returns>
        public static int DataGridViewFindMasterCellForColumn(DataGridViewRow row, int columnIndex)
        {
            if (row == null || columnIndex < 0 || columnIndex >= row.Cells.Count)
                return -1;

            for (int i = 0; i <= columnIndex; i++)
            {
                DataGridViewCell cell = row.Cells[i];
                int colSpan = cell.Style.ColSpan;

                if (colSpan > 1 && (i + colSpan) > columnIndex)
                {
                    return i; // Restituisce l'indice della cella master
                }
            }

            return -1; // Nessuna cella master trovata
        }





        public enum ControlCenteringMode
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2,
            Both = 3
        }
        public static void CenterControlInParent(Control control, ControlCenteringMode centeringMode = ControlCenteringMode.Both)
        {
            if (control?.Parent == null) return;

            int x = control.Location.X;
            int y = control.Location.Y;

            bool centerHorizontally = (centeringMode & ControlCenteringMode.Horizontal) == ControlCenteringMode.Horizontal;
            bool centerVertically = (centeringMode & ControlCenteringMode.Vertical) == ControlCenteringMode.Vertical;

            if (centerHorizontally)
            {
                x = control.Parent.ClientSize.Width / 2 - control.Size.Width / 2;
            }

            if (centerVertically)
            {
                y = control.Parent.ClientSize.Height / 2 - control.Size.Height / 2;
            }

            control.Location = new Point(x, y);

            // Imposta l'anchor appropriato in base alla centratura
            AnchorStyles anchor = AnchorStyles.None;
            if (centeringMode == ControlCenteringMode.None)
            {
                anchor = AnchorStyles.Top | AnchorStyles.Left;
            }
            else if (!centerHorizontally)
            {
                anchor = AnchorStyles.Left;
            }
            else if (!centerVertically)
            {
                anchor = AnchorStyles.Top;
            }

            control.Anchor = anchor;
        }
        public static int GetDataGridViewHeight(DataGridView dataGridView)
        {
            // Se non ci sono righe, esci
            if (dataGridView.Rows.Count == 0)
                return dataGridView .Height;

            // Calcolo dell’altezza totale:
            // 1) Aggiungo l’altezza dell’intestazione colonne
            int totalHeight = dataGridView.ColumnHeadersHeight;

            // 2) Altezza di una riga (in genere si può usare dataGridView1.RowTemplate.Height
            //    oppure, se ci sono già righe, la prima riga).
            int rowHeight = dataGridView.Rows[0].Height;

            // 3) Moltiplico l’altezza per il numero di righe
            totalHeight += rowHeight * dataGridView.Rows.Count;

            // 4) Imposto la Height della DataGridView
            return totalHeight+20;
            
        }
        public static int GetTextBoxHeight(TextBox textBox)
        {
            if (textBox.Multiline)
            {
                int lineHeight = textBox.Font.Height;
                int numberOfLines = textBox.Lines.Length + 1;
                int newHeight = lineHeight * numberOfLines + textBox.Margin.Vertical;
                return newHeight;
            }
            else
            {
                return textBox.Height;  
            }
        }
        public static Control GetChildControl(Control ParentControl, Control ChildControlToFind)
        {
            if (ParentControl is null | ChildControlToFind is null)
            {
                return null;
            }

            if (ParentControl.Controls.ContainsKey(ChildControlToFind.Name))
            {
                return ParentControl.Controls[ChildControlToFind.Name];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Datas the column is numeric.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <returns></returns>
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


        /// <summary>
        /// TreeViews the load.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="dataTable">The data table.</param>
        /// <param name="IDColumnName">Name of the identifier column.</param>
        /// <param name="parentIDColumnName">Name of the parent identifier column.</param>
        /// <param name="textColumnName">Name of the text column.</param>
        /// <param name="keyColumName">Name of the key colum.</param>
        /// <param name="sort">The sort.</param>
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

        /// <summary>
        /// Gets the name of the tree node by.
        /// </summary>
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
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
        /// <summary>
        /// TreeViews the ComboBox load.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="dataTable">The data table.</param>
        /// <param name="IDColumnName">Name of the identifier column.</param>
        /// <param name="parentIDColumnName">Name of the parent identifier column.</param>
        /// <param name="textColumnName">Name of the text column.</param>
        /// <param name="keyColumName">Name of the key colum.</param>
        /// <param name="sort">The sort.</param>
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






        /// <summary>
        /// Centers the on parent control.
        /// </summary>
        /// <param name="control">The control.</param>
        public static void CenterOnParentControl(Control control)
        {

            control.CenterToParent();
            control.Anchor = AnchorStyles.None; 
        }
        /// <summary>
        /// Gets the first visible column for data grid view.
        /// </summary>
        /// <param name="DataGridView">The data grid view.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the first visible column for data grid view row.
        /// </summary>
        /// <param name="DataGridViewRow">The data grid view row.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the first editable column for data grid view.
        /// </summary>
        /// <param name="DataGridView">The data grid view.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the first editable column for data grid view row.
        /// </summary>
        /// <param name="DataGridViewRow">The data grid view row.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the data repeater item control.
        /// </summary>
        /// <param name="DataRepeaterItem">The data repeater item.</param>
        /// <param name="TemplateControl">The template control.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the data repeater current item control.
        /// </summary>
        /// <param name="DataRepeater">The data repeater.</param>
        /// <param name="TemplateControl">The template control.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the data repeater item control property.
        /// </summary>
        /// <param name="DataRepeaterItem">The data repeater item.</param>
        /// <param name="TemplateControl">The template control.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        public static object GetDataRepeaterItemControlProperty(DataRepeaterItem DataRepeaterItem, Control TemplateControl, string PropertyName)
        {
            object Value = null;
            if (DataRepeaterItem.Controls.ContainsKey(TemplateControl.Name))
            {
                Value = ReflectionHelper.GetPropertyValue(DataRepeaterItem.Controls[TemplateControl.Name], PropertyName);
            }
            return Value;
        }

        /// <summary>
        /// Sets the data repeater item control property.
        /// </summary>
        /// <param name="DataRepeaterItem">The data repeater item.</param>
        /// <param name="TemplateControl">The template control.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <param name="Value">The value.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the data repeater control.
        /// </summary>
        /// <param name="DataRepeater">The data repeater.</param>
        /// <param name="TemplateControl">The template control.</param>
        /// <param name="Index">The index.</param>
        /// <returns></returns>
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



        /// <summary>
        /// Creates the passero binding from binding source.
        /// </summary>
        /// <param name="Form">The form.</param>
        /// <param name="BindingSource">The binding source.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Bindings the source controls.
        /// </summary>
        /// <param name="Form">The form.</param>
        /// <param name="BindingSource">The binding source.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Clears the binding source controls.
        /// </summary>
        /// <param name="Form">The form.</param>
        /// <param name="BindingSource">The binding source.</param>
        /// <param name="T">The t.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Shows the form.
        /// </summary>
        /// <param name="FormCollection">The form collection.</param>
        /// <param name="FormName">Name of the form.</param>
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

        /// <summary>
        /// Gets the existing form.
        /// </summary>
        /// <param name="FormCollection">The form collection.</param>
        /// <param name="FormName">Name of the form.</param>
        /// <returns></returns>
        public static Form GetExistingForm(Application.FormCollection FormCollection, string FormName)
        {

            var form = FormCollection[FormName];
            return form;

        }
        /// <summary>
        /// Gets the existing page.
        /// </summary>
        /// <param name="PageCollection">The page collection.</param>
        /// <param name="PageName">Name of the page.</param>
        /// <returns></returns>
        public static Page GetExistingPage(Application.PageCollection PageCollection, string PageName)
        {

            var page = PageCollection[PageName];
            return page;

        }

        /// <summary>
        /// Restituisce la prima istanza aperta di una form MDIChild del tipo specificato, oppure null se non esiste.
        /// </summary>
        /// <typeparam name="T">Il tipo di form da cercare</typeparam>
        /// <returns>La prima istanza trovata del tipo specificato, oppure null</returns>
        public static T FindOpenForm<T>(bool CreateIfNotExist = false) where T : Form
        {
            foreach (Form f in Wisej.Web.Application.OpenForms)
            {
                if (f is T)
                {
                    return (T)f;
                }
            }
            if (CreateIfNotExist)
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
            
            return null;
        }

        /// <summary>
        /// Forms the exist.
        /// </summary>
        /// <param name="FormCollection">The form collection.</param>
        /// <param name="FormName">Name of the form.</param>
        /// <param name="ShowIfExist">if set to <c>true</c> [show if exist].</param>
        /// <returns></returns>
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

        /// <summary>
        /// Datas the grid row move up.
        /// </summary>
        /// <param name="dgv">The DGV.</param>
        /// <param name="dgvc">The DGVC.</param>
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

        /// <summary>
        /// Datas the grid row move down.
        /// </summary>
        /// <param name="dgv">The DGV.</param>
        /// <param name="dgvc">The DGVC.</param>
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


        /// <summary>
        /// Gets the ComboBox selected item data.
        /// </summary>
        /// <typeparam name="Model">The type of the odel.</typeparam>
        /// <param name="ComboBox">The ComboBox.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the ComboBox selected item data property.
        /// </summary>
        /// <typeparam name="Model">The type of the odel.</typeparam>
        /// <param name="ComboBox">The ComboBox.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <param name="DefaultValue">The default value.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Centers the control to parent.
        /// </summary>
        /// <param name="Control">The control.</param>
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


        /// <summary>
        /// Centers the control to form.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="Form">The form.</param>
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
        /// <summary>
        /// Fits the control to parent.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="Margins">The margins.</param>
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
        /// <summary>
        /// Sets the controls location.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ReferenceControl">The reference control.</param>
        /// <param name="MinHeight">The minimum height.</param>
        /// <param name="MaxHeight">The maximum height.</param>
        /// <param name="MinWidth">The minimum width.</param>
        /// <param name="MaxWidth">The maximum width.</param>
        /// <param name="Offset">The offset.</param>
        /// <param name="Parent">The parent.</param>
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


        /// <summary>
        /// Invokes the on resize.
        /// </summary>
        /// <param name="Control">The control.</param>
        public static void InvokeOnResize(Control Control)
        {
            typeof(Control).GetMethod("OnResize", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Control, new object[] { EventArgs.Empty });
        }

        /// <summary>
        /// Invokes the on resize.
        /// </summary>
        /// <param name="Page">The page.</param>
        public static void InvokeOnResize(Page Page)
        {
            typeof(Page).GetMethod("OnResize", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Page, new object[] { EventArgs.Empty });
        }

        /// <summary>
        /// Invokes the on resize.
        /// </summary>
        /// <param name="Form">The form.</param>
        public static void InvokeOnResize(Form Form)
        {
            typeof(Form).GetMethod("OnResize", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Form, new object[] { EventArgs.Empty });
        }

        /// <summary>
        /// Invokes the on resize.
        /// </summary>
        /// <param name="Desktop">The desktop.</param>
        public static void InvokeOnResize(Desktop Desktop)
        {
            typeof(Desktop).GetMethod("OnResize", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(Desktop, new object[] { EventArgs.Empty });
        }

        /// <summary>
        /// Gets the height of the table rows.
        /// </summary>
        /// <param name="Table">The table.</param>
        /// <returns></returns>
        public static int GetTableRowsHeight(TableLayoutPanel Table)
        {

            int Height = 0;
            int[] RowHeights = Table.GetRowHeights();
            for (int i = 0, loopTo = Table.RowCount; i <= loopTo; i++)
                Height = Height + RowHeights[i];

            return Height;

        }

        /// <summary>
        /// Sets the panel position.
        /// </summary>
        /// <param name="Panel">The panel.</param>
        /// <param name="MinWidth">The minimum width.</param>
        /// <param name="MaxWidth">The maximum width.</param>
        /// <param name="ReferenceControl">The reference control.</param>
        /// <param name="HeightReferenceControl">The height reference control.</param>
        /// <param name="LocationOffset">The location offset.</param>
        /// <param name="HeightOffset">The height offset.</param>
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

        /// <summary>
        /// Sets the table position.
        /// </summary>
        /// <param name="Table">The table.</param>
        /// <param name="MinWidth">The minimum width.</param>
        /// <param name="MaxWidth">The maximum width.</param>
        /// <param name="ReferenceControl">The reference control.</param>
        /// <param name="HeightReferenceControl">The height reference control.</param>
        /// <param name="LocationOffset">The location offset.</param>
        /// <param name="HeightOffset">The height offset.</param>
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


        /// <summary>
        /// Sets the height of the panel.
        /// </summary>
        /// <param name="Panel">The panel.</param>
        /// <param name="ReferenceControl">The reference control.</param>
        /// <param name="Offset">The offset.</param>
        public static void SetPanelHeight(ref FlowLayoutPanel Panel, Control ReferenceControl, int Offset = 5)
        {
            Panel.Height = ReferenceControl.Top + ReferenceControl.Height + Panel.HeaderSize + Offset;
        }



    }

}
