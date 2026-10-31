using Dapper;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    [DefaultBindingProperty("Value")]
    public partial class DbLookUpTextBoxN : Wisej.Web.TextBox
    {
        public enum LookUpModes
        {
            Standard = 0,
            Enhanced = 1
        }

        private Expression<Func<object, bool>> _linqExpression;
        private Func<object> mDynamicDataSource;
        private object mDataSource;
        private object mValue;
        private Type mModelClass;
        private string mValueMember = string.Empty;
        private string mDisplayMember = string.Empty;
        private LookUpModes mLookUpMode = LookUpModes.Standard;
        private bool mSameColumn;
        private bool mLookUpOnEdit;
        private bool mShowSearchTool = true;
        private bool mShowClearTool;
        private bool mLock;
        private DynamicParameters parameters = new DynamicParameters();

        public DbLookUpTextBoxN()
        {
            InitializeComponent();
            KeyPress += DbLookUpTextBoxN_KeyPress;
            TextChanged += DbLookUpTextBoxN_TextChanged;
            ToolClick += DbLookUpTextBoxN_ToolClick;
        }

        public DbLookUpTextBoxN(Type modelClass) : this()
        {
            ModelClass = modelClass;
        }

        public DbLookUpTextBoxN(IContainer container) : this()
        {
            container?.Add(this);
        }

        [Browsable(false)]
        public Func<object> DynamicDataSource
        {
            get => mDynamicDataSource;
            set => mDynamicDataSource = value;
        }

        public object DataSource
        {
            get => mDataSource;
            set => mDataSource = value;
        }

        public Dictionary<string, DataBindControl> DataBindControls { get; set; } = new Dictionary<string, DataBindControl>(StringComparer.InvariantCultureIgnoreCase);
        public delegate void eLookUpDataEventHandler();
        public event EventHandler LookUpCompleted;
        public event eLookUpDataEventHandler eLookUp;
        public string EditPropertyName { get; private set; } = "Value";
        public bool ValidLookUp { get; set; }
        public IDbConnection DbConnection { get; set; }
        public string SQLQuery { get; private set; }
        public DynamicParameters DbParameters { get; set; } = new DynamicParameters();
        public IDictionary<string, object> Model { get; private set; }
        public string SelectClause { get; set; } = " * ";
        public string WhereClause { get; set; } = string.Empty;

        public Type ModelClass
        {
            get => mModelClass;
            set => mModelClass = value;
        }

        public Type ModelType
        {
            get => ModelClass;
            set => ModelClass = value;
        }

        public LookUpModes LookUpMode
        {
            get => mLookUpMode;
            set => mLookUpMode = value;
        }

        public bool ShowSearchTool
        {
            get => mShowSearchTool;
            set
            {
                mShowSearchTool = value;
                if (Tools["search"] != null)
                    Tools["search"].Visible = value;
            }
        }

        private void DbLookUpTextBoxN_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (string.Equals(e.Tool?.Name, "search", StringComparison.OrdinalIgnoreCase))
                LookUp(true);
            else if (string.Equals(e.Tool?.Name, "clear", StringComparison.OrdinalIgnoreCase))
                Value = null;
        }

        public bool ShowClearTool
        {
            get => mShowClearTool;
            set
            {
                mShowClearTool = value;
                if (Tools["clear"] != null)
                    Tools["clear"].Visible = value;
            }
        }

        public bool LookUpOnEdit
        {
            get => mLookUpOnEdit;
            set => mLookUpOnEdit = value;
        }

        public string ValueMember
        {
            get => mValueMember;
            set
            {
                mValueMember = value ?? string.Empty;
                UpdateLookupMode();
            }
        }

        public string DisplayMember
        {
            get => mDisplayMember;
            set
            {
                mDisplayMember = value ?? string.Empty;
                UpdateLookupMode();
            }
        }

        [Bindable(true)]
        public object Value
        {
            get => mValue;
            set
            {
                if (value == null || value == DBNull.Value)
                {
                    mValue = null;
                    Model = null;
                    ValidLookUp = false;
                    SetTextWithoutLookup(string.Empty);
                    ClearControls();
                    return;
                }

                if (Equals(mValue, value))
                    return;

                mValue = value;
                if (mSameColumn)
                {
                    SetTextWithoutLookup(Convert.ToString(value, System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty);
                }
                LookUp(false);
            }
        }

        public void SetFilter(Expression<Func<object, bool>> linqExpression) => _linqExpression = linqExpression;
        public void ClearFilter() => _linqExpression = null;
        public void SetupDefault() { }

        private void UpdateLookupMode()
        {
            mSameColumn = string.Equals(mValueMember, mDisplayMember, StringComparison.InvariantCultureIgnoreCase);
            mLookUpMode = mSameColumn ? LookUpModes.Standard : LookUpModes.Enhanced;
        }

        private void SetTextWithoutLookup(string text)
        {
            mLock = true;
            Text = text;
            mLock = false;
        }

        private void DbLookUpTextBoxN_TextChanged(object sender, EventArgs e)
        {
            if (!mLock && mLookUpOnEdit)
                LookUp(true);
        }

        private void DbLookUpTextBoxN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (mLookUpOnEdit)
                return;

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Tab)
            {
                LookUp(true);
            }
            else
            {
                ClearControls(true);
                Model = null;
                mValue = null;
                ValidLookUp = false;
            }
        }

        public void ClearControls(bool fromEditing = false)
        {
            foreach (var binding in DataBindControls.Values)
            {
                if (binding?.Control == null || string.IsNullOrWhiteSpace(binding.ControlPropertyName))
                    continue;

                var property = binding.Control.GetType().GetProperty(binding.ControlPropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null || !property.CanWrite)
                    continue;

                object defaultValue = property.PropertyType.IsValueType && Nullable.GetUnderlyingType(property.PropertyType) == null
                    ? Activator.CreateInstance(property.PropertyType)
                    : null;
                property.SetValue(binding.Control, defaultValue);
            }
        }

        public bool LookUp(bool fromEditing = false)
        {
            ValidLookUp = false;
            Model = null;
            ClearControls(fromEditing);

            if (string.IsNullOrWhiteSpace(ValueMember) || string.IsNullOrWhiteSpace(DisplayMember))
                return CompleteLookup(false);

            try
            {
                object source = DynamicDataSource != null ? DynamicDataSource() : DataSource;
                IEnumerable items = null;

                if (DynamicDataSource != null || DataSource != null)
                {
                    source = UnwrapExecutionResult(source);
                    items = source as IEnumerable;
                    if (items == null && source != null)
                        Model = ToModelDictionary(source);

                    if (items != null)
                    {
                        Type itemType = GetItemType(source);
                        var filter = _linqExpression;
                        if (filter == null && itemType != null && !string.IsNullOrWhiteSpace(fromEditing ? Text : Convert.ToString(mValue, System.Globalization.CultureInfo.CurrentCulture)))
                        {
                            string searchValue = fromEditing ? Text : Convert.ToString(mValue, System.Globalization.CultureInfo.CurrentCulture);
                            string member = fromEditing || !mSameColumn ? DisplayMember : ValueMember;
                            filter = BuildMemberFilter(itemType, member, searchValue);
                        }

                        foreach (var item in items)
                        {
                            if (item == null)
                                continue;
                            if (filter != null && !filter.Compile()(item))
                                continue;
                            Model = ToModelDictionary(item);
                            break;
                        }
                    }
                }
                else
                {
                    if (DbConnection == null)
                        throw new InvalidOperationException("DbConnection deve essere impostata per il lookup SQL.");
                    BuildSql(fromEditing);
                    Model = DbConnection.Query(SQLQuery, parameters).Cast<object>().Select(ToModelDictionary).FirstOrDefault();
                }

                if (Model == null)
                {
                    mValue = null;
                    if (!fromEditing)
                        SetTextWithoutLookup(string.Empty);
                    ClearControls(fromEditing);
                    return CompleteLookup(false);
                }

                if (!Model.TryGetValue(ValueMember, out var foundValue))
                    throw new InvalidOperationException($"La proprietà '{ValueMember}' non è presente nel risultato del lookup.");
                if (!Model.TryGetValue(DisplayMember, out var displayValue))
                    throw new InvalidOperationException($"La proprietà '{DisplayMember}' non è presente nel risultato del lookup.");

                mValue = foundValue == DBNull.Value ? null : foundValue;
                SetTextWithoutLookup(Convert.ToString(displayValue, System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty);
                ApplyBoundControls();
                ValidLookUp = true;
                if (fromEditing && mSameColumn)
                    mValue = Text;
                return CompleteLookup(true);
            }
            catch
            {
                Model = null;
                mValue = null;
                ValidLookUp = false;
                SetTextWithoutLookup(string.Empty);
                ClearControls(fromEditing);
                throw;
            }
        }

        private bool CompleteLookup(bool result)
        {
            eLookUp?.Invoke();
            LookUpCompleted?.Invoke(this, EventArgs.Empty);
            return result;
        }

        private void BuildSql(bool fromEditing)
        {
            parameters = new DynamicParameters();
            string member = fromEditing || !mSameColumn ? DisplayMember : ValueMember;
            string parameterName = "lookupValue";
            SQLQuery = $"SELECT {SelectClause} FROM {Passero.Framework.Utilities.GetModelTableName(ModelClass)} WHERE {member}=@{parameterName}";
            if (!string.IsNullOrWhiteSpace(WhereClause))
                SQLQuery += $" AND ({WhereClause})";

            foreach (var name in DbParameters.ParameterNames)
            {
                if (!string.Equals(name.TrimStart('@', ':', '?'), parameterName, StringComparison.OrdinalIgnoreCase))
                    parameters.Add(name, ((SqlMapper.IParameterLookup)DbParameters)[name]);
            }
            parameters.Add(parameterName, fromEditing ? Text : mValue ?? Text);
        }

        private object UnwrapExecutionResult(object result)
        {
            if (result == null)
                return null;

            var resultType = result.GetType();
            if (typeof(ExecutionResult).IsAssignableFrom(resultType))
            {
                var typedValue = resultType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
                return typedValue?.GetValue(result);
            }

            var valueProperty = resultType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            if (valueProperty != null && resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(ExecutionResult<>))
                return valueProperty.GetValue(result);

            return result;
        }

        private static Type GetItemType(object source)
        {
            if (source == null)
                return null;
            var enumerableType = source.GetType().GetInterfaces()
                .Concat(new[] { source.GetType() })
                .FirstOrDefault(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            return enumerableType?.GetGenericArguments()[0];
        }

        private static Expression<Func<object, bool>> BuildMemberFilter(Type itemType, string memberName, string value)
        {
            var member = itemType.GetProperty(memberName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (member == null)
                throw new InvalidOperationException($"La proprietà '{memberName}' non esiste nel tipo '{itemType.FullName}'.");

            var parameter = Expression.Parameter(typeof(object), "item");
            var cast = Expression.Convert(parameter, itemType);
            var property = Expression.Property(cast, member);
            var targetType = Nullable.GetUnderlyingType(member.PropertyType) ?? member.PropertyType;
            var converted = Convert.ChangeType(value, targetType, System.Globalization.CultureInfo.CurrentCulture);
            var constant = Expression.Constant(converted, targetType);
            Expression body = property.Type == targetType
                ? Expression.Equal(property, constant)
                : Expression.Equal(property, Expression.Convert(constant, property.Type));
            return Expression.Lambda<Func<object, bool>>(body, parameter);
        }

        private void ApplyBoundControls()
        {
            foreach (var binding in DataBindControls.Values)
            {
                if (binding?.Control == null || string.IsNullOrWhiteSpace(binding.ControlPropertyName))
                    continue;
                if (!Model.TryGetValue(binding.ModelPropertyName ?? string.Empty, out var value))
                    throw new InvalidOperationException($"La proprietà '{binding.ModelPropertyName}' non è presente nel modello del lookup.");

                var property = binding.Control.GetType().GetProperty(binding.ControlPropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null || !property.CanWrite)
                    throw new InvalidOperationException($"La proprietà '{binding.ControlPropertyName}' non è scrivibile sul controllo associato.");
                property.SetValue(binding.Control, ConvertForProperty(value == DBNull.Value ? null : value, property.PropertyType));
            }
        }

        private static object ConvertForProperty(object value, Type propertyType)
        {
            if (value == null)
                return propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null
                    ? Activator.CreateInstance(propertyType)
                    : null;
            var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            if (targetType.IsInstanceOfType(value))
                return value;
            if (targetType.IsEnum)
                return Enum.ToObject(targetType, value);
            return Convert.ChangeType(value, targetType, System.Globalization.CultureInfo.CurrentCulture);
        }

        private static IDictionary<string, object> ToModelDictionary(object item)
        {
            if (item is IDictionary<string, object> dictionary)
                return new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
            if (item is IDictionary nonGeneric)
            {
                var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                foreach (DictionaryEntry entry in nonGeneric)
                    result[Convert.ToString(entry.Key, System.Globalization.CultureInfo.InvariantCulture)] = entry.Value;
                return result;
            }
            return item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.CanRead && property.GetIndexParameters().Length == 0)
                .ToDictionary(property => property.Name, property => property.GetValue(item), StringComparer.OrdinalIgnoreCase);
        }

        private string GetBoundControlKey(Control control, string propertyName)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Nome proprietà obbligatorio.", nameof(propertyName));
            return $"{control.Name}|{propertyName.Trim()}";
        }

        public bool AddControl(Control control, string controlPropertyName, string modelPropertyName)
        {
            if (string.IsNullOrWhiteSpace(modelPropertyName))
                throw new ArgumentException("Nome proprietà modello obbligatorio.", nameof(modelPropertyName));
            string key = GetBoundControlKey(control, controlPropertyName);
            DataBindControls[key] = new DataBindControl(control, controlPropertyName, modelPropertyName);
            return true;
        }

        public int RemoveControl(Control control, string controlPropertyName) => RemoveControl(GetBoundControlKey(control, controlPropertyName));

        public int RemoveControl(string key)
        {
            return key != null && DataBindControls.Remove(key) ? 1 : 0;
        }

        public int RemoveControl(Control control)
        {
            if (control == null)
                return 0;
            string prefix = $"{control.Name}|";
            var keys = DataBindControls.Keys.Where(key => key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var key in keys)
                DataBindControls.Remove(key);
            return keys.Count;
        }
    }
}
