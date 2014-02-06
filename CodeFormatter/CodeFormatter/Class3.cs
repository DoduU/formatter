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
    class Highlighter
    {
        #region structures
        public new struct SysVar
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }

        public new struct stringVar
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }

        public new struct SQLLabel
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }

        public new struct Comment
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }

        public new struct Tag
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }
        Regex reg = new Regex(@"(\w{0,})'\w{0,}'(?!\s|;)");
        #endregion
        #region Get script elements
        #region Get comments
        public bool GetAllComment(List<Comment> comments,bool isComment, string word, Run run, int sIndex, string text)
        {
            if (!isComment)
            {
                isComment = IfCommentStartsChecker(word);
            }
            if (isComment)
            {
                isComment = IfCommentEndsChecker(comments, word, run, sIndex);
            }
            HighlightComment(isComment, comments, word, run, sIndex);
            HighLightSingleLineComment(comments, word, text, run, sIndex);
            return isComment;
        }

        bool IfCommentStartsChecker(string word)
        {
            if (word.Contains("/*"))
            {
                return true;
            }
            return false;
        }


        public bool IfCommentEndsChecker(List<Comment> comments, string word, Run run, int sIndex)
        {
            if (word.Contains(@"*/"))
            {
                string comment = word;
                Comment comm = new Comment();
                comm.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                comm.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + comment.Length + 1, LogicalDirection.Backward);
                comm.Word = comment;
                comments.Add(comm);
                return false;
            }
            return true;
        }

        void HighlightComment(bool isComment, List<Comment> comments, string word, Run run, int sIndex)
        {
            if (isComment)
            {
                string comment = word;
                Comment comm = new Comment();
                comm.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                comm.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + comment.Length + 1, LogicalDirection.Backward);
                comm.Word = comment;
                comments.Add(comm);
            }
        }
        void HighLightSingleLineComment(List<Comment> comments, string word, string text, Run run, int sIndex)
        {
            if (word.Contains("--"))
            {
                string comment = text.Substring(sIndex);
                Comment comm = new Comment();
                comm.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                comm.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + comment.Length + 1, LogicalDirection.Backward);
                comm.Word = comment;
                comments.Add(comm);
            }
        }
        #endregion

        public void GetAllKeyWords(List<Tag> m_tags, string text, string word, int sIndex, int eIndex, Run run)
        {
            if (word.Length != 0)
            {
                if (SyntaxLibrary.IsKnownTag(word))
                {
                    Tag t = new Tag();
                    t.StartPosition = run.ContentStart.GetPositionAtOffset(text.IndexOf(word), LogicalDirection.Forward);
                    t.EndPosition = run.ContentStart.GetPositionAtOffset(text.IndexOf(word) + word.Length + 1, LogicalDirection.Backward);
                    t.Word = word;
                    m_tags.Add(t);
                }
            }
        }

        public void GetAllKeyWords(List<Tag> m_tags,  string word, int sIndex, int eIndex, Run run)
        {
            if (word.Length != 0)
            {
                if (SyntaxLibrary.IsKnownTag(word))
                {
                    Tag t = new Tag();
                    t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                    t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                    t.Word = word;
                    m_tags.Add(t);
                }
            }
        }
        public void GetAllLabels(List<SQLLabel> labels, string lastWord, int sIndex, int eIndex, Run run)
        {
            if (lastWord.Length != 0)
            {
                if (lastWord.Substring(lastWord.Length - 1).Contains(':'))
                {
                    SQLLabel t = new SQLLabel();
                    t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                    t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + lastWord.Length, LogicalDirection.Backward);
                    t.Word = lastWord;
                    labels.Add(t);
                }
            }
        }

        public void GetAllSysVariables(List<SysVar> sysVariables, string word, int sIndex, int eIndex, Run run)
        {
            if (word.Contains("@@"))
            {
                SysVar sysVar = new SysVar();
                sysVar.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                sysVar.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                sysVar.Word = word;
                sysVariables.Add(sysVar);
            }
        }
        #region Get all text variables
        public void GetTextVariables(List<stringVar> strVar, ref bool isVariable, string word, int sIndex, int eIndex, Run run)
        {
            bool enter  = false;
                IfTextVarStarts(ref enter, ref isVariable, word);

                IfTextVar(strVar, ref isVariable, word, sIndex, eIndex, run);
                if (reg.Match(word).Success)
                {
                    isVariable = false;
                }
                if (!enter)
                {
                    IfTextVarEnds(strVar, ref isVariable, word, sIndex, eIndex, run);
                }
        }
        private void IfTextVarStarts(ref bool enter, ref bool isVariable, string word)
        {
            if (word.Contains(@"'") && !isVariable)
            {
                isVariable = true;
                enter = true;
            }    

        }
        private void IfTextVarEnds(List<stringVar> strVar, ref bool isVariable, string word, int sIndex, int eIndex, Run run)
        {
            if (word.Contains(@"'") && isVariable)
            {
                string variable = word;
                stringVar var = new stringVar();
                var.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                var.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + word.Length, LogicalDirection.Backward);
                var.Word = word;
                strVar.Add(var);
                isVariable = false;
            }
        }
        private void IfTextVar(List<stringVar> strVar,ref bool isVariable,string word, int sIndex, int eIndex, Run run)
        {
            if (isVariable)
            {
                string variable = word;
                stringVar var = new stringVar();
                var.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                var.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + word.Length, LogicalDirection.Backward);
                var.Word = word;
                strVar.Add(var);
            }
        }
       
        #endregion




        #endregion

        #region Format method
        public void Format(List<Comment> comments, List<Tag> m_tags, List<SQLLabel> labels, List<SysVar> sysVariables, List<stringVar> strVar)
        {
            ApplyPropertyToKeyWords(m_tags);
            ApplyPropertyToLabels(labels);
            ApplyPropertyToComment(comments);
            ApplyPropertyToSysVar(sysVariables);
            ApplyPropertyToStrVar(strVar);

        }
        #endregion 

        #region ApplyProperty methods
        void ApplyPropertyToComment(List<Comment> comments)
        {
          for (int i = 0; i < comments.Count; i++)
            {
                TextRange range = new TextRange(comments[i].StartPosition, comments[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Green));
            }
          comments.Clear();
        }

        void ApplyPropertyToLabels(List<SQLLabel> labels)
        {
            for (int i = 0; i < labels.Count; i++)
            {
                TextRange range = new TextRange(labels[i].StartPosition, labels[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
            }
            labels.Clear();
        }

        void ApplyPropertyToStrVar(List<stringVar> strVar)
        {
            for (int i = 0; i < strVar.Count; i++)
            {
                TextRange range = new TextRange(strVar[i].StartPosition, strVar[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
            }
            strVar.Clear();
        }

        void ApplyPropertyToKeyWords(List<Tag> m_tags)
        {
            for (int i = 0; i < m_tags.Count; i++)
            {
                TextRange range = new TextRange(m_tags[i].StartPosition, m_tags[i].EndPosition);
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
                range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            m_tags.Clear();
        }

        void ApplyPropertyToSysVar(List<SysVar> sysVariables)
        {
            for (int i = 0; i < sysVariables.Count; i++)
            {
                TextRange range = new TextRange(sysVariables[i].StartPosition, sysVariables[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.DarkViolet));
            }
            sysVariables.Clear();
        }
        #endregion 
    }
}
