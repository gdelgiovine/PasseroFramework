using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Passero.Framework
{

    /// <summary>
    /// 
    /// </summary>
    public enum ConfigurationSyntax
    {
        /// <summary>
        /// The ini
        /// </summary>
        INI = 0,
        /// <summary>
        /// The json
        /// </summary>
        JSON = 1,
        /// <summary>
        /// The XML
        /// </summary>
        XML = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationDictionary
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public Dictionary<string, Dictionary<string, string>> Items { get; set; } = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// Gets or sets the syntax.
        /// </summary>
        /// <value>
        /// The syntax.
        /// </value>
        public ConfigurationSyntax Syntax { get; set; } = ConfigurationSyntax.INI;
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the json configuration string.
        /// </summary>
        /// <value>
        /// The json configuration string.
        /// </value>
        public string JsonConfigurationString { get; set; }
        /// <summary>
        /// Gets or sets the XML configuration string.
        /// </summary>
        /// <value>
        /// The XML configuration string.
        /// </value>
        public string XMLConfigurationString { get; set; }

        /// <summary>
        /// Gets or sets the database connections.
        /// </summary>
        /// <value>
        /// The database connections.
        /// </value>
        public Dictionary<string, System.Data.IDbConnection> DBConnections { get; set; } = new Dictionary<string, System.Data.IDbConnection>(StringComparer.InvariantCultureIgnoreCase);

        //public Dictionary<string, System.Data.Common.DbConnection> DBConnections { get; set; } = new Dictionary<string, System.Data.Common.DbConnection>(StringComparer.InvariantCultureIgnoreCase);
        /// <summary>
        /// Gets or sets the configuration dictionary.
        /// </summary>
        /// <value>
        /// The configuration dictionary.
        /// </value>
        private Dictionary<string, Dictionary<string, string>> ConfigurationDictionary { get; set; } = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        // Public Property SessionConfigurationDictionary As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(StringComparer.InvariantCultureIgnoreCase)
        /// <summary>
        /// Gets or sets the session configuration dictionary.
        /// </summary>
        /// <value>
        /// The session configuration dictionary.
        /// </value>
        private Dictionary<string, Dictionary<string, string>> SessionConfigurationDictionary { get; set; } = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);


        /// <summary>
        /// Gets or sets the last execution result.
        /// </summary>
        /// <value>
        /// The last execution result.
        /// </value>
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationManager"/> class.
        /// </summary>
        public ConfigurationManager()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationManager"/> class.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        public ConfigurationManager(string FileName = "")
        {

            this.FileName = FileName;

        }

        /// <summary>
        /// Gets the configuration dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetConfigurationDictionary()
        {
            return ConfigurationDictionary;
        }

        /// <summary>
        /// Sets the configuration dictionary.
        /// </summary>
        /// <param name="ConfigurationDictionary">The configuration dictionary.</param>
        public void SetConfigurationDictionary(Dictionary<string, Dictionary<string, string>> ConfigurationDictionary)
        {
            this.ConfigurationDictionary = ConfigurationDictionary;
        }

        /// <summary>
        /// Gets the session configuration dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetSessionConfigurationDictionary()
        {
            return SessionConfigurationDictionary;
        }

        /// <summary>
        /// Gets the session configuration dictionary.
        /// </summary>
        /// <param name="SessionConfigurationDictionary">The session configuration dictionary.</param>
        public void GetSessionConfigurationDictionary(Dictionary<string, Dictionary<string, string>> SessionConfigurationDictionary)
        {
            this.SessionConfigurationDictionary = SessionConfigurationDictionary;
        }


        /// <summary>
        /// Reads the json configuration file.
        /// </summary>
        /// <returns></returns>
        public string ReadJsonConfigurationFile()
        {

            LastExecutionResult = new ExecutionResult("Passero.Framework.ConfigurationManager:ReadConfigurationFile()");

            if (System.IO.File.Exists(FileName) == false)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = $"Il file {FileName} non esiste!";
                return "";
            }

            string s;

            try
            {
                s = System.IO.File.ReadAllText(FileName);
                if (Utilities.IsValidJson(s) == true)
                {
                    JsonConfigurationString = s;
                }
                else
                {
                    LastExecutionResult.ErrorCode = 2;
                    LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                    LastExecutionResult.ResultMessage = $"Il file {FileName} non contiene valori JSON!";
                }
            }

            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 3;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = ex.Message;
            }
            return JsonConfigurationString;

        }

        /// <summary>
        /// Writes the json configuration file.
        /// </summary>
        /// <param name="AsFileName">Name of as file.</param>
        /// <returns></returns>
        public bool WriteJsonConfigurationFile(string AsFileName = "")
        {

            LastExecutionResult = new ExecutionResult("Passero.Common.ConfigurationManager:WriteConfigurationFile()");

            string _FileName = FileName;
            if (!string.IsNullOrEmpty(AsFileName.Trim()))
            {
                _FileName = AsFileName;
            }

            try
            {
                if (Utilities.IsValidJson(JsonConfigurationString) == true)
                {
                    System.IO.File.WriteAllText(_FileName, JsonConfigurationString);
                }
                else
                {
                    LastExecutionResult.ErrorCode = 1;
                    LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                    LastExecutionResult.ResultMessage = $"La configurazione letta non è una stringa JSON valida!";

                }
            }

            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 2;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = ex.Message;
            }

            return LastExecutionResult.Success;

        }

        /// <summary>
        /// Assigns the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T __Assign<T>(ref T target, T value)
        {
            target = value;
            return value;
        }
        /// <summary>
        /// Reads the configuration.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <returns></returns>
        public bool ReadConfiguration(string FileName = "")
        {

            LastExecutionResult = new ExecutionResult("Passero.Common.ConfigurationManager:ReadConfiguration()");

            string _FileName = this.FileName;

            if (!string.IsNullOrEmpty(FileName.Trim()))
            {
                this.FileName = FileName;
            }

            if (System.IO.File.Exists(this.FileName) == false)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = $"Il file {this.FileName} non esiste!";
                return false;
            }

            try
            {
                ConfigurationDictionary.Clear();
                string[] content = System.IO.File.ReadAllLines(this.FileName);
                Dictionary<string, string> CurrentSection = null;
                string CurrentSectionName = null;

                for (int i = 1, loopTo = content.Length + 1 - 1; i <= loopTo; i++)
                {
                    string s = content[i - 1].Trim();
                    if (s.StartsWith("["))
                    {
                        CurrentSectionName = s.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        CurrentSectionName = CurrentSectionName.Trim(); 
                        if (ConfigurationDictionary.ContainsKey(CurrentSectionName))
                        {
                            CurrentSection = ConfigurationDictionary[CurrentSectionName];
                        }
                        else
                        {
                            ConfigurationDictionary.Add(CurrentSectionName.Trim(), new Dictionary<string, string>());
                            CurrentSection = ConfigurationDictionary[CurrentSectionName];
                        }
                    }
                    else if (CurrentSection is not null && !string.IsNullOrWhiteSpace(s) && !s.StartsWith("#") && !s.StartsWith(";"))
                    {
                        // Dim res = s.Split("=", 2).[Select](Function(x) x.Trim()).ToArray()
                        string[] res = s.Split(new char[] { '=' }, 2);
                        CurrentSection[res[0].Trim() ] = res[1].Trim ();
                    }
                    else
                    {
                    }
                }
            }


            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 2;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = ex.Message;
            }

            return LastExecutionResult.Success;

        }

        /// <summary>
        /// Gets the configuration key value.
        /// </summary>
        /// <param name="Section">The section.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        public string GetConfigurationKeyValue(string Section, string Key)
        {
            LastExecutionResult = new ExecutionResult($"Passero.Common.ConfigurationManager.GetConfigurationValue({Section},{Key}");

#pragma warning disable CS0219 // La variabile è assegnata, ma il suo valore non viene mai usato
            bool ok = false;
#pragma warning restore CS0219 // La variabile è assegnata, ma il suo valore non viene mai usato
            string value = "";
            try
            {
                if (ConfigurationDictionary.ContainsKey(Section))
                {
                    if (ConfigurationDictionary[Section].ContainsKey(Key))
                    {
                        value = ConfigurationDictionary[Section][Key];
                        ok = true;
                    }
                    else
                    {
                        LastExecutionResult.ErrorCode = 3;
                        LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                        LastExecutionResult.ResultMessage = $"La chiave {Key} non esiste nella Sezione {Section} del file {FileName}";
                    }
                }
                else
                {
                    LastExecutionResult.ErrorCode = 2;
                    LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                    LastExecutionResult.ResultMessage = $"La Sezione {Section} non esiste nel file {FileName}";
                }
            }



            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante la lettura del file {FileName}{Constants.vbCrLf}{ex.Message}";
            }

            return value;
        }


        /// <summary>
        /// Gets the session configuration key value.
        /// </summary>
        /// <param name="Section">The section.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        public string GetSessionConfigurationKeyValue(string Section, string Key)
        {
            LastExecutionResult = new ExecutionResult($"Passero.Common.ConfigurationManager.GetSessionConfigurationValue({Section},{Key}");

#pragma warning disable CS0219 // La variabile è assegnata, ma il suo valore non viene mai usato
            bool ok = false;
#pragma warning restore CS0219 // La variabile è assegnata, ma il suo valore non viene mai usato
            string value = "";
            try
            {
                if (SessionConfigurationDictionary.ContainsKey(Section))
                {
                    if (SessionConfigurationDictionary[Section].ContainsKey(Key))
                    {
                        value = SessionConfigurationDictionary[Section][Key];
                        ok = true;
                    }
                    else
                    {
                        LastExecutionResult.ErrorCode = 3;
                        LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                        LastExecutionResult.ResultMessage = $"La chiave {Key} non esiste nella Sezione {Section} del dizionario di sessione.";
                    }
                }
                else
                {
                    LastExecutionResult.ErrorCode = 2;
                    LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                    LastExecutionResult.ResultMessage = $"La Sezione {Section} non esiste nel del dizionario di sessione.";
                }
            }



            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante la lettura del dizionario di sessione.{Constants.vbCrLf}{ex.Message}";
            }

            return value;
        }



        /// <summary>
        /// Adds the session configuration section.
        /// </summary>
        /// <param name="Section">The section.</param>
        /// <returns></returns>
        public bool AddSessionConfigurationSection(string Section)
        {
            LastExecutionResult = new ExecutionResult($"Passero.Common.ConfigurationManager.AddSessionConfigurationSection({Section}");
            bool ok = false;

            try
            {
                // If Me.SessionConfigurationDictionary.ContainsKey(Section) = False Then
                // Me.SessionConfigurationDictionary.Add(Section, New Dictionary(Of String, String))
                // End If
                SessionConfigurationDictionary[Section] = new Dictionary<string, string>();
                return ok;
            }
            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante aggiunta sezione {Section}{Constants.vbCrLf}{ex.Message}";
            }

            return ok;



        }

        /// <summary>
        /// Removes the session configuration section.
        /// </summary>
        /// <param name="Section">The section.</param>
        /// <returns></returns>
        public bool RemoveSessionConfigurationSection(string Section)
        {
            LastExecutionResult = new ExecutionResult($"Passero.Common.ConfigurationManager.AddSessionConfigurationSection({Section}");
            bool ok = false;

            try
            {
                if (SessionConfigurationDictionary.ContainsKey(Section))
                {
                    SessionConfigurationDictionary.Remove(Section);
                    return ok;
                }
            }

            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante rimozione sezione {Section}{Constants.vbCrLf}{ex.Message}";
            }

            return ok;



        }

        /// <summary>
        /// Sets the session configuration key value.
        /// </summary>
        /// <param name="Section">The section.</param>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        /// <param name="AutoCreation">if set to <c>true</c> [automatic creation].</param>
        /// <returns></returns>
        public bool SetSessionConfigurationKeyValue(string Section, string Key, object Value, bool AutoCreation = true)
        {
            LastExecutionResult = new ExecutionResult($"Passero.Common.ConfigurationManager.SetSessionConfigurationKeyValue({Section},{Key}");

            bool ok = false;

            try
            {

                if (SessionConfigurationDictionary.ContainsKey(Section) == false)
                {
                    if (AutoCreation == true)
                    {
                        SessionConfigurationDictionary.Add(Section, new Dictionary<string, string>());
                    }
                    else
                    {
                        LastExecutionResult.ErrorCode = 1;
                        LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                        LastExecutionResult.ResultMessage = $"La sezione {Section} non esiste del dizionario di sessione.";
                        return false;
                    }
                }

                if (SessionConfigurationDictionary.ContainsKey(Section))
                {

                    if (ConfigurationDictionary[Section].ContainsKey(Key))
                    {
                        SessionConfigurationDictionary[Section][Key] = Value.ToString();
                        ok = true;
                    }
                    else if (AutoCreation == true)
                    {
                        SessionConfigurationDictionary[Section].Add(Key, Value.ToString());
                        ok = true;
                    }
                    else
                    {
                        LastExecutionResult.ErrorCode = 2;
                        LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                        LastExecutionResult.ResultMessage = $"La chiave {Key} della sezione {Section} non esiste del dizionario di sessione.";
                        return false;
                    }
                }
            }


            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 3;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante modifica del dizionario di sessione, sezione {Section} chiave {Key}.{Constants.vbCrLf}{ex.Message}";
            }

            return ok;
        }

        /// <summary>
        /// Adds the session configuration key value.
        /// </summary>
        /// <param name="Section">The section.</param>
        /// <param name="Key">The key.</param>
        /// <param name="Value">The value.</param>
        /// <returns></returns>
        public bool AddSessionConfigurationKeyValue(string Section, string Key, object Value)
        {
            LastExecutionResult = new ExecutionResult($"Passero.Common.ConfigurationManager.AddSessionConfigurationKeyValue({Section},{Key}");

            bool ok = false;
            ok = SetSessionConfigurationKeyValue(Section, Key, Value, true);
            LastExecutionResult.Context = $"Passero.Common.ConfigurationManager.AddSessionConfigurationKeyValue({Section},{Key}";
            return ok;

        }

    }
}