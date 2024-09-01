using Dapper;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using Wisej.Web;
namespace Passero.Framework.Controls
{

    public partial class QBEForm<ModelClass> : Wisej.Web.Form where ModelClass : class
    {
        public int TopRows { get; set; } = 500;
        public string RecordLabelSeparator { get; set; } = "of";
        public string RecordLabelHtmlFormat { get; set; } = "<p style='margin-top:2px;line-height:1.0;text-align:center;'>{0}<br>{1}<br>{2}</p>";
        private Dictionary<string, System.Reflection.PropertyInfo> ModelProperties;
        private Repository<ModelClass> mRepository = new Repository<ModelClass>();
        public DynamicParameters SQLQueryParameters = new DynamicParameters();
        public string SQLQuery = "";
        public string OrderBy = "";
        public string BaseSQLQuery { get; set; } = "";
        public DynamicParameters  BaseDbParameteres { get; set; }= new DynamicParameters();

        public ModelClass QBEModel
        {
            get { return this.Repository.ModelItem; }
            set { this.Repository.ModelItem = value; }
        }

        public List<ModelClass> QBEModelItems
        {
            get { return this.Repository.ModelItems; }
            set { this.Repository.ModelItems = value; }
        }

        public string SQLQueryResolved()
        {
            return Passero.Framework.DapperHelper.Utilities.ResolveSQL(this.SQLQuery, this.SQLQueryParameters);
        }
        private Repository<ModelClass> Repository
        {
            get
            {

                return mRepository;
            }
            set
            {

                mRepository = value;
                var ModelPropertiesInfo = mRepository.GetProperties();
                this.ModelProperties = new Dictionary<string, System.Reflection.PropertyInfo>();
                foreach (var item in ModelPropertiesInfo)
                {
                    this.ModelProperties.Add(item.Name, item);
                }
            }
        }

        public QBEBoundControls QBEBoundControls { get; set; } = new QBEBoundControls();
        public QBEResultMode QBEResultMode { get; set; }
        public IDbConnection DbConnection { get; set; }
        public QBEColumns QBEColumns = new QBEColumns();



        public bool UseLikeOperator
        {
            get { return this.chkLikeOperator.Checked; }
            set { this.chkLikeOperator.Checked = value; }
        }

        public ModelPropertiesMapping QBEModelPropertiesMapping { get; set; } = new ModelPropertiesMapping();
        public object TargetRepository;
        public Action CallBackAction { get; set; }
        public Action ResultGridModelItemsCallBackAction { get; set; }
        public bool AutoLoadData { get; set; } = true;

        public TargetModelItems<ModelClass> ResultGridModelItems = null;

        public Wisej.Web.Control SetFocusControlAfterClose { get; set; }

        public QBEForm()
        {
            InitializeComponent();
            this.TabPageDebug.Hidden = true;
            this.TabPageExport.Hidden = true;
            this.TabPageReportQuery.Hidden = true;
            this.RecordLabel.Text = this.RecordLabelHtmlFormat;
            this.bSaveQBE.Visible = false;
            this.bLoadQBE.Visible = false;
            this.bPrint.Visible = false;

        }

        public QBEForm(IDbConnection DbConnection)
        {
            this.Repository = new Repository<ModelClass>();
            this.Repository.DbConnection = DbConnection;
            InitializeComponent();
            this.bSaveQBE.Visible = false;
            this.bLoadQBE.Visible = false;
            this.bPrint.Visible = false;
        }

        public void SetTargetRepository<T>(T targetRepository, Action CallBackAction = null) where T : class
        {
            TargetRepository = targetRepository;
            if (CallBackAction != null)
            {
                this.CallBackAction = CallBackAction;
            }
            else
            {

            }
        }



        public void ExportResultGrid()
        {

            string filename = "";
            filename = System.IO.Path.GetTempPath() + @"\" + System.Guid.NewGuid().ToString();
            string exportfilename = Passero.Framework.FileHelper.GetSafeFileName(this.Text);
            string expofilenameextension = "";
            System.IO.FileStream Stream = null;

            if (rbExcel.Checked)
            {
                MiniExcelLibs.MiniExcel.SaveAs(filename, this.Repository.ModelItems, true, "Sheet1", MiniExcelLibs.ExcelType.XLSX);
                expofilenameextension = ".xlsx";
            }

            if (rbCSV.Checked)
            {
                MiniExcelLibs.MiniExcel.SaveAs(filename, this.Repository.ModelItems, true, "Sheet1", MiniExcelLibs.ExcelType.CSV);
                expofilenameextension = ".csv";
            }

            if (rbJSON.Checked)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(this.Repository.ModelItems, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(filename, json);
                expofilenameextension = ".json";
            }
            if (rbXML.Checked)
            {
                DataTable dt;
                dt = Passero.Framework.DataBaseHelper.ListToDataTable(this.Repository.ModelItems);
                dt.TableName = "Row";
                dt.WriteXml(filename, XmlWriteMode.WriteSchema, true);
                dt.Dispose();
                expofilenameextension = ".xml";
            }

            if (expofilenameextension != "")
            {
                Stream = new FileStream(Convert.ToString(filename), FileMode.Open);
                Application.Download(Stream, exportfilename + expofilenameextension);
                Stream.Close();
            }
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
        }


        private ModelClass GetEmptyModel()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }


        public void ShowQBEWait()
        {
            this.ShowQBE(true);
        }
        public void ShowQBE(bool Wait = false)
        {
            this.SetupQBEForm();

            if (this.AutoLoadData)
                this.LoadData();

            if (Wait == true)
            {
                this.MdiParent = null;
                this.ShowDialog();
            }
            else
            {
                this.Show();
            }
        }


        public void SetupQBEForm(bool OverrideResultGridDefinition = false)
        {

            this.TabPageReportQuery.Hidden = false;
            this.TabPageExport.Hidden = false;
            this.TabPageDebug.Hidden = true;
            this.PanelExport.Visible = true;

            this.CheckQBEColumns();
            this.SetupQueryGrid();
            this.SetupResultGrid(OverrideResultGridDefinition);

            this.ResultGrid.Dock = DockStyle.Fill;
            //this.ResultGrid.Visible = true;
            this.QueryGrid.Visible = true;

            if (this.Owner == null && this.SetFocusControlAfterClose != null)
                this.Owner = Passero.Framework.Utilities.GetParentOfType<Form>(this.SetFocusControlAfterClose);

            if (this.Owner != null && this.Owner.MdiParent != null)
                this.MdiParent = this.Owner.MdiParent;
        }

        public void CheckQBEColumns()
        {
            if (this.QBEColumns.Count == 0)
            {

                foreach (var item in this.ModelProperties.Values)
                {
                    QBEColumn column = new QBEColumn();
                    column.DbColumn = item.Name;
                    column.FriendlyName = item.Name;
                    column.UseInQBE = true;
                    this.QBEColumns.Add(column.DbColumn, column);
                }

            }
        }


        public void SetupQueryGrid()
        {

            this.QueryGrid.Rows.Clear();

            if (QBEColumns.Count == 0)
            {

                foreach (var item in ModelProperties.Values)
                {
                    QBEColumn column = new QBEColumn();
                    column.DbColumn = item.Name;
                    column.FriendlyName = item.Name;
                    column.UseInQBE = true;
                    column.DisplayInQBEResult = true;
                    QBEColumns.Add(column.DbColumn, column);
                }

            }


            foreach (var QBEColumn in this.QBEColumns.Values)
            {
                if (QBEColumn.UseInQBE)
                {
                    int i;
                    i = this.QueryGrid.Rows.Add(QBEColumn.FriendlyName, QBEColumn.QBEValue);
                    if (Passero.Framework.Utilities.GetSystemTypeIs(this.ModelProperties[QBEColumn.DbColumn].PropertyType) == EnumSystemTypeIs.Boolean)
                    {
                        DataGridViewCheckBoxCell ncell = new DataGridViewCheckBoxCell();
                        ncell.ThreeState = true;
                        ncell.IndeterminateValue = "";
                        ncell.TrueValue = true;
                        ncell.FalseValue = false;
                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(QBEColumn.QBEValue, null, false)))
                        {
                            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(QBEColumn.QBEValue, "True", false)))
                            {
                                ncell.Value = true;
                            }
                            else
                            {
                                ncell.Value = false;
                            }
                        }
                        else
                        {
                            ncell.Value = ncell.IndeterminateValue;
                        }
                        this.QueryGrid.Rows[i].Cells[1] = ncell;
                        this.QueryGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                    this.QueryGrid.Rows[i].Tag = QBEColumn.DbColumn;

                    if (QBEColumn.QBEValue != null)
                    {
                        if (!string.IsNullOrEmpty(QBEColumn.QBEValue.ToString().Trim()))
                        {
                            QueryGrid.Rows[i].Cells[1].ReadOnly = true;
                        }
                    }
                    if (QBEColumn.DisplayInQBE == false)
                    {
                        QueryGrid.Rows[i].Visible = false;
                    }
                    


                }
            }
        }


        public void SetupResultGrid(bool OverrideColumns = false)
        {


            switch (this.QBEResultMode)
            {
                case QBEResultMode.BoundControls:
                    this.ResultGrid.MultiSelect = false;    
                    break;
                case QBEResultMode.AllRowsSQLQuery:
                    break;
                case QBEResultMode.SingleRowSQLQuery:
                    this.ResultGrid.MultiSelect = false;
                    break;
                case QBEResultMode.MultipleRowsSQLQuery:
                    break;
                case QBEResultMode.MultipleRowsItems:
                    break;
                case QBEResultMode.SingleRowItem:
                    this.ResultGrid.MultiSelect = false;
                    break;
                case QBEResultMode.AllRowsItems:
                    break;
                default:
                    break;
            }
            
            QBEColumns ResultColumns = new QBEColumns();
            foreach (var QBEColumn in this.QBEColumns.Values)
            {
                //if (QBEColumn.DisplayInQBEResult)
                {
                    ResultColumns.Add(QBEColumn.DbColumn, QBEColumn);
                }
            }


            if (ResultColumns.Count > 0)
            {
                if (OverrideColumns == true)
                {
                    this.ResultGrid.Columns.Clear();
                }
                this.ResultGrid.AutoGenerateColumns = false;

                foreach (QBEColumn column in ResultColumns.Values)
                {
                    DataGridViewColumn nc;
                    bool newcolumn = false;
                    if (this.ResultGrid.Columns[column.DbColumn] == null)
                    {
                        nc = new DataGridViewColumn();
                        newcolumn = true;
                    }
                    else
                    {
                        nc = this.ResultGrid.Columns[column.DbColumn];
                    }

                    switch (column.QBEColumnType)
                    {
                        case QBEColumnsTypes.CheckBox:
                            DataGridViewCheckBoxColumn nccheck = new DataGridViewCheckBoxColumn();
                            nccheck.ThreeState = true;
                            nccheck.IndeterminateValue = "";
                            nccheck.TrueValue = true;
                            nccheck.FalseValue = false;
                            nc = (DataGridViewColumn)nccheck;
                            break;
                        case QBEColumnsTypes.ComboBox:

                            break;
                        case QBEColumnsTypes.Image:

                            break;
                        case QBEColumnsTypes.Link:

                            break;
                        case QBEColumnsTypes.TextBox:

                            break;
                        default:
                            break;
                    }


                    if (newcolumn == true)
                    {
                        nc.Name = column.DbColumn;
                        nc.HeaderText = column.FriendlyName;
                        nc.DataPropertyName = nc.Name;
                        nc.Visible = column.DisplayInQBEResult;

                        nc.DefaultCellStyle.ForeColor = column.ForeColor;
                        nc.DefaultCellStyle.BackColor = column.BackColor;
                        nc.DefaultCellStyle.Format = column.DisplayFormat;
                        nc.DefaultCellStyle.Alignment = column.Aligment;

                        if (column.Font != null)
                        {
                            nc.DefaultCellStyle.Font = column.Font;
                        }
                        else
                        {
                            nc.DefaultCellStyle.Font = this.ResultGrid.DefaultCellStyle.Font;
                            if (nc.DefaultCellStyle.Font == null)
                                nc.DefaultCellStyle.Font = this.ResultGrid.Font;
                        }
                        nc.DefaultCellStyle.Font = nc.DefaultCellStyle.Font.Change(column.FontStyle);
                        if (column.FontSize > 0)
                        {
                            nc.DefaultCellStyle.Font = nc.DefaultCellStyle.Font.Change(column.FontSize);
                        }


                        switch (column.ColumnSize)
                        {
                            case > 0:

                                nc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                nc.Width = Wisej.Base.TextUtils.MeasureText("0", nc.DefaultCellStyle.Font).Width * column.ColumnSize;
                                break;
                            case 0:
                                nc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                nc.Width = Wisej.Base.TextUtils.MeasureText("0", nc.DefaultCellStyle.Font).Width * 30;
                                break;
                            case -1:
                                nc.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.AllCells;
                                break;
                            case -2:
                                nc.AutoSizeMode = (DataGridViewAutoSizeColumnMode)DataGridViewAutoSizeColumnsMode.Fill;
                                break;
                            default:
                                break;
                        }
                    }

                    if (newcolumn)
                    {
                        this.ResultGrid.Columns.Add(nc);
                    }

                }
            }
            else
            {

            }

        }


        public void LoadData()
        {
            this.DoQuery();
        }

        public void DoQuery()
        {
            BuildQuery3();
            this.ResultGrid.DataSource = this.mRepository.GetItems(this.SQLQuery, this.SQLQueryParameters).Value;
            this.ResultGrid.Visible = true;

        }

        private void MovePrevious()
        {

            ResultGrid.Focus();
            if (ResultGrid.CurrentRow.ClientIndex > 0)
            {
                int CurrentRowClientIndex = ResultGrid.CurrentRow.ClientIndex - 1;
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, CurrentRowClientIndex];
            }



            // SendKeys.Send("{UP}")

        }
        private void MoveNext()
        {


            ResultGrid.Focus();
            if (ResultGrid.CurrentRow.ClientIndex < ResultGrid.Rows.Count - 1)
            {
                int CurrentRowClientIndex = ResultGrid.CurrentRow.ClientIndex + 1;
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, CurrentRowClientIndex];
            }


        }
        private void MoveFirst()
        {

            ResultGrid.Focus();
            if (Conversions.ToBoolean(ResultGrid.Rows.Count))
            {
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, 0];
            }


        }
        private void MoveLast()
        {

            ResultGrid.Focus();
            if (Conversions.ToBoolean(ResultGrid.Rows.Count))
            {
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, ResultGrid.Rows.Count - 1];
            }


        }


        private void BuildQuery3()
        {

            StringBuilder sqlwhere = new StringBuilder();
            string _WhereAND = "";
            this.OrderBy = this.OrderBy.Trim();
            this.QueryGrid.EndEdit();
            DynamicParameters parameters = new DynamicParameters();


            foreach (var item in this.QueryGrid.Rows)
            {
                StringBuilder sqlwhereitem = new StringBuilder();
                string Value = "";
                if (item[1].Value != null)
                    Value = item[1].Value.ToString();

                string _WhereItemOR = "";
                string[] Values;
                Type PropertyType = this.ModelProperties[item.Tag.ToString()].PropertyType;
                Passero.Framework.EnumSystemTypeIs PropertyTypeIs = Passero.Framework.Utilities.GetSystemTypeIs(PropertyType);
                if (!string.IsNullOrEmpty(Strings.Trim(Value)) | !string.IsNullOrEmpty(Value))
                {
                    Value = Value.Trim();


                    if (Value != ";")
                    {
                        Values = Strings.Split(Value, ";");
                    }
                    else
                    {
                        Values = new string[1];
                        Values[0] = ";";
                    }

                    int i = 1;
                    foreach (var _Value in Values)
                    {
                        string parametername = $"@{item.Tag.ToString()}_{i.ToString().Trim()}";
                        if (this.chkLikeOperator.Checked)
                        {

                            // controlla il tipo di dato della colonna
                            if (Passero.Framework .Utilities .IsNumericType(this.ModelProperties[item.Tag.ToString()].GetMethod .ReturnType)) 
                            {
                                sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()}{GetComparisionOperator(_Value)}{parametername}");
                                parameters.Add(parametername, RemoveComparisionOperator(_Value), Passero.Framework.Utilities.GetDbType(PropertyType));
                            }
                            else
                            {
                                sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()} Like {parametername} ");
                                parameters.Add(parametername, "%" + _Value + "%", Passero.Framework.Utilities.GetDbType(PropertyType));
                            }

                        }
                        else
                        {
                            sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()}{GetComparisionOperator(_Value)}{parametername}");
                            parameters.Add(parametername, RemoveComparisionOperator(_Value), Passero.Framework.Utilities.GetDbType(PropertyType));
                        }

                        if (sqlwhereitem.Length > 0)
                        {
                            _WhereItemOR = " OR ";
                        }
                        i++;
                    }
                    if (sqlwhere.Length > 0)
                    {
                        _WhereAND = " AND ";
                    }
                    sqlwhere.Append($" {_WhereAND} ( {sqlwhereitem.ToString()} )");

                }


            }

            string sTopRows = "";

            if (this.TopRows > 0)
            {
                sTopRows = $"TOP ({this.TopRows})";
            }

            if (string .IsNullOrEmpty(this.BaseSQLQuery) ==true)
            {
                this.SQLQuery = $"SELECT {sTopRows} * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName<ModelClass>()}";
            }
            else
            {
                this.SQLQuery = $"SELECT {sTopRows} * FROM ({this.BaseSQLQuery.Trim()}) _b ";
            }
            
            if (sqlwhere.ToString().Trim() != "")
                this.SQLQuery = this.SQLQuery + $" WHERE {sqlwhere.ToString()}";
            if (string.IsNullOrEmpty(this.OrderBy)==false)
                this.SQLQuery = this.SQLQuery + $" ORDER BY {this.OrderBy}";
            this.SQLQueryParameters = parameters;

            foreach (var p in this.BaseDbParameteres.ParameterNames  )
            {
                this.SQLQueryParameters.Add(p, ((SqlMapper.IParameterLookup)BaseDbParameteres)[p]);
            }
            
            string rSQL=Framework.DapperHelper .Utilities .ResolveSQL (this.SQLQuery , this.SQLQueryParameters);    
        }


        private string GetComparisionOperator(string Value)
        {

            Value = Value.ToUpper();
            string _operator = "";
            if (Value.StartsWith("="))
                _operator = " = ";
            if (Value.StartsWith(">"))
                _operator = " > ";
            if (Value.StartsWith("<"))
                _operator = " < ";
            if (Value.StartsWith(">="))
                _operator = " >= ";
            if (Value.StartsWith("<="))
                _operator = " <= ";
            if (Value.StartsWith("<>"))
                _operator = " <> ";
            if (Value.StartsWith("LIKE ", StringComparison.CurrentCultureIgnoreCase))
                _operator = " LIKE ";
            if (Value.StartsWith("NOT LIKE ", StringComparison.CurrentCultureIgnoreCase))
                _operator = " NOT LIKE ";
            if (_operator == "")
                _operator = " = ";

            return _operator;
        }
        private string RemoveComparisionOperator(string Value)
        {
            string op = GetComparisionOperator(Value).Trim();

            if (Value.StartsWith(op, StringComparison.CurrentCultureIgnoreCase) == false)
                Value = op + Value;
            return Value.Substring(op.Length).Trim();
        }

        private void XQBEForm_Load(object sender, EventArgs e)
        {

        }

        private void bPrev_Click(object sender, EventArgs e)
        {
            this.MovePrevious();
        }

        private void bFirst_Click(object sender, EventArgs e)
        {
            this.MoveFirst();
        }

        private void bNext_Click(object sender, EventArgs e)
        {
            this.MoveNext();
        }

        private void bLast_Click(object sender, EventArgs e)
        {
            this.MoveLast();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            this.DoQuery();
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        public void ClearFilters()
        {
            foreach (DataGridViewRow row in QueryGrid.Rows)
            {

                QBEColumn column = QBEColumns[row.Tag.ToString ()];
                if (column != null)
                {
                    if (column.DisplayInQBE == true)
                    {
                        if (string.IsNullOrEmpty(column.QBEInitialValue.ToString()))
                        {
                            row[1].Value = "";
                        }
                        else
                        {
                            row[1].Value = column.QBEInitialValue.ToString();
                          
                        }
                    }
                }

            }

        }
        private void bSave_Click(object sender, EventArgs e)
        {
            SaveQueryResult();
        }

        public void SaveQueryResult()
        {
            switch (this.QBEResultMode)
            {
                case QBEResultMode.BoundControls:
                    QBEResultMode_BoundControls();
                    break;
                case QBEResultMode.AllRowsSQLQuery:
                    break;
                case QBEResultMode.SingleRowSQLQuery:
                    QBEResultMode_SelectedRows();
                    break;
                case QBEResultMode.MultipleRowsSQLQuery:
                    QBEResultMode_SelectedRows();
                    break;
                case QBEResultMode.AllRowsItems:
                    QBEResultMode_AllRowsItems();
                    break;
                case QBEResultMode.SingleRowItem:
                    break;
                case QBEResultMode.MultipleRowsItems:
                    QBEResultMode_MultipleRowsItems();
                    break;
                default:
                    break;
            }
        }


        private void CloseQBEForm(bool IgnoreCallBack = false)
        {
            if (this.Owner == null && this.SetFocusControlAfterClose != null)
                this.Owner = Passero.Framework.Utilities.GetParentOfType<Form>(this.SetFocusControlAfterClose);


            if (IgnoreCallBack == false)
            {
                if (this.Owner != null && this.CallBackAction != null)
                {
                    try
                    {
                        this.CallBackAction.Invoke();
                    }
                    catch (Exception)
                    {

                    }
                }


                if (this.Owner != null && this.ResultGridModelItemsCallBackAction != null)
                {
                    try
                    {
                        this.ResultGridModelItemsCallBackAction.Invoke();
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (this.SetFocusControlAfterClose != null && this.SetFocusControlAfterClose.Focusable)
            {
                this.SetFocusControlAfterClose.Focus();
            }
            this.Close();
            this.Dispose();
        }
        private void QBEResultMode_AllRowsItems()
        {
            {
                try
                {
                    ResultGridModelItems = new TargetModelItems<ModelClass>();
                    this.ResultGridModelItems.Items = this.mRepository.ModelItems;
                    //Passero.Framework.ReflectionHelper.SetPropertyValue(ref TargetModelItems, "Items",this.mRepository .ModelItems );
                }
                catch (Exception)
                {


                }

            }


            this.CloseQBEForm();
        }


        private void QBEResultMode_MultipleRowsItems()
        {
            {
                try
                {

                    ResultGridModelItems = new TargetModelItems<ModelClass>();

                    foreach (DataGridViewRow row in this.ResultGrid.SelectedRows)
                    {
                        ResultGridModelItems.Items.Add((ModelClass)row.DataBoundItem);
                    }

                    Passero.Framework.ReflectionHelper.SetPropertyValue(ref ResultGridModelItems, "Items", this.ResultGridModelItems.Items);
                }
                catch (Exception)
                {


                }

            }


            this.CloseQBEForm();

            //Microsoft.Reporting.WebForms.ReportViewer x = new Microsoft.Reporting.WebForms.ReportViewer();
            //x.BackColor = System.Drawing.Color.White;   

        }


        private void QBEResultMode_BoundControls()
        {
            if (this.ResultGrid.CurrentRow == null)
                return;

            ModelClass currentrowmodel = (ModelClass)this.ResultGrid.CurrentRow.DataBoundItem;
            foreach (QBEBoundControl item in this.QBEBoundControls)
            {
                var Value = Interaction.CallByName(currentrowmodel, item.ModelPropertyName, CallType.Get, (object[])null);
                if (Value is not null)
                {
                    Interaction.CallByName(item.Control, item.ControlPropertyName, CallType.Set, Value);
                }
                else
                {
                    Interaction.CallByName(item.Control, item.ControlPropertyName, CallType.Set, "");
                }
            }
            this.CloseQBEForm();
        }


        private void QBEResultMode_SelectedRows()
        {
            if (ResultGrid.SelectedRows.Count == 0)
                return;
            if (this.QBEModelPropertiesMapping.Count == 0)
                return;

            object TargetModel = Passero.Framework.ReflectionHelper.GetPropertyValue(TargetRepository, "ModelItem");
            if (TargetModel is null)
            {
                TargetModel = GetEmptyModel();
            }
            Type TargetModelType = TargetModel.GetType();
            DynamicParameters parameters = new DynamicParameters();

            Dictionary<string, PropertyInfo> TargetModelProperties = new Dictionary<string, PropertyInfo>();
            foreach (var item in TargetModelType.GetProperties())
            {
                TargetModelProperties.Add(item.Name, item);
            }


            string propertyname = "";
            object propertyvalue;
            string _AND = "";
            string _OR = "";
            int i = 1;
            StringBuilder sqlwhere = new StringBuilder();
            string sqlquery = "";
            foreach (DataGridViewRow row in ResultGrid.SelectedRows)
            {
                StringBuilder sqlwhererow = new StringBuilder();
                _AND = "";
                foreach (ModelPropertyMapping Mapping in this.QBEModelPropertiesMapping)
                {
                    propertyvalue = row[Mapping.QBEModelProperty].Value;
                    propertyname = Mapping.TargetModelProperty;
                    Type Type = TargetModelProperties[propertyname].PropertyType;
                    DbType DbType = Passero.Framework.Utilities.GetDbType(Type);
                    string parametername = $"@{propertyname}_{i}";
                    parameters.Add(parametername, propertyvalue, DbType);
                    sqlwhererow.Append($"{_AND} {propertyname} = {parametername} ");
                    _AND = " AND ";
                    i++;
                }
                sqlwhere.Append($" {_OR} ( {sqlwhererow.ToString()} )");

                _OR = " OR ";
            }

            sqlquery = $"SELECT * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName(TargetModel)}";
            if (sqlwhere.ToString() != "")
                sqlquery = sqlquery + " WHERE " + sqlwhere.ToString();

            if (this.TargetRepository != null)
            {
                try
                {
                    Passero.Framework.ReflectionHelper.InvokeMethodByName(ref TargetRepository, "SetSQLQuery", sqlquery, parameters);
                }
                catch (Exception)
                {


                }

            }

            this.CloseQBEForm();
        }



        private void CopyDataRow(DataRow oSourceRow, DataRow oTargetRow)
        {
            int nIndex = 0;
            // - Copy all the fields from the source row to the target row
            foreach (var oItem in oSourceRow.ItemArray)
            {
                oTargetRow[nIndex] = oItem;
                nIndex += 1;
            }
        }

        private void XQBEForm_Shown(object sender, EventArgs e)
        {
            this.SetupQBEForm();
            this.ResultGrid.Dock = DockStyle.Fill;
            //this.ResultGrid.Visible = true;
            //if (this.AutoLoadData)
            //    this.LoadData();
            this.Show();
            this.Focus();

        }

        private void XQBEForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            GC.Collect();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.ExportResultGrid();
        }

        private void ResultGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


            if (this.ResultGrid.CurrentRow == null)
                return;


            switch (this.QBEResultMode)
            {
                case QBEResultMode.BoundControls:
                    QBEResultMode_BoundControls();
                    break;
                case QBEResultMode.AllRowsSQLQuery:
                    break;
                case QBEResultMode.SingleRowSQLQuery:
                    QBEResultMode_SelectedRows();
                    break;
                case QBEResultMode.MultipleRowsSQLQuery:
                    QBEResultMode_SelectedRows();
                    break;
                case QBEResultMode.AllRowsItems:
                    break;
                case QBEResultMode.SingleRowItem:
                    QBEResultMode_SelectedRows();
                    break;
                case QBEResultMode.MultipleRowsItems:
                    break;
                default:
                    break;
            }

        }

        private void ContextMenuRecords_MenuItemClicked(object sender, MenuItemEventArgs e)
        {
            this.TopRows = (int)e.MenuItem.Tag;
            this.Records.Text = e.MenuItem.Text;
        }


        private void ResultGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.ResultGrid.CurrentCell == null)
                return;
            try
            {
                this.RecordLabel.Text = String.Format(this.RecordLabelHtmlFormat, this.ResultGrid.CurrentCell.RowIndex + 1, this.RecordLabelSeparator, this.ResultGrid.Rows.Count);
            }
            catch (Exception)
            {
            }

        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.CloseQBEForm(true);

        }



        private void ResultGrid_Appear(object sender, EventArgs e)
        {
            //this.ResultGrid.Visible = true;
        }


        private void ManageToolsClick(ComponentTool Tool)
        {
            if (Tool.Name == "selectrows")
            {
                if (Tool.Pushed == false)
                {
                    this.ResultGrid.SelectAllRows();
                    Tool.Pushed = true;
                }

                else
                {
                    this.ResultGrid.ClearSelection();
                    Tool.Pushed = false;
                }
            }

            if (Tool.Name == "movefirst")
            {
                MoveFirst();
            }
            if (Tool.Name == "movelast")
            {
                MoveLast();
            }
            if (Tool.Name == "moveprevious")
            {
                MovePrevious();
            }
            if (Tool.Name == "movenext")
            {
                MoveNext();
            }
            if (Tool.Name == "autosize")
            {
                this.ResultGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                foreach (DataGridViewColumn column in this.ResultGrid.Columns)
                {
                    if (column.Visible)
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }
        private void SplitContainer_Panel1_ToolClick(object sender, ToolClickEventArgs e)
        {
            ManageToolsClick(e.Tool);
         }

        private void QueryGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex ==1)
            {
                this.QueryGrid.CurrentCell.ReadOnly = false;
            }
        }
    }



}
