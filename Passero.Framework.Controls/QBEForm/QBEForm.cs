using Dapper;
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Passero.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
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


        public bool ShowSaveLoadButtons
        {
            get 
            { 
                return bSaveQBE.Visible && bLoadQBE.Visible; 
            }    
            set
            {
                bSaveQBE.Visible = value;
                bLoadQBE.Visible = value;
            }
        }

        public bool ShowPrintButton
        {
            get
            {
                return bPrint.Visible;
            }
            set
            {
                bPrint.Visible = value;
            }
        }

        public string QueryContext { get; set; }
        public string QueryDescription { get; set; }
        public QBEQueryMode QBEQueryMode { get; set; } = QBEQueryMode.QueryGrid;
        public CultureInfo FilterCulture { get; set; } = CultureInfo.CurrentCulture;
        public bool EnableRelativeDateTokens { get; set; } = true;
        public bool FilterCaseInsensitiveText { get; set; } = true;
        public bool AllowTextRelationalOperators { get; set; } = false;
        public ISet<string> CodeColumns { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Tooltip che descrive la sintassi di filtro supportata per i campi di testo (stringhe).
        /// </summary>
        public const string QueryGridFilterSyntaxStringTooltip = @"Sintassi di filtro per campi di testo";
        /// <summary>
        /// Tooltip che descrive la sintassi di filtro supportata per i campi numerici.
        /// </summary>
        public const string QueryGridFilterSyntaxNumberTooltip = @"Sintassi di filtro per campi numerici";

        /// <summary>
        /// Tooltip che descrive la sintassi di filtro supportata per i campi data.
        /// </summary>
        public const string QueryGridFilterSyntaxDateTooltip = @"Sintassi di filtro per campi data";
        /// <summary>
        /// Tooltip che descrive la sintassi di filtro supportata per i campi booleani.
        /// </summary>
        public const string QueryGridQueryGridFilterSyntaxBooleanTooltip = @"Sintassi di filtro per campi booleani";

        /// <summary>
        /// Tooltip che descrive la sintassi generale del QueryBuilder.
        /// </summary>
        public const string QueryGridGeneralTooltip = @"Guida QueryGrid";

        /// <summary>
        /// Ottiene il tooltip della sintassi di filtro per un dato tipo di campo.
        /// </summary>
        /// <param name="fieldType">Il tipo di campo.</param>
        /// <returns>Il tooltip della sintassi supportata per quel tipo.</returns>
        public static string QueryGridGetFilterSyntaxTooltip(QueryGridFieldType fieldType)
        {
            return fieldType switch
            {
                QueryGridFieldType.String => QueryGridFilterSyntaxStringTooltip,
                QueryGridFieldType.Number => QueryGridFilterSyntaxNumberTooltip,
                QueryGridFieldType.Date => QueryGridFilterSyntaxDateTooltip,
                QueryGridFieldType.DateTime => QueryGridFilterSyntaxDateTooltip,
                QueryGridFieldType.Boolean => QueryGridQueryGridFilterSyntaxBooleanTooltip,
                QueryGridFieldType.Enum => QueryGridFilterSyntaxStringTooltip,
                _ => QueryGridGeneralTooltip
            };
        }

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
        //private Repository<ModelClass> mRepository = new Repository<ModelClass>();
        private Base.IPasseroRepository<ModelClass> mRepository = new Repository<ModelClass>();

        /// <summary>
        /// The SQL query parameters
        /// </summary>
        public DynamicParameters SQLQueryParameters = new DynamicParameters();
        /// <summary>
        /// The SQL query
        /// </summary>
        public string SQLQuery = "";

        public string SQLQuerySelector = "";
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
            return Passero.Framework.Utilities.ResolveSQL(SQLQuery, SQLQueryParameters);
        }
 

        public ProviderFeatures ProviderFeatures
        {
            get => Repository.ProviderFeatures; 
            set => Repository.ProviderFeatures  = value;
        }
        private Base.IPasseroRepository<ModelClass> Repository
        {
            get => mRepository as Repository<ModelClass>;
            set
            {
                mRepository = value;
                LoadModelProperties();
            }
        }

        /// <summary>
        /// Espone il repository interno come <see cref="IPasseroRepository{ModelClass}"/>.
        /// Usare questa proprietà quando il repository può essere EF o Dapper.
        /// </summary>
        public Base.IPasseroRepository<ModelClass> InternalRepository
        {
            get => mRepository;
            set
            {
                mRepository = value;
                LoadModelProperties();
            }
        }

        //private void LoadModelProperties()
        //{
        //    var ModelPropertiesInfo = mRepository.GetProperties();
        //    ModelProperties = new Dictionary<string, System.Reflection.PropertyInfo>();
        //    foreach (var item in ModelPropertiesInfo)
        //    {
        //        ModelProperties.Add(item.Name, item);
        //    }
        //}   
        private void LoadModelProperties()
        {
            // Legge le proprietà direttamente dal tipo ModelClass via reflection,
            // senza dipendere dall'implementazione concreta del repository (Dapper o EF).
            ModelProperties = new Dictionary<string, System.Reflection.PropertyInfo>();
            foreach (var prop in typeof(ModelClass).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                // Esclude le proprietà [Computed] e [NotMapped] come fa EfRepository
                bool isComputed = prop.GetCustomAttribute<Dapper.Contrib.Extensions.ComputedAttribute>() != null;
                bool isNotMapped = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() != null;
                if (!isComputed && !isNotMapped)
                {
                    ModelProperties[prop.Name] = prop;
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
        public IDbConnection DbConnection
        {
            get
            {
                return Repository.DbConnection;
            }
            set
            {
                Repository = new Repository<ModelClass>();
                Repository.DbConnection = value;
            }

        }
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
        /// Gets or sets the QueryGridQuerySave call back action.
        /// </summary>
        /// <value>
        /// The call back action.
        /// </value>
        public Action<string> QueryGridQuerySaveCallBackAction { get; set; }
        /// <summary>
        /// Gets or sets the QueryGridQueryLoad call back action.
        /// </summary>
        /// <value>
        /// The call back action.
        /// </value>
        public Action QueryGridQueryLoadCallBackAction { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [automatic load data].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic load data]; otherwise, <c>false</c>.
        /// </value>
        /// 
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
            TabPageQueryGrid.Hidden = true;
            RecordLabel.Text = RecordLabelHtmlFormat;
            bSaveQBE.Visible = false;
            bLoadQBE.Visible = false;
            bPrint.Visible = false;
            

        }

        /// <summary>
        /// Costruttore con <see cref="IPasseroDbContext"/>: istanzia automaticamente
        /// il repository corretto (<see cref="EfRepository{ModelClass}"/> o <see cref="Repository{ModelClass}"/>)
        /// in base all'<see cref="ORMType"/> del DbContext fornito.
        /// </summary>
        /// <param name="dbContext">Il DbContext che definisce l'ORM da usare.</param>
        /// <param name="owner">Form proprietaria opzionale.</param>
        public QBEForm(Base.IPasseroDbContext dbContext, Form owner = null)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            if (owner != null)
                this.Owner = owner;

            // Crea il repository corretto in base all'ORMType del DbContext
            dbContext.EnsureConnectionOpen();
            switch (dbContext.ORMType)
            {
                case Base.ORMType.EntityFrameworkCore:
                case Base.ORMType.EntityFramework6:
                case Base.ORMType.EntityFramework:
                    mRepository = new EfRepository<ModelClass>(dbContext);
                    break;
                default:
                    // Dapper: usa la connessione esposta dal DbContext
                    var dapperRepo = new Repository<ModelClass>();
                    dapperRepo.DbConnection = dbContext.DbConnection;
                    mRepository = dapperRepo;
                    break;
            }

            LoadModelProperties();
            InitializeComponent();
            bSaveQBE.Visible = false;
            bLoadQBE.Visible = false;
            bPrint.Visible = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QBEForm{ModelClass}"/> class.
        /// </summary>
        /// <param name="DbConnection">The database connection.</param>
        public QBEForm(IDbConnection DbConnection, Form Owner = null)
        {

            if (Owner != null)
            {
                this.Owner = Owner;
            }
            Repository = new Repository<ModelClass>(DbConnection);
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
        /// <param name="TargetRepository">The target repository.</param>
        /// <param name="CallBackAction">The call back action.</param>
        public void SetTargetRepository<T>(T Repository, Action CallBackAction = null) where T : class
        {
            TargetRepository = Repository;
            if (CallBackAction != null)
            {
                this.CallBackAction = CallBackAction;
            }
            else
            {

            }
        }
        /// <summary>
        /// Sets the target ViewModel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TargetViewModel">The target ViewModel.</param>
        /// <param name="CallBackAction">The call back action.</param>
        public void SetTargetViewModel<T>(T ViewModel, Action CallBackAction = null) where T : class
        {
            TargetRepository = Passero.Framework.ReflectionHelper.GetPropertyValue(ViewModel, "Repository");

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
                MiniExcelLibs.MiniExcel.SaveAs(filename, mRepository.ModelItems, true, "Sheet1", MiniExcelLibs.ExcelType.XLSX);
                expofilenameextension = ".xlsx";
            }

            if (rbCSV.Checked)
            {
                MiniExcelLibs.MiniExcel.SaveAs(filename, Repository.ModelItems, true, "Sheet1", MiniExcelLibs.ExcelType.CSV);
                expofilenameextension = ".csv";
            }

            if (rbJSON.Checked)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(mRepository.ModelItems, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(filename, json);
                expofilenameextension = ".json";
            }

            if (rbXML.Checked)
            {
                DataTable dt;
                dt = Passero.Framework.DataBaseHelper.ListToDataTable(mRepository.ModelItems);
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
        /// <param name="AsMdiChildren">if set to <c>true</c> [wait].</param>
        public void ShowQBE(bool AsMdiChildren = true)
        {
            SetupQBEForm();

            BuildQuery3(true);

            if (AutoLoadData)
                LoadData();

            if (AsMdiChildren == false)
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

            TabPageQueryGrid.Hidden = false;
            TabPageExport.Hidden = false;
            TabPageDebug.Hidden = true;
            PanelExport.Visible = true;

            CheckQBEColumns();
            SetupQueryGrid();
            SetupResultGrid(OverrideResultGridDefinition);

            ResultGrid.Dock = DockStyle.Fill;
            //this.ResultGrid.Visible = true;
            //QueryGrid.Visible = true;

            //if (Owner == null && SetFocusControlAfterClose != null)
            //    Owner = Passero.Framework.Utilities.GetParentOfType<Form>(SetFocusControlAfterClose);

            if (Owner != null && Owner.MdiParent != null)
                MdiParent = Owner.MdiParent;

            if (this.QBEQueryMode == QBEQueryMode.QueryGrid)
                this.TabControl.SelectedTab = this.TabPageQueryGrid;

            if (this.QBEQueryMode == QBEQueryMode.QueryGridQueryBuilder)
                this.TabControl.SelectedTab = this.TabPageQueryBuilder;
            if (this.QBEQueryMode == QBEQueryMode.QueryGridQueryBuilder)
                this.TabControl.SelectedTab = this.TabPageQueryGrid;

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
                    column.DbColumnName = item.Name;
                    column.FriendlyName = item.Name;
                    column.UseInQBE = true;
                    column.DisplayInQBEResult = true;
                    QBEColumns.Add(column.DbColumnName, column);
                }

            }
        }

        /// <summary>
        /// Setups the query grid.
        /// </summary>
        public void SetupQueryGrid()
        {
            QueryGrid.Rows.Clear();

            foreach (var QBEColumn in QBEColumns.Values)
            {
                if (QBEColumn.UseInQBE)
                {
                    int rowIndex = QueryGrid.Rows.Add(QBEColumn.FriendlyName, QBEColumn.QBEValue);
                    DataGridViewRow row = QueryGrid.Rows[rowIndex];

                    Type columnType = ModelProperties[QBEColumn.DbColumnName].PropertyType;
                    Type underlyingType = Nullable.GetUnderlyingType(columnType) ?? columnType;
                    bool isBoolean = Passero.Framework.Utilities.IsBooleanType(underlyingType);

                    // Usa QBEColumnType se specificato, altrimenti determina automaticamente
                    bool useCheckBox = (QBEColumn.QBEColumnType == QBEColumnsTypes.CheckBox) ||
                                       (isBoolean && QBEColumn.QBEColumnType == QBEColumnsTypes.TextBox == false);

                    if (useCheckBox)
                    {
                        DataGridViewCheckBoxCell checkBoxCell = new DataGridViewCheckBoxCell
                        {
                            ThreeState = true,
                            IndeterminateValue = "",
                            TrueValue = 1,
                            FalseValue = 0
                        };

                        // Assegna il valore iniziale
                        if (QBEColumn.QBEValue != null)
                        {
                            if (QBEColumn.QBEValue is bool boolValue)
                            {
                                checkBoxCell.Value = boolValue ? 1 : 0;
                            }
                            else if (QBEColumn.QBEValue is int intValue)
                            {
                                checkBoxCell.Value = intValue;
                            }
                            else if (int.TryParse(QBEColumn.QBEValue.ToString(), out int parsedValue))
                            {
                                checkBoxCell.Value = parsedValue;
                            }
                            else
                            {
                                checkBoxCell.Value = checkBoxCell.IndeterminateValue;
                            }
                        }
                        else
                        {
                            checkBoxCell.Value = checkBoxCell.IndeterminateValue;
                        }

                        row.Cells[1] = checkBoxCell;
                        row.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                        // Aggiungi tooltip per booleani
                        row.Cells[1].ToolTipText = QueryGridQueryGridFilterSyntaxBooleanTooltip;
                    }
                    else
                    {
                        // Aggiungi tooltip per TextBox basato sul tipo di dato
                        var fieldType = DetermineFieldTypeFromPropertyType(underlyingType);
                        row.Cells[1].ToolTipText = QueryGridGetFilterSyntaxTooltip(fieldType);
                    }

                    row.Tag = QBEColumn.DbColumnName;

                    // Se il valore è pre-configurato, rendi la cella ReadOnly
                    if (QBEColumn.QBEValue != null && !string.IsNullOrWhiteSpace(QBEColumn.QBEValue.ToString().Trim()))
                    {
                        row.Cells[1].ReadOnly = true;
                    }

                    // Nascondi la riga se configurato
                    if (QBEColumn.DisplayInQBE == false)
                    {
                        row.Visible = false;
                    }
                }
            }

            CheckQBEQueryMode();
        }

        /// <summary>
        /// Determina il tipo di campo del QueryBuilder in base al tipo .NET.
        /// </summary>
        private QueryGridFieldType DetermineFieldTypeFromPropertyType(Type type)
        {
            if (type == typeof(bool))
                return QueryGridFieldType.Boolean;

            if (type == typeof(DateTime) || type == typeof(System.DateTime))
                return QueryGridFieldType.DateTime;

            if (type == typeof(decimal) || type == typeof(double) || type == typeof(float) ||
                type == typeof(int) || type == typeof(long) || type == typeof(short))
                return QueryGridFieldType.Number;

            if (type.IsEnum)
                return QueryGridFieldType.Enum;

            return QueryGridFieldType.String;
        }




        /// <summary>
        /// Setups the query grid or query builder.
        /// </summary>
        public void CheckQBEQueryMode()
        {
            switch (this.QBEQueryMode)
            {
                case QBEQueryMode.QueryGrid:
                    this.TabPageQueryGrid.Hidden = false;
                    this.TabPageQueryBuilder.Hidden = true;
                    break;
                case QBEQueryMode.QueryBuilder:
                    this.TabPageQueryGrid.Hidden = true;
                    this.TabPageQueryBuilder.Hidden = false;
                    break;
                case QBEQueryMode.QueryGridQueryBuilder:
                    this.TabPageQueryGrid.Hidden = false;
                    this.TabPageQueryBuilder.Hidden = false;
                    break;
                default:
                    this.TabPageQueryGrid.Hidden = false;
                    this.TabPageQueryBuilder.Hidden = true;
                    break;
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
                    ResultColumns.Add(QBEColumn.DbColumnName, QBEColumn);
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
                    if (ResultGrid.Columns[column.DbColumnName] == null)
                    {
                        nc = new DataGridViewColumn();
                        newcolumn = true;
                    }
                    else
                    {
                        nc = ResultGrid.Columns[column.DbColumnName];
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
                        nc.Name = column.DbColumnName;
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

       
            // QueryGrid: usa la query generata dal QueryGrid   
            if (this.TabControl.SelectedTab == this.TabPageQueryGrid)
                ResultGrid.DataSource = mRepository.GetItems(SQLQuery, SQLQueryParameters).Value;

            // QueryBuilder: usa la query generata dal QueryBuilderControl
            if (this.TabControl.SelectedTab == this.TabPageQueryBuilder)
            {
                string _sqlquery = this.SQLQuerySelector;
                DynamicParameters _sqlqueryparameters;

                var sql = this.queryBuilderControl.GetParameterizedSqlWhere();
                _sqlqueryparameters = QueryBuilderControl.DictionaryToDynamicParameters(sql.Parameters);
                if (sql.WhereClause != "")
                {
                    _sqlquery = $"{this.SQLQuerySelector} WHERE {sql.WhereClause}";
                }

                ResultGrid.DataSource = mRepository.GetItems(_sqlquery, _sqlqueryparameters).Value;
            }

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
        /// 

        private void BuildQuery3_OLD()
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

                        // ✅ GESTIONE SPECIFICA PER COLONNE BOOLEAN/BIT
                        if (PropertyTypeIs == Passero.Framework.EnumSystemTypeIs.Boolean)
                        {
                            // Converte il valore stringa in boolean
                            bool boolValue;
                            if (bool.TryParse(_Value, out boolValue))
                            {
                                sqlwhereitem.Append($" {_WhereItemOR} {item.Tag.ToString()} = {parametername}");
                                parameters.Add(parametername, boolValue, System.Data.DbType.Boolean);
                            }
                            // Se il valore non è un boolean valido, salta questa condizione
                        }
                        else if (chkLikeOperator.Checked)
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
                SQLQuery = $"SELECT {sTopRows} * FROM {Passero.Framework.Utilities.GetModelTableName<ModelClass>()}";
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

            string rSQL = Framework.Utilities.ResolveSQL(SQLQuery, SQLQueryParameters);
        }

        /// <summary>
        /// Costruisce la query WHERE usando NavFilterSqlEngine.
        /// Sintassi supportata:
        /// - Singolo valore: "Smith"
        /// - OR (pipe): "Smith|Jones" → (Name = Smith) OR (Name = Jones)
        /// - AND (ampersand): ">100&<500" → (ID > 100) AND (ID < 500)
        /// - Range: "100..500" → (ID >= 100) AND (ID <= 500)
        /// - Wildcards: "Smith*", "*son" (solo su stringhe, con LIKE)
        /// - Operatori: "=", "<>", ">", ">=", "<", "<="
        /// - Speciali: "<empty>", "<notempty>"
        /// - Case insensitive per booleani: "true", "false", "yes", "no", "1", "0"
        /// </summary>
        private void BuildQuery3(bool setup = false)
        {


            if (setup == true | this.TabControl.SelectedTab == this.TabPageQueryGrid)
            {
                StringBuilder sqlWhere = new StringBuilder();
                OrderBy = (OrderBy ?? string.Empty).Trim();
                QueryGrid.EndEdit();
                DynamicParameters parameters = new DynamicParameters();

                foreach (DataGridViewRow row in QueryGrid.Rows)
                {
                    string value = null;
                    object cellValue = row?.Cells?[1]?.Value;

                    // Gestisci i diversi tipi di celle
                    if (cellValue == null || cellValue.Equals(""))
                    {
                        // Valore nullo o stringa vuota (indeterminato) - salta il filtro
                        continue;
                    }
                    else if (cellValue is bool boolValue)
                    {
                        // Converti bool a "true" o "false"
                        value = boolValue ? "true" : "false";
                    }
                    else if (cellValue is int intValue)
                    {
                        // Converti int (da checkbox 0/1) a stringa
                        value = intValue.ToString();
                    }
                    else
                    {
                        // Per altri tipi, converti a stringa
                        value = cellValue.ToString();
                    }

                    // Salta se il valore è vuoto
                    if (string.IsNullOrWhiteSpace(value))
                        continue;

                    string columnName = row.Tag?.ToString();
                    if (string.IsNullOrWhiteSpace(columnName))
                        continue;

                    if (!ModelProperties.ContainsKey(columnName))
                        continue;

                    Type propertyType = ModelProperties[columnName].PropertyType;
                    bool isCodeColumn = CodeColumns.Contains(columnName);

                    var build = NavFilterSqlEngine.BuildColumnPredicate(
                        columnName: columnName,
                        propertyType: propertyType,
                        filterText: value.Trim(),
                        isCodeColumn: isCodeColumn,
                        parameterPrefix: $"f_{columnName}_{row.Index}",
                        parameters: parameters,
                        options: new NavFilterSqlOptions
                        {
                            Culture = FilterCulture,
                            CaseInsensitiveText = FilterCaseInsensitiveText,
                            AllowRelativeDateTokens = EnableRelativeDateTokens,
                            AllowTextRelationalOperators = AllowTextRelationalOperators,
                            UseLikeOperator = UseLikeOperator
                        });

                    if (build.Errors.Count > 0)
                    {
                        NavFilterError first = build.Errors[0];
                        string technical = $"{first.Code}: {first.TechnicalMessage}";
                        string user = first.UserMessage;
                        MessageBox.Show($"{user}\n{technical}", "Filtro non valido",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        throw new InvalidOperationException(technical);
                    }

                    if (!string.IsNullOrWhiteSpace(build.Sql))
                    {
                        if (sqlWhere.Length > 0)
                            sqlWhere.Append(" AND ");

                        sqlWhere.Append("(");
                        sqlWhere.Append(build.Sql);
                        sqlWhere.Append(")");
                    }
                }

                if (string.IsNullOrEmpty(BaseSQLQuery))
                    SQLQuery = $"SELECT  * FROM {Passero.Framework.Utilities.GetModelTableName<ModelClass>()}";
                else
                    SQLQuery = $"SELECT  * FROM ({BaseSQLQuery.Trim()}) _b";


                SQLQuerySelector = SQLQuery;

                if (sqlWhere.Length > 0)
                    SQLQuery += $" WHERE {sqlWhere}";

                // rimossa di "ORDER BY" se presente all'inizio della stringa OrderBy   
                OrderBy = (OrderBy ?? string.Empty).Trim(); 
                if (OrderBy .StartsWith("ORDER BY", StringComparison.InvariantCultureIgnoreCase))
                {
                    OrderBy = OrderBy.Substring(8).Trim(); // Rimuove "ORDER BY" se presente    
                }

                if (string.IsNullOrEmpty(OrderBy))
                    SQLQuery += $" ORDER BY {Repository.DefaultOrderbyClause}";
                else
                    SQLQuery += $" ORDER BY {OrderBy} ";


                

                SQLQuery = SqlDialectBuilder.ApplyLimit (SQLQuery, Repository.ProviderFeatures, TopRows );    

                SQLQueryParameters = parameters;

                foreach (var p in BaseDbParameteres.ParameterNames)
                    SQLQueryParameters.Add(p, ((SqlMapper.IParameterLookup)BaseDbParameteres)[p]);


                if (this.TabControl.SelectedTab == TabPageQueryBuilder)
                {
                    this.queryBuilderControl.LoadColumnsAndRulesFromQBE(this.QBEColumns);
                    this.queryBuilderControl.LoadQueryFromSql(SQLQuery, SQLQueryParameters);

                }
            }

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
            if (this.Owner != null)
            {
            }

            else
            {
                if (Owner == null && SetFocusControlAfterClose != null)
                    Owner = Passero.Framework.Utilities.GetParentOfType<Form>(SetFocusControlAfterClose);
            }

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
        /// 
        private void QBEResultMode_SelectedRows()
        {
            if (ResultGrid.SelectedRows.Count == 0)
                return;
            if (QBEModelPropertiesMapping.Count == 0)
                return;

            object targetModel = Passero.Framework.ReflectionHelper.GetPropertyValue(TargetRepository, "ModelItem");
            if (targetModel is null)
                targetModel = GetEmptyModel();

            Type targetModelType = targetModel.GetType();
            var targetModelProperties = new Dictionary<string, PropertyInfo>();
            foreach (var item in targetModelType.GetProperties())
                targetModelProperties.Add(item.Name, item);

            DynamicParameters parameters = new DynamicParameters();
            string _AND = "";
            string _OR = "";
            int i = 1;
            StringBuilder sqlwhere = new StringBuilder();

            foreach (DataGridViewRow row in ResultGrid.SelectedRows)
            {
                StringBuilder sqlwhererow = new StringBuilder();
                _AND = "";
                foreach (ModelPropertyMapping mapping in QBEModelPropertiesMapping)
                {
                    object propertyvalue = row[mapping.QBEModelProperty].Value;
                    string propertyname = mapping.TargetModelProperty;
                    Type type = targetModelProperties[propertyname].PropertyType;
                    DbType dbType = Passero.Framework.Utilities.GetDbType(type);
                    string parametername = $"@{propertyname}_{i}";
                    parameters.Add(parametername, propertyvalue, dbType);
                    sqlwhererow.Append($"{_AND} {propertyname} = {parametername} ");
                    _AND = " AND ";
                    i++;
                }
                sqlwhere.Append($" {_OR} ( {sqlwhererow} )");
                _OR = " OR ";
            }

            string sqlquery = $"SELECT * FROM {Passero.Framework.Utilities.GetModelTableName(targetModel)}";
            if (sqlwhere.Length > 0)
                sqlquery = sqlquery + " WHERE " + sqlwhere;

            if (TargetRepository != null)
            {
                try
                {
                    // Usa l'interfaccia IPasseroRepository<T> direttamente se disponibile,
                    // evitando la reflection che può fallire silenziosamente su EfRepository.
                    if (TargetRepository is Passero.Framework.Base.IPasseroRepository<ModelClass> typedRepo)
                    {
                        typedRepo.SetSQLQuery(sqlquery, parameters);
                    }
                    else
                    {
                        // Fallback reflection per repository non tipizzati (retrocompatibilità)
                        Passero.Framework.ReflectionHelper.InvokeMethodByName(
                            ref TargetRepository, "SetSQLQuery", sqlquery, parameters);
                    }
                }
                catch (Exception)
                {
                    // Silenzioso: il QBE si chiude comunque
                }
            }

            CloseQBEForm();
        }
        private void QBEResultMode_SelectedRows_OLD()
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

            sqlquery = $"SELECT * FROM {Passero.Framework.Utilities.GetModelTableName(TargetModel)}";
            if (sqlwhere.ToString() != "")
                sqlquery = sqlquery + " WHERE " + sqlwhere.ToString();

            if (TargetRepository != null)
            {
                try
                {
                    Passero.Framework.ReflectionHelper.InvokeMethodByName(ref TargetRepository, "SetSQLQuery", sqlquery, parameters);
                }
                catch (Exception ex)
                {

                    //MessageBox.Show("a");
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
        private void XQBEForm_Show(object sender, EventArgs e)
        {
            //SetupQBEForm();
            //ResultGrid.Dock = DockStyle.Fill;
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

        private void TabPageReportQuery_Resize(object sender, EventArgs e)
        {
            this.QueryGrid.Width = this.TabPageQueryGrid.Width - this.TabPageQueryGrid.Left;
        }

        /// <summary>
        /// Legge le righe della QueryGrid e serializza i valori delle colonne 0 e 1 in un oggetto JSON.
        /// </summary>
        /// <returns>Stringa JSON contenente i dati della QueryGrid.</returns>
        public string QueryGridToJson()
        {
            var rowDataList = new List<Dictionary<string, object>>();

            foreach (DataGridViewRow row in QueryGrid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                var rowData = new Dictionary<string, object>();

                // Leggi il valore e il tag della colonna 0
                object column0Value = row.Cells[0].Value;
                object column0Tag = row.Cells[0].Tag;

                // Leggi il valore e il tag della colonna 1
                object column1Value = row.Cells[1].Value;
                object column1Tag = row.Cells[1].Tag;

                // Costruisci il dizionario per la riga
                rowData["column0"] = new
                {
                    value = column0Value,
                    tag = column0Tag
                };

                rowData["column1"] = new
                {
                    value = column1Value,
                    tag = column1Tag
                };

                rowData["rowTag"] = row.Tag;

                rowDataList.Add(rowData);
            }

            // Serializza in JSON usando Newtonsoft.Json (Nuget package già presente nel progetto)
            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(rowDataList, Newtonsoft.Json.Formatting.Indented);

            return jsonResult;
        }

        /// <summary>
        /// Riconfigura la QueryGrid a partire da una stringa JSON.
        /// </summary>
        /// <param name="jsonData">Stringa JSON contenente i dati della QueryGrid.</param>
        public void JsonToQueryGrid(string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                return;

            try
            {
                var rowDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData);

                if (rowDataList == null || rowDataList.Count == 0)
                    return;

                // Itera su tutti i dati JSON
                foreach (var rowData in rowDataList)
                {
                    // Estrai il rowTag per identificare la riga corretta
                    string rowTag = rowData.ContainsKey("rowTag") ? rowData["rowTag"]?.ToString() : null;

                    if (string.IsNullOrEmpty(rowTag))
                        continue;

                    // Trova la riga nella QueryGrid corrispondente al rowTag
                    DataGridViewRow targetRow = null;
                    foreach (DataGridViewRow row in QueryGrid.Rows)
                    {
                        if (row.Tag?.ToString() == rowTag)
                        {
                            targetRow = row;
                            break;
                        }
                    }

                    if (targetRow == null)
                        continue;

                    // Ripristina il valore della colonna 1 (il valore del filtro)
                    if (rowData.ContainsKey("column1"))
                    {
                        var column1Data = rowData["column1"];

                        if (column1Data is Newtonsoft.Json.Linq.JObject jObject)
                        {
                            object column1Value = jObject["value"];

                            // Gestisci i diversi tipi di celle (TextBox vs CheckBox)
                            if (targetRow.Cells[1] is DataGridViewCheckBoxCell checkBoxCell)
                            {
                                // Per le checkbox, converti il valore appropriatamente
                                if (column1Value != null)
                                {
                                    if (column1Value is bool boolValue)
                                    {
                                        checkBoxCell.Value = boolValue ? 1 : 0;
                                    }
                                    else if (column1Value is int intValue)
                                    {
                                        checkBoxCell.Value = intValue;
                                    }
                                    else if (int.TryParse(column1Value.ToString(), out int parsedValue))
                                    {
                                        checkBoxCell.Value = parsedValue;
                                    }
                                    else
                                    {
                                        checkBoxCell.Value = checkBoxCell.IndeterminateValue;
                                    }
                                }
                                else
                                {
                                    checkBoxCell.Value = checkBoxCell.IndeterminateValue;
                                }
                            }
                            else
                            {
                                // Per le TextBox, assegna il valore direttamente
                                targetRow.Cells[1].Value = column1Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore nel deserializzare JSON in QueryGrid: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Evento generato quando la QueryGrid viene salvata in JSON.
        /// </summary>
        public event EventHandler<QueryGridSaveEventArgs> QueryGridSaving;

        /// <summary>
        /// Evento generato quando la QueryGrid viene caricata da JSON.
        /// </summary>
        public event EventHandler<QueryGridLoadEventArgs> QueryGridLoading;

        /// <summary>
        /// Solleva l'evento QueryGridSaving.
        /// </summary>
        protected virtual void OnQueryGridSaving(QueryGridSaveEventArgs e)
        {
            QueryGridSaving?.Invoke(this, e);
        }

        /// <summary>
        /// Solleva l'evento QueryGridLoading.
        /// </summary>
        protected virtual void OnQueryGridLoading(QueryGridLoadEventArgs e)
        {
            QueryGridLoading?.Invoke(this, e);
        }

      

        private void bSaveQBE_Click(object sender, EventArgs e)
        {
            string jsonData = QueryGridToJson();

            if (QueryGridQuerySaveCallBackAction != null)
            {
                QueryGridQuerySaveCallBackAction?.Invoke(jsonData);
            }
            else
            {
                var saveEventArgs = new QueryGridSaveEventArgs { QueryGridJson = jsonData };
                OnQueryGridSaving(saveEventArgs);

                if (!saveEventArgs.Cancel)
                {
                    // Esegui azioni predefinite se necessario
                    //System.Diagnostics.Debug.WriteLine($"QueryGrid salvata: {jsonData}");
                }
            }
        }

        private void bLoadQBE_Click(object sender, EventArgs e)
        {
            if (QueryGridQueryLoadCallBackAction != null)
            {
                QueryGridQueryLoadCallBackAction?.Invoke();
            }
            else
            {
                var loadEventArgs = new QueryGridLoadEventArgs();
                OnQueryGridLoading(loadEventArgs);
                if (!loadEventArgs.Cancel)
                {
                    // Esegui azioni predefinite se necessario
                    //System.Diagnostics.Debug.WriteLine("Caricamento QueryGrid completato.");
                }
            }
        }
    }
}
