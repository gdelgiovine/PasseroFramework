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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ModelClass">The type of the odel class.</typeparam>
    /// <seealso cref="Wisej.Web.Form" />
    public partial class QBEForm<ModelClass> : Wisej.Web.Form where ModelClass : class
    {
        /// <summary>
        /// Gets or sets the top rows.
        /// </summary>
        /// <value>
        /// The top rows.
        /// </value>
        public int TopRows { get; set; } = 500;
        /// <summary>
        /// Gets or sets the record label separator.
        /// </summary>
        /// <value>
        /// The record label separator.
        /// </value>
        public string RecordLabelSeparator { get; set; } = "of";
        /// <summary>
        /// Gets or sets the record label HTML format.
        /// </summary>
        /// <value>
        /// The record label HTML format.
        /// </value>
        public string RecordLabelHtmlFormat { get; set; } = "<p style='margin-top:2px;line-height:1.0;text-align:center;'>{0}<br>{1}<br>{2}</p>";
        /// <summary>
        /// The model properties
        /// </summary>
        private Dictionary<string, System.Reflection.PropertyInfo> ModelProperties;
        /// <summary>
        /// The m repository
        /// </summary>
        private Repository<ModelClass> mRepository = new Repository<ModelClass>();
        /// <summary>
        /// The SQL query parameters
        /// </summary>
        public DynamicParameters SQLQueryParameters = new DynamicParameters();
        /// <summary>
        /// The SQL query
        /// </summary>
        public string SQLQuery = "";
        /// <summary>
        /// The order by
        /// </summary>
        public string OrderBy = "";
        /// <summary>
        /// Gets or sets the base SQL query.
        /// </summary>
        /// <value>
        /// The base SQL query.
        /// </value>
        public string BaseSQLQuery { get; set; } = "";
        /// <summary>
        /// Gets or sets the base database parameteres.
        /// </summary>
        /// <value>
        /// The base database parameteres.
        /// </value>
        public DynamicParameters BaseDbParameteres { get; set; } = new DynamicParameters();

        /// <summary>
        /// Gets or sets the qbe model.
        /// </summary>
        /// <value>
        /// The qbe model.
        /// </value>
        public ModelClass QBEModel
        {
            get { return Repository.ModelItem; }
            set { Repository.ModelItem = value; }
        }

        /// <summary>
        /// Gets or sets the qbe model items.
        /// </summary>
        /// <value>
        /// The qbe model items.
        /// </value>
        public IList<ModelClass> QBEModelItems
        {
            get { return Repository.ModelItems; }
            set { Repository.ModelItems = value; }
        }

        /// <summary>
        /// SQLs the query resolved.
        /// </summary>
        /// <returns></returns>
        public string SQLQueryResolved()
        {
            return Passero.Framework.DapperHelper.Utilities.ResolveSQL(SQLQuery, SQLQueryParameters);
        }
        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
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
                ModelProperties = new Dictionary<string, System.Reflection.PropertyInfo>();
                foreach (var item in ModelPropertiesInfo)
                {
                    ModelProperties.Add(item.Name, item);
                }
            }
        }

        /// <summary>
        /// Gets or sets the qbe bound controls.
        /// </summary>
        /// <value>
        /// The qbe bound controls.
        /// </value>
        public QBEBoundControls QBEBoundControls { get; set; } = new QBEBoundControls();
        /// <summary>
        /// Gets or sets the qbe result mode.
        /// </summary>
        /// <value>
        /// The qbe result mode.
        /// </value>
        public QBEResultMode QBEResultMode { get; set; }
        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>
        public IDbConnection DbConnection { get; set; }
        /// <summary>
        /// The qbe columns
        /// </summary>
        public QBEColumns QBEColumns = new QBEColumns();



        /// <summary>
        /// Gets or sets a value indicating whether [use like operator].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use like operator]; otherwise, <c>false</c>.
        /// </value>
        public bool UseLikeOperator
        {
            get { return chkLikeOperator.Checked; }
            set { chkLikeOperator.Checked = value; }
        }

        /// <summary>
        /// Gets or sets the qbe model properties mapping.
        /// </summary>
        /// <value>
        /// The qbe model properties mapping.
        /// </value>
        public ModelPropertiesMapping QBEModelPropertiesMapping { get; set; } = new ModelPropertiesMapping();
        /// <summary>
        /// The target repository
        /// </summary>
        public object TargetRepository;
        /// <summary>
        /// Gets or sets the call back action.
        /// </summary>
        /// <value>
        /// The call back action.
        /// </value>
        public Action CallBackAction { get; set; }
        /// <summary>
        /// Gets or sets the result grid model items call back action.
        /// </summary>
        /// <value>
        /// The result grid model items call back action.
        /// </value>
        public Action ResultGridModelItemsCallBackAction { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [automatic load data].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic load data]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoLoadData { get; set; } = true;

        /// <summary>
        /// The result grid model items
        /// </summary>
        public TargetModelItems<ModelClass> ResultGridModelItems = null;

        /// <summary>
        /// Gets or sets the set focus control after close.
        /// </summary>
        /// <value>
        /// The set focus control after close.
        /// </value>
        public Wisej.Web.Control SetFocusControlAfterClose { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QBEForm{ModelClass}"/> class.
        /// </summary>
        public QBEForm()
        {
            InitializeComponent();
            TabPageDebug.Hidden = true;
            TabPageExport.Hidden = true;
            TabPageReportQuery.Hidden = true;
            RecordLabel.Text = RecordLabelHtmlFormat;
            bSaveQBE.Visible = false;
            bLoadQBE.Visible = false;
            bPrint.Visible = false;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QBEForm{ModelClass}"/> class.
        /// </summary>
        /// <param name="DbConnection">The database connection.</param>
        public QBEForm(IDbConnection DbConnection)
        {
            Repository = new Repository<ModelClass>();
            Repository.DbConnection = DbConnection;
            InitializeComponent();
            bSaveQBE.Visible = false;
            bLoadQBE.Visible = false;
            bPrint.Visible = false;
        }

        /// <summary>
        /// Sets the target repository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetRepository">The target repository.</param>
        /// <param name="CallBackAction">The call back action.</param>
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



        /// <summary>
        /// Exports the result grid.
        /// </summary>
        public void ExportResultGrid()
        {

            string filename = "";
            filename = System.IO.Path.GetTempPath() + @"\" + System.Guid.NewGuid().ToString();
            string exportfilename = Passero.Framework.FileHelper.GetSafeFileName(Text);
            string expofilenameextension = "";
            System.IO.FileStream Stream = null;

            if (rbExcel.Checked)
            {
                MiniExcelLibs.MiniExcel.SaveAs(filename, Repository.ModelItems, true, "Sheet1", MiniExcelLibs.ExcelType.XLSX);
                expofilenameextension = ".xlsx";
            }

            if (rbCSV.Checked)
            {
                MiniExcelLibs.MiniExcel.SaveAs(filename, Repository.ModelItems, true, "Sheet1", MiniExcelLibs.ExcelType.CSV);
                expofilenameextension = ".csv";
            }

            if (rbJSON.Checked)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(Repository.ModelItems, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(filename, json);
                expofilenameextension = ".json";
            }
            if (rbXML.Checked)
            {
                DataTable dt;
                dt = Passero.Framework.DataBaseHelper.ListToDataTable(Repository.ModelItems);
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


        /// <summary>
        /// Gets the empty model.
        /// </summary>
        /// <returns></returns>
        private ModelClass GetEmptyModel()
        {
            return (ModelClass)Activator.CreateInstance(typeof(ModelClass));
        }


        /// <summary>
        /// Shows the qbe wait.
        /// </summary>
        public void ShowQBEWait()
        {
            ShowQBE(true);
        }
        /// <summary>
        /// Shows the qbe.
        /// </summary>
        /// <param name="Wait">if set to <c>true</c> [wait].</param>
        public void ShowQBE(bool Wait = false)
        {
            SetupQBEForm();

            if (AutoLoadData)
                LoadData();

            if (Wait == true)
            {
                MdiParent = null;
                ShowDialog();
            }
            else
            {
                Show();
            }
        }


        /// <summary>
        /// Setups the qbe form.
        /// </summary>
        /// <param name="OverrideResultGridDefinition">if set to <c>true</c> [override result grid definition].</param>
        public void SetupQBEForm(bool OverrideResultGridDefinition = false)
        {

            TabPageReportQuery.Hidden = false;
            TabPageExport.Hidden = false;
            TabPageDebug.Hidden = true;
            PanelExport.Visible = true;

            CheckQBEColumns();
            SetupQueryGrid();
            SetupResultGrid(OverrideResultGridDefinition);

            ResultGrid.Dock = DockStyle.Fill;
            //this.ResultGrid.Visible = true;
            QueryGrid.Visible = true;

            if (Owner == null && SetFocusControlAfterClose != null)
                Owner = Passero.Framework.Utilities.GetParentOfType<Form>(SetFocusControlAfterClose);

            if (Owner != null && Owner.MdiParent != null)
                MdiParent = Owner.MdiParent;
        }

        /// <summary>
        /// Checks the qbe columns.
        /// </summary>
        public void CheckQBEColumns()
        {
            if (QBEColumns.Count == 0)
            {

                foreach (var item in ModelProperties.Values)
                {
                    QBEColumn column = new QBEColumn();
                    column.DbColumn = item.Name;
                    column.FriendlyName = item.Name;
                    column.UseInQBE = true;
                    QBEColumns.Add(column.DbColumn, column);
                }

            }
        }


        /// <summary>
        /// Setups the query grid.
        /// </summary>
        public void SetupQueryGrid()
        {

            QueryGrid.Rows.Clear();

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


            foreach (var QBEColumn in QBEColumns.Values)
            {
                if (QBEColumn.UseInQBE)
                {
                    int i;
                    i = QueryGrid.Rows.Add(QBEColumn.FriendlyName, QBEColumn.QBEValue);
                    if (Passero.Framework.Utilities.GetSystemTypeIs(ModelProperties[QBEColumn.DbColumn].PropertyType) == EnumSystemTypeIs.Boolean)
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
                        QueryGrid.Rows[i].Cells[1] = ncell;
                        QueryGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                    QueryGrid.Rows[i].Tag = QBEColumn.DbColumn;

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


        /// <summary>
        /// Setups the result grid.
        /// </summary>
        /// <param name="OverrideColumns">if set to <c>true</c> [override columns].</param>
        public void SetupResultGrid(bool OverrideColumns = false)
        {

            ResultGrid.MultiSelect = true;
            switch (QBEResultMode)
            {
                case QBEResultMode.BoundControls:
                    ResultGrid.MultiSelect = false;
                    break;
                case QBEResultMode.AllRowsSQLQuery:
                    break;
                case QBEResultMode.SingleRowSQLQuery:
                    ResultGrid.MultiSelect = false;
                    break;
                case QBEResultMode.MultipleRowsSQLQuery:
                    break;
                case QBEResultMode.MultipleRowsItems:
                    break;
                case QBEResultMode.SingleRowItem:
                    ResultGrid.MultiSelect = false;
                    break;
                case QBEResultMode.AllRowsItems:
                    break;
                default:
                    break;
            }

            QBEColumns ResultColumns = new QBEColumns();
            foreach (var QBEColumn in QBEColumns.Values)
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
                    ResultGrid.Columns.Clear();
                }
                ResultGrid.AutoGenerateColumns = false;

                foreach (QBEColumn column in ResultColumns.Values)
                {
                    DataGridViewColumn nc;
                    bool newcolumn = false;
                    if (ResultGrid.Columns[column.DbColumn] == null)
                    {
                        nc = new DataGridViewColumn();
                        newcolumn = true;
                    }
                    else
                    {
                        nc = ResultGrid.Columns[column.DbColumn];
                    }

                    switch (column.QBEColumnType)
                    {
                        case QBEColumnsTypes.CheckBox:
                            DataGridViewCheckBoxColumn nccheck = new DataGridViewCheckBoxColumn();
                            nccheck.ThreeState = true;
                            nccheck.IndeterminateValue = "";
                            nccheck.TrueValue = true;
                            nccheck.FalseValue = false;
                            nc = nccheck;
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
                            nc.DefaultCellStyle.Font = ResultGrid.DefaultCellStyle.Font;
                            if (nc.DefaultCellStyle.Font == null)
                                nc.DefaultCellStyle.Font = ResultGrid.Font;
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
                        ResultGrid.Columns.Add(nc);
                    }

                }
            }
            else
            {

            }

        }


        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData()
        {
            DoQuery();
        }

        /// <summary>
        /// Does the query.
        /// </summary>
        public void DoQuery()
        {
            BuildQuery3();
            ResultGrid.DataSource = mRepository.GetItems(SQLQuery, SQLQueryParameters).Value;
            ResultGrid.Visible = true;

        }

        /// <summary>
        /// Moves the previous.
        /// </summary>
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
        /// <summary>
        /// Moves the next.
        /// </summary>
        private void MoveNext()
        {


            ResultGrid.Focus();
            if (ResultGrid.CurrentRow.ClientIndex < ResultGrid.Rows.Count - 1)
            {
                int CurrentRowClientIndex = ResultGrid.CurrentRow.ClientIndex + 1;
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, CurrentRowClientIndex];
            }


        }
        /// <summary>
        /// Moves the first.
        /// </summary>
        private void MoveFirst()
        {

            ResultGrid.Focus();
            if (Conversions.ToBoolean(ResultGrid.Rows.Count))
            {
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, 0];
            }


        }
        /// <summary>
        /// Moves the last.
        /// </summary>
        private void MoveLast()
        {

            ResultGrid.Focus();
            if (Conversions.ToBoolean(ResultGrid.Rows.Count))
            {
                ResultGrid.CurrentCell = ResultGrid[ResultGrid.CurrentCell.ColumnIndex, ResultGrid.Rows.Count - 1];
            }


        }


        /// <summary>
        /// Builds the query3.
        /// </summary>
        private void BuildQuery3()
        {

            StringBuilder sqlwhere = new StringBuilder();
            string _WhereAND = "";
            OrderBy = OrderBy.Trim();
            QueryGrid.EndEdit();
            DynamicParameters parameters = new DynamicParameters();


            foreach (var item in QueryGrid.Rows)
            {
                StringBuilder sqlwhereitem = new StringBuilder();
                string Value = "";
                if (item[1].Value != null)
                    Value = item[1].Value.ToString();

                string _WhereItemOR = "";
                string[] Values;
                Type PropertyType = ModelProperties[item.Tag.ToString()].PropertyType;
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
                        if (chkLikeOperator.Checked)
                        {

                            // controlla il tipo di dato della colonna
                            if (Passero.Framework.Utilities.IsNumericType(ModelProperties[item.Tag.ToString()].GetMethod.ReturnType))
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

            if (TopRows > 0)
            {
                sTopRows = $"TOP ({TopRows})";
            }

            if (string.IsNullOrEmpty(BaseSQLQuery) == true)
            {
                SQLQuery = $"SELECT {sTopRows} * FROM {Passero.Framework.DapperHelper.Utilities.GetTableName<ModelClass>()}";
            }
            else
            {
                SQLQuery = $"SELECT {sTopRows} * FROM ({BaseSQLQuery.Trim()}) _b ";
            }

            if (sqlwhere.ToString().Trim() != "")
                SQLQuery = SQLQuery + $" WHERE {sqlwhere.ToString()}";
            if (string.IsNullOrEmpty(OrderBy) == false)
                SQLQuery = SQLQuery + $" ORDER BY {OrderBy}";
            SQLQueryParameters = parameters;

            foreach (var p in BaseDbParameteres.ParameterNames)
            {
                SQLQueryParameters.Add(p, ((SqlMapper.IParameterLookup)BaseDbParameteres)[p]);
            }

            string rSQL = Framework.DapperHelper.Utilities.ResolveSQL(SQLQuery, SQLQueryParameters);
        }


        /// <summary>
        /// Gets the comparision operator.
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Removes the comparision operator.
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <returns></returns>
        private string RemoveComparisionOperator(string Value)
        {
            string op = GetComparisionOperator(Value).Trim();

            if (Value.StartsWith(op, StringComparison.CurrentCultureIgnoreCase) == false)
                Value = op + Value;
            return Value.Substring(op.Length).Trim();
            // Sostituzione di Substring con AsSpan
            //return Value.AsSpan(op.Length).Trim().ToString();
        }

        /// <summary>
        /// Handles the Load event of the XQBEForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void XQBEForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the bPrev control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bPrev_Click(object sender, EventArgs e)
        {
            MovePrevious();
        }

        /// <summary>
        /// Handles the Click event of the bFirst control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bFirst_Click(object sender, EventArgs e)
        {
            MoveFirst();
        }

        /// <summary>
        /// Handles the Click event of the bNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bNext_Click(object sender, EventArgs e)
        {
            MoveNext();
        }

        /// <summary>
        /// Handles the Click event of the bLast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bLast_Click(object sender, EventArgs e)
        {
            MoveLast();
        }

        /// <summary>
        /// Handles the Click event of the bRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bRefresh_Click(object sender, EventArgs e)
        {
            DoQuery();
        }

        /// <summary>
        /// Handles the Click event of the bDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bDelete_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        /// <summary>
        /// Clears the filters.
        /// </summary>
        public void ClearFilters()
        {
            foreach (DataGridViewRow row in QueryGrid.Rows)
            {

                QBEColumn column = QBEColumns[row.Tag.ToString()];
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
        /// <summary>
        /// Handles the Click event of the bSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bSave_Click(object sender, EventArgs e)
        {
            SaveQueryResult();
        }

        /// <summary>
        /// Saves the query result.
        /// </summary>
        public void SaveQueryResult()
        {
            switch (QBEResultMode)
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


        /// <summary>
        /// Closes the qbe form.
        /// </summary>
        /// <param name="IgnoreCallBack">if set to <c>true</c> [ignore call back].</param>
        private void CloseQBEForm(bool IgnoreCallBack = false)
        {
            if (Owner == null && SetFocusControlAfterClose != null)
                Owner = Passero.Framework.Utilities.GetParentOfType<Form>(SetFocusControlAfterClose);


            if (IgnoreCallBack == false)
            {
                if (Owner != null && CallBackAction != null)
                {
                    try
                    {
                        CallBackAction.Invoke();
                    }
                    catch (Exception)
                    {

                    }
                }


                if (Owner != null && ResultGridModelItemsCallBackAction != null)
                {
                    try
                    {
                        ResultGridModelItemsCallBackAction.Invoke();
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (SetFocusControlAfterClose != null && SetFocusControlAfterClose.Focusable)
            {
                SetFocusControlAfterClose.Focus();
            }
            Close();
            Dispose();
        }
        /// <summary>
        /// Qbes the result mode all rows items.
        /// </summary>
        private void QBEResultMode_AllRowsItems()
        {
            {
                try
                {
                    ResultGridModelItems = new TargetModelItems<ModelClass>();
                    ResultGridModelItems.Items = mRepository.ModelItems;
                    //Passero.Framework.ReflectionHelper.SetPropertyValue(ref TargetModelItems, "Items",this.mRepository .ModelItems );
                }
                catch (Exception)
                {


                }

            }


            CloseQBEForm();
        }


        /// <summary>
        /// Qbes the result mode multiple rows items.
        /// </summary>
        private void QBEResultMode_MultipleRowsItems()
        {
            {
                try
                {

                    ResultGridModelItems = new TargetModelItems<ModelClass>();

                    foreach (DataGridViewRow row in ResultGrid.SelectedRows)
                    {
                        ResultGridModelItems.Items.Add((ModelClass)row.DataBoundItem);
                    }

                    Passero.Framework.ReflectionHelper.SetPropertyValue(ref ResultGridModelItems, "Items", ResultGridModelItems.Items);
                }
                catch (Exception)
                {


                }

            }


            CloseQBEForm();

            //Microsoft.Reporting.WebForms.ReportViewer x = new Microsoft.Reporting.WebForms.ReportViewer();
            //x.BackColor = System.Drawing.Color.White;   

        }


        /// <summary>
        /// Qbes the result mode bound controls.
        /// </summary>
        private void QBEResultMode_BoundControls()
        {
            if (ResultGrid.CurrentRow == null)
                return;

            ModelClass currentrowmodel = (ModelClass)ResultGrid.CurrentRow.DataBoundItem;
            foreach (QBEBoundControl item in QBEBoundControls)
            {
                var Value = Interaction.CallByName(currentrowmodel, item.ModelPropertyName, CallType.Get, null);
                if (Value is not null)
                {
                    Interaction.CallByName(item.Control, item.ControlPropertyName, CallType.Set, Value);
                }
                else
                {
                    Interaction.CallByName(item.Control, item.ControlPropertyName, CallType.Set, "");
                }
            }
            CloseQBEForm();
        }


        /// <summary>
        /// Qbes the result mode selected rows.
        /// </summary>
        private void QBEResultMode_SelectedRows()
        {
            if (ResultGrid.SelectedRows.Count == 0)
                return;
            if (QBEModelPropertiesMapping.Count == 0)
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
                foreach (ModelPropertyMapping Mapping in QBEModelPropertiesMapping)
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

            if (TargetRepository != null)
            {
                try
                {
                    Passero.Framework.ReflectionHelper.InvokeMethodByName(ref TargetRepository, "SetSQLQuery", sqlquery, parameters);
                }
                catch (Exception)
                {


                }

            }

            CloseQBEForm();
        }



        /// <summary>
        /// Copies the data row.
        /// </summary>
        /// <param name="oSourceRow">The o source row.</param>
        /// <param name="oTargetRow">The o target row.</param>
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

        /// <summary>
        /// Handles the Shown event of the XQBEForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void XQBEForm_Shown(object sender, EventArgs e)
        {
            SetupQBEForm();
            ResultGrid.Dock = DockStyle.Fill;
            //this.ResultGrid.Visible = true;
            //if (this.AutoLoadData)
            //    this.LoadData();
            Show();
            Focus();

        }

        /// <summary>
        /// Handles the FormClosed event of the XQBEForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        private void XQBEForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
            GC.Collect();
        }

        /// <summary>
        /// Handles the Click event of the btnExport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportResultGrid();
        }

        /// <summary>
        /// Handles the CellDoubleClick event of the ResultGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void ResultGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


            if (ResultGrid.CurrentRow == null)
                return;


            switch (QBEResultMode)
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

        /// <summary>
        /// Handles the MenuItemClicked event of the ContextMenuRecords control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MenuItemEventArgs"/> instance containing the event data.</param>
        private void ContextMenuRecords_MenuItemClicked(object sender, MenuItemEventArgs e)
        {
            TopRows = (int)e.MenuItem.Tag;
            Records.Text = e.MenuItem.Text;
        }


        /// <summary>
        /// Handles the RowEnter event of the ResultGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void ResultGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ResultGrid.CurrentCell == null)
                return;
            try
            {
                RecordLabel.Text = String.Format(RecordLabelHtmlFormat, ResultGrid.CurrentCell.RowIndex + 1, RecordLabelSeparator, ResultGrid.Rows.Count);
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Handles the Click event of the bClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bClose_Click(object sender, EventArgs e)
        {
            CloseQBEForm(true);

        }



        /// <summary>
        /// Handles the Appear event of the ResultGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ResultGrid_Appear(object sender, EventArgs e)
        {
            //this.ResultGrid.Visible = true;
        }


        /// <summary>
        /// Manages the tools click.
        /// </summary>
        /// <param name="Tool">The tool.</param>
        private void ManageToolsClick(ComponentTool Tool)
        {
            if (Tool.Name == "selectrows")
            {
                if (Tool.Pushed == false)
                {
                    ResultGrid.SelectAllRows();
                    Tool.Pushed = true;
                }

                else
                {
                    ResultGrid.ClearSelection();
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
                ResultGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                foreach (DataGridViewColumn column in ResultGrid.Columns)
                {
                    if (column.Visible)
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }
        /// <summary>
        /// Handles the ToolClick event of the SplitContainer_Panel1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ToolClickEventArgs"/> instance containing the event data.</param>
        private void SplitContainer_Panel1_ToolClick(object sender, ToolClickEventArgs e)
        {
            ManageToolsClick(e.Tool);
        }

        /// <summary>
        /// Handles the CellDoubleClick event of the QueryGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void QueryGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                QueryGrid.CurrentCell.ReadOnly = false;
            }
        }
    }



}
