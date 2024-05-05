using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Passero.Framework
{

    public enum ConfigurationSyntax
    {
        INI = 0,
        JSON = 1,
        XML = 2
    }

    public class ConfigurationDictionary
    {
        public Dictionary<string, Dictionary<string, string>> Items { get; set; } = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
    }

    public class ConfigurationManager
    {
        public ConfigurationSyntax Syntax { get; set; } = ConfigurationSyntax.INI;
        public string FileName { get; set; }
        public string JsonConfigurationString { get; set; }
        public string XMLConfigurationString { get; set; }

        public Dictionary<string, System.Data.Common.DbConnection> DBConnections { get; set; } = new Dictionary<string, System.Data.Common.DbConnection>(StringComparer.InvariantCultureIgnoreCase);
        private Dictionary<string, Dictionary<string, string>> ConfigurationDictionary { get; set; } = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        // Public Property SessionConfigurationDictionary As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(StringComparer.InvariantCultureIgnoreCase)
        private Dictionary<string, Dictionary<string, string>> SessionConfigurationDictionary { get; set; } = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);


        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult();

        public ConfigurationManager()
        {

        }
        public ConfigurationManager(string FileName = "")
        {

            this.FileName = FileName;

        }

        public Dictionary<string, Dictionary<string, string>> GetConfigurationDictionary()
        {
            return ConfigurationDictionary;
        }

        public void SetConfigurationDictionary(Dictionary<string, Dictionary<string, string>> ConfigurationDictionary)
        {
            this.ConfigurationDictionary = ConfigurationDictionary;
        }

        public Dictionary<string, Dictionary<string, string>> GetSessionConfigurationDictionary()
        {
            return SessionConfigurationDictionary;
        }

        public void GetSessionConfigurationDictionary(Dictionary<string, Dictionary<string, string>> SessionConfigurationDictionary)
        {
            this.SessionConfigurationDictionary = SessionConfigurationDictionary;
        }


        public string ReadJsonConfigurationFile()
        {

            LastExecutionResult = new ExecutionResult("Passero.Framework.ConfigurationManager:ReadConfigurationFile()");

            if (System.IO.File.Exists(FileName) == false)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
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
                    LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                    LastExecutionResult.ResultMessage = $"Il file {FileName} non contiene valori JSON!";
                }
            }

            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 3;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult.ResultMessage = ex.Message;
            }
            return JsonConfigurationString;

        }

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
                    LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                    LastExecutionResult.ResultMessage = $"La configurazione letta non è una stringa JSON valida!";

                }
            }

            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 2;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult.ResultMessage = ex.Message;
            }

            return LastExecutionResult.Success;

        }

        public static T __Assign<T>(ref T target, T value)
        {
            target = value;
            return value;
        }
        public bool ReadConfiguration(string FileName = "")
        {

            LastExecutionResult = new ExecutionResult("Passero.Common.ConfigurationManager:ReadConfiguration()");

            string _FileName = this.FileName;

            if (!string.IsNullOrEmpty(FileName.Trim()))
            {
                _FileName = FileName;
            }

            if (System.IO.File.Exists(this.FileName) == false)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult.ResultMessage = $"Il file {_FileName} non esiste!";
                return false;
            }

            try
            {
                ConfigurationDictionary.Clear();
                string[] content = System.IO.File.ReadAllLines(_FileName);
                Dictionary<string, string> CurrentSection = null;
                string CurrentSectionName = null;

                for (int i = 1, loopTo = content.Length + 1 - 1; i <= loopTo; i++)
                {
                    string s = content[i - 1].Trim();
                    if (s.StartsWith("["))
                    {
                        CurrentSectionName = s.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        if (ConfigurationDictionary.ContainsKey(CurrentSectionName))
                        {
                            CurrentSection = ConfigurationDictionary[CurrentSectionName];
                        }
                        else
                        {
                            ConfigurationDictionary.Add(CurrentSectionName, new Dictionary<string, string>());
                            CurrentSection = ConfigurationDictionary[CurrentSectionName];
                        }
                    }
                    else if (CurrentSection is not null && !string.IsNullOrWhiteSpace(s) && !s.StartsWith("#") && !s.StartsWith(";"))
                    {
                        // Dim res = s.Split("=", 2).[Select](Function(x) x.Trim()).ToArray()
                        string[] res = s.Split(new char[] { '=' }, 2);
                        CurrentSection[res[0]] = res[1];
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
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult.ResultMessage = ex.Message;
            }

            return LastExecutionResult.Success;

        }

        public string GetConfigurationKeyValue(string Section, string Key)
        {
            LastExecutionResult = new ExecutionResult($"Passero.Common.ConfigurationManager.GetConfigurationValue({Section},{Key}");

            bool ok = false;
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
                        LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                        LastExecutionResult.ResultMessage = $"La chiave {Key} non esiste nella Sezione {Section} del file {FileName}";
                    }
                }
                else
                {
                    LastExecutionResult.ErrorCode = 2;
                    LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                    LastExecutionResult.ResultMessage = $"La Sezione {Section} non esiste nel file {FileName}";
                }
            }



            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante la lettura del file {FileName}{Constants.vbCrLf}{ex.Message}";
            }

            return value;
        }


        public string GetSessionConfigurationKeyValue(string Section, string Key)
        {
            LastExecutionResult = new ExecutionResult($"Passero.Common.ConfigurationManager.GetSessionConfigurationValue({Section},{Key}");

            bool ok = false;
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
                        LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                        LastExecutionResult.ResultMessage = $"La chiave {Key} non esiste nella Sezione {Section} del dizionario di sessione.";
                    }
                }
                else
                {
                    LastExecutionResult.ErrorCode = 2;
                    LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                    LastExecutionResult.ResultMessage = $"La Sezione {Section} non esiste nel del dizionario di sessione.";
                }
            }



            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante la lettura del dizionario di sessione.{Constants.vbCrLf}{ex.Message}";
            }

            return value;
        }



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
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante aggiunta sezione {Section}{Constants.vbCrLf}{ex.Message}";
            }

            return ok;



        }

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
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante rimozione sezione {Section}{Constants.vbCrLf}{ex.Message}";
            }

            return ok;



        }

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
                        LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
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
                        LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                        LastExecutionResult.ResultMessage = $"La chiave {Key} della sezione {Section} non esiste del dizionario di sessione.";
                        return false;
                    }
                }
            }


            catch (Exception ex)
            {
                LastExecutionResult.ErrorCode = 3;
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult.ResultMessage = $"Errore durante modifica del dizionario di sessione, sezione {Section} chiave {Key}.{Constants.vbCrLf}{ex.Message}";
            }

            return ok;
        }

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