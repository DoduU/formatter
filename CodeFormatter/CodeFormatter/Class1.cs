using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFormatter
{
    public class Formatting_Methods
    {
        private string script;
        private int Get_keyWord_lastIndex(int index, string keyWord)
        {
            return script.IndexOf(keyWord, index) + keyWord.Length;

        }

        private int Get_keyWord_firstIndex(int index, string keyWord)
        {
            return script.IndexOf(keyWord, index);
        }
        public string To_Upper_Case(string script)
        {
            string key;
            int startPos = 0;
            string[] clausesSQL = { "declare", "FROM", "SELECT", "WHERE", "GROUP BY", "ORDER BY", "TOP", "HAVING" };
            foreach (string keyWord in clausesSQL)
            {
                for (int i = 0; i < script.Length; i += keyWord.Length + 1)
                {
                    startPos = script.IndexOf(keyWord.ToLower(), i);
                    if (startPos < 0)
                        break;
                    key = keyWord.ToUpper();
                    script = script.Replace(script.Substring(startPos, keyWord.Length), key);
                }
            }
            return script;
        }

        private string To_New_Line(string needNewLine)
        {
            return System.Environment.NewLine + needNewLine;
        }
        private string Pad_Generator(int padNum)
        {
            string padStr = " ";
            for (int i = 0; i < padNum; i++)
            {
                padStr = padStr + " ";
            }
            return padStr;
        }

    }
}
