using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public static class StringHelper
    {

        public static bool StringStartsWith(string inputstring, string[] strings, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase )
        {
            if (string.IsNullOrEmpty(inputstring))
                return false;
            foreach (string x in strings)
            {
                if (inputstring.StartsWith(x,comparison ))
                {
                    return true;
                }
            }
            return false;   
        }

        public static bool StringEndsWith(string inputstring, string[] strings, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            if (string.IsNullOrEmpty(inputstring))
                return false;

            foreach (string x in strings)
            {
                if (inputstring.EndsWith(x, comparison))
                {
                    return true;
                }
            }
            return false;
        }
        public static string SequenceFrom(string value)
        {
            char chr;
            int i = 0;
            int c = 0;
            for (i = value.Length; i >= 1; i -= 1)
            {
                c = Convert.ToInt32(value[i - 1]);
                if (c < 48 ? true : c > 57)
                {
                    if (c < 65 ? false : c <= 90)
                    {
                        if (c < 65 ? false : c <= 89)
                        {
                            c += 1;
                            string str = value.Remove(i - 1, 1);
                            chr = Strings.ChrW(c);
                            value = str.Insert(i - 1, chr.ToString());
                            break;
                        }
                        else if (c == 90)
                        {
                            c = 65;
                            string str1 = value.Remove(i - 1, 1);
                            chr = Strings.ChrW(c);
                            value = str1.Insert(i - 1, chr.ToString());
                        }
                    }
                }
                else if (c < 48 ? false : c <= 56)
                {
                    c += 1;
                    string str2 = value.Remove(i - 1, 1);
                    chr = Strings.ChrW(c);
                    value = str2.Insert(i - 1, chr.ToString());
                    break;
                }
                else if (c == 57)
                {
                    c = 48;
                    string str3 = value.Remove(i - 1, 1);
                    chr = Strings.ChrW(c);
                    value = str3.Insert(i - 1, chr.ToString());
                }
            }
            return value;
        }

        public static string SequenceFrom(string value, string SequenceMask)
        {
            char chr;
            bool flag;
            int x = 0;
            int i = 0;
            int c = 0;
            string a = null;
            x = value.Length;
            SequenceMask = SequenceMask.ToUpper();
            if (value.Length < SequenceMask.Length)
            {
                value = new string(' ', SequenceMask.Length);
            }
            for (i = x; i >= 1; i -= 1)
            {
                a = SequenceMask.Substring(i - 1, 1);
                if (!Equals(a, "F"))
                {
                    c = Convert.ToInt32(value[i - 1]);
                    string str = a;
                    if (!Equals(str, null))
                    {
                        if (!Equals(str, "0"))
                        {
                            goto Label2;
                        }
                        if (c < 48 ? true : c > 56)
                        {
                            if (c != 57)
                            {
                                c = 48;
                                string str1 = value.Remove(i - 1, 1);
                                chr = Strings.ChrW(c);
                                value = str1.Insert(i - 1, chr.ToString());
                            }
                            else
                            {
                                c = 48;
                                string str2 = value.Remove(i - 1, 1);
                                chr = Strings.ChrW(c);
                                value = str2.Insert(i - 1, chr.ToString());
                            }
                            goto Label0;
                        }
                        else
                        {
                            c += 1;
                            string str3 = value.Remove(i - 1, 1);
                            chr = Strings.ChrW(c);
                            value = str3.Insert(i - 1, chr.ToString());
                            break;
                        }
                    }

                Label2:
                    ;

                    flag = c < 65 ? false : c <= 89;
                    if (flag)
                    {
                        c += 1;
                        string str4 = value.Remove(i - 1, 1);
                        chr = Strings.ChrW(c);
                        value = str4.Insert(i - 1, chr.ToString());
                        break;
                    }
                    else if (c != 90)
                    {
                        c = 65;
                        string str5 = value.Remove(i - 1, 1);
                        chr = Strings.ChrW(c);
                        value = str5.Insert(i - 1, chr.ToString());
                    }
                    else
                    {
                        c = 65;
                        string str6 = value.Remove(i - 1, 1);
                        chr = Strings.ChrW(c);
                        value = str6.Insert(i - 1, chr.ToString());
                    }

                Label0:
                    ;

                }
            }
            return value;
        }


        public static string ValueSubString(string Value, int StartPos, int EndPos)
        {
            string str;
            if (StartPos < 1)
            {
                str = "";
            }
            else if (EndPos < 1)
            {
                str = "";
            }
            else if (EndPos >= StartPos)
            {
                if (EndPos > Value.Length)
                {
                    EndPos = Value.Length;
                }
                str = StartPos <= Value.Length ? Value.Substring(StartPos - 1, EndPos - StartPos + 1) : "";
            }
            else
            {
                str = "";
            }
            return str;
        }


        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another 
        /// specified string according the type of search to use for the specified string.
        /// </summary>
        /// <paramname="str">The string performing the replace method.</param>
        /// <paramname="oldValue">The string to be replaced.</param>
        /// <paramname="newValue">The string replace all occurrences of <paramrefname="oldValue"/>. 
        /// If value is equal to <c>null</c>, than all occurrences of <paramrefname="oldValue"/> will be removed from the <paramrefname="str"/>.</param>
        /// <paramname="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>A string that is equivalent to the current string except that all instances of <paramrefname="oldValue"/> are replaced with <paramrefname="newValue"/>. 
        /// If <paramrefname="oldValue"/> is not found in the current instance, the method returns the current instance unchanged.</returns>
        [DebuggerStepThrough]
        public static string StringReplace(this string str, string oldValue, string newValue, StringComparison comparisonType)
        {

            // Check inputs.
            if (Equals(str, null))
            {
                // Same as original .NET C# string.Replace behavior.
                throw new ArgumentNullException(nameof(str));
            }
            if (Equals(oldValue, null))
            {
                // Same as original .NET C# string.Replace behavior.
                throw new ArgumentNullException(nameof(oldValue));
            }
            if (oldValue.Length == 0)
            {
                // Same as original .NET C# string.Replace behavior.
                throw new ArgumentException("String cannot be of zero length.");
            }
            if (str.Length == 0)
            {
                // Same as original .NET C# string.Replace behavior.
                return str;
            }


            // if (oldValue.Equals(newValue, comparisonType))
            // {
            // This condition has no sense
            // It will prevent method from replacesing: "Example", "ExAmPlE", "EXAMPLE" to "example"
            // return str;
            // }



            // Prepare string builder for storing the processed string.
            // Note: StringBuilder has a better performance than String by 30-40%.
            var resultStringBuilder = new StringBuilder(str.Length);



            // Analyze the replacement: replace or remove.
            bool isReplacementNullOrEmpty = string.IsNullOrEmpty(newValue);



            // Replace all values.
            const int valueNotFound = -1;
            var foundAt = default(int);
            int startSearchFromIndex = 0;
            while (Utilities.Assign(ref foundAt, str.IndexOf(oldValue, startSearchFromIndex, comparisonType)) != valueNotFound)
            {

                // Append all characters until the found replacement.
                int charsUntilReplacment = foundAt - startSearchFromIndex;
                bool isNothingToAppend = charsUntilReplacment == 0;
                if (!isNothingToAppend)
                {
                    resultStringBuilder.Append(str, startSearchFromIndex, charsUntilReplacment);
                }



                // Process the replacement.
                if (!isReplacementNullOrEmpty)
                {
                    resultStringBuilder.Append(newValue);
                }


                // Prepare start index for the next search.
                // This needed to prevent infinite loop, otherwise method always start search 
                // from the start of the string. For example: if an oldValue == "EXAMPLE", newValue == "example"
                // and comparisonType == "any ignore case" will conquer to replacing:
                // "EXAMPLE" to "example" to "example" to "example" … infinite loop.
                startSearchFromIndex = foundAt + oldValue.Length;
                if (startSearchFromIndex == str.Length)
                {
                    // It is end of the input string: no more space for the next search.
                    // The input string ends with a value that has already been replaced. 
                    // Therefore, the string builder with the result is complete and no further action is required.
                    return resultStringBuilder.ToString();
                }
            }


            // Append the last part to the result.
            int charsUntilStringEnd = str.Length - startSearchFromIndex;
            resultStringBuilder.Append(str, startSearchFromIndex, charsUntilStringEnd);


            return resultStringBuilder.ToString();

        }

    }

}
