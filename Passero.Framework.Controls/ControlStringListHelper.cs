using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework.Controls
{
    /// <summary>
    /// Classe helper per la conversione di stringhe in liste e viceversa
    /// </summary>
    public static class StringListHelper
    {
        /// <summary>
        /// Converte una stringa separata da virgole in una lista di stringhe
        /// </summary>
        public static List<string> ConvertToList(string value)
        {
            List<string> result = new List<string>();
            if (!string.IsNullOrEmpty(value))
            {
                string[] items = value.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    result.Add(item.Trim());
                }
            }
            return result;
        }

        /// <summary>
        /// Converte una lista di stringhe in una stringa separata da virgole
        /// </summary>
        public static string ConvertToString(List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            return string.Join(",", list);
        }
    }
}
