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
        public void To_Upper_Case(string script, string keyWord)
        {
            int startPos = 0;
            for (int i = 0; i < script.Length; i+=keyWord.Length+1)
            {
                startPos = script.IndexOf(keyWord.ToLower(), i);
                if (startPos < 0) 
                    break;
                script.Replace(script.Substring(startPos,keyWord.Length-1), keyWord.ToUpper());
               
            }


        }
    }
}
