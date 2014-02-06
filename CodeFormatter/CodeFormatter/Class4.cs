using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;

namespace CodeFormatter
{
    class SearchAndHighlight:Highlighter
    {

        List<Tag> m_tags = new List<Tag>();
        List<SysVar> sysVariables = new List<SysVar>();
        List<Comment> comments = new List<Comment>();
        List<SQLLabel> labels = new List<SQLLabel>();
        List<stringVar> strVar = new List<stringVar>();
        bool isComment = false;
        bool isVariable = false;

        public void GetWordFromRTB(RichTextBox rtb)
        {
            if (rtb.Document == null)
                return;

            TextRange documentRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            documentRange.ClearAllProperties();

            TextPointer navigator = rtb.Document.ContentStart;
            while (navigator.CompareTo(rtb.Document.ContentEnd) < 0)
            {
                TextPointerContext context = navigator.GetPointerContext(LogicalDirection.Backward);
                if (context == TextPointerContext.ElementStart && navigator.Parent is Run)
                {
                    CheckWordsInRun((Run)navigator.Parent);

                }
                navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
            }
            Format(comments, m_tags, labels, sysVariables, strVar);
        }

        void CheckWordsInRun(Run run)
        {
            string text = run.Text;
            int sIndex = 0;
            int eIndex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsWhiteSpace(text[i]) | SyntaxLibrary.GetSpecials.Contains(text[i]))
                {
                    if (i > 0 && !(Char.IsWhiteSpace(text[i - 1]) | SyntaxLibrary.GetSpecials.Contains(text[i - 1])))
                    {

                        eIndex = i - 1;
                        string word = text.Substring(sIndex, eIndex - sIndex + 1);
                        isComment = GetAllComment(comments, isComment, word, run, sIndex, text);
                        GetAllKeyWords(m_tags, word, sIndex, eIndex, run);
                        GetAllLabels(labels, word, sIndex, eIndex, run);
                        GetAllSysVariables(sysVariables, word, sIndex, eIndex, run);
                       // GetTextVariables(strVar,ref isVariable, word, sIndex, eIndex, run);
                    }
                    sIndex = i + 1;
                }
            }
            string lastWord = text.Substring(sIndex, text.Length - sIndex);
            if (isComment)
            {
                isComment = IfCommentEndsChecker(comments, lastWord, run, sIndex);
            }
            GetAllKeyWords(m_tags, text, lastWord, sIndex, eIndex, run);
            GetAllLabels(labels, lastWord, sIndex, eIndex, run);
            GetAllSysVariables(sysVariables, lastWord, sIndex, eIndex, run);
           // GetTextVariables(strVar, ref isVariable, lastWord, sIndex, eIndex, run);
        }

    }
}
