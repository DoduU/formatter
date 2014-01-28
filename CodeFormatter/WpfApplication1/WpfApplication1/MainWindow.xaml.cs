using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool isComment = false;
        bool isVariable = false;
        new struct SysVar
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }

        new struct stringVar
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }

        new struct SQLLabel
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }

        new struct Comment
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;

        }

        new struct Tag
        {
            public TextPointer StartPosition;
            public TextPointer EndPosition;
            public string Word;
            
        }

        List<Tag> m_tags = new List<Tag>();
        List<SysVar>sysVariables = new List<SysVar>();
        List<Comment> comments = new List<Comment>();
        List<SQLLabel> labels = new List<SQLLabel>();
        List<stringVar> strVar = new List<stringVar>();

        string curFile;
        string file;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openSqlScript = new OpenFileDialog();
            openSqlScript.ShowDialog();
            curFile = openSqlScript.FileName;
            file = File.ReadAllText(curFile);
            rtb.Document.Blocks.Clear();

            rtb.AppendText(file);
            //Color_Format_Clauses();
            Highlight_sysVar();
        }
        private void Highlight_sysVar()
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

            Format();
        }
 /*       private void To_Upper()
        {
            for (int i = 0; i < m_tags.Count; i++)
            {
                m_tags[i].Word.ToUpper();
            }

            for (int i = 0; i < labels.Count; i++)
            {
                TextRange range = new TextRange(labels[i].StartPosition, labels[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
            }

            for (int i = 0; i < sysVariables.Count; i++)
            {
                TextRange range = new TextRange(sysVariables[i].StartPosition, sysVariables[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.DarkViolet));
            }

            for (int i = 0; i < comments.Count; i++)
            {
                TextRange range = new TextRange(comments[i].StartPosition, comments[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Green));
            }

        } */

        void Format()
        {
            for (int i = 0; i < m_tags.Count; i++)
            {
                TextRange range = new TextRange(m_tags[i].StartPosition, m_tags[i].EndPosition);
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
                range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }

            for (int i = 0; i < labels.Count; i++)
            {
                TextRange range = new TextRange(labels[i].StartPosition, labels[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
            }

            for (int i = 0; i < sysVariables.Count; i++)
            {
                TextRange range = new TextRange(sysVariables[i].StartPosition, sysVariables[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.DarkViolet));
            }

            for (int i = 0; i < comments.Count; i++)
            {
                TextRange range = new TextRange(comments[i].StartPosition, comments[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Green));
            }

            for (int i = 0; i < strVar.Count; i++)
            {
                TextRange range = new TextRange(strVar[i].StartPosition, strVar[i].EndPosition);
                range.ClearAllProperties();
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
            }
            comments.Clear();
            sysVariables.Clear();
            m_tags.Clear();
        }

        void CheckWordsInRun(Run run)
        {
            Regex reg = new Regex(@"(\w{0,})'\w{0,}'(?!\s|;)");
            string text = run.Text;
            int sIndex = 0;
            int eIndex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsWhiteSpace(text[i]) | JSSyntaxProvider.GetSpecials.Contains(text[i]))
                {
                    if (i > 0 && !(Char.IsWhiteSpace(text[i - 1]) | JSSyntaxProvider.GetSpecials.Contains(text[i - 1])))
                    {

                        eIndex = i - 1;


                        string word = text.Substring(sIndex, eIndex - sIndex + 1);
                        if (word.Contains("/*"))
                        {
                            isComment = true;
                        }

                        if (word.Contains(@"*/"))
                        {
                            isComment = false;
                            string comment = word;
                            Comment comm = new Comment();
                            comm.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                            comm.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + comment.Length + 1, LogicalDirection.Backward);
                            comm.Word = comment;
                            comments.Add(comm);

                        }
                        if (isComment)
                        {
                            string comment = word; 
                            Comment comm = new Comment();
                            comm.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                            comm.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + comment.Length + 1, LogicalDirection.Backward);
                            comm.Word = comment;
                            comments.Add(comm);
                        }

                        if (word.Contains("--"))
                        {
                            string comment = text.Substring(sIndex);
                            Comment comm = new Comment();
                            comm.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                            comm.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex+comment.Length+1, LogicalDirection.Backward);
                            comm.Word = comment;
                            comments.Add(comm);
                        }


                        //if (reg.Match(word).Success)
                        //{
                        //    string variable = word;
                        //    stringVar var = new stringVar();
                        //    var.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                        //    var.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + word.Length + 1, LogicalDirection.Backward);
                        //    var.Word = word;
                        //    strVar.Add(var);
                        //}

                        //if (!reg.Match(word).Success && isVariable == false && word.Contains(@"'"))
                        //{
                        //    isVariable = true;
                        //}
                             



                        //if (isVariable)
                        //{
                        //    string variable = word;
                        //    stringVar var = new stringVar();
                        //    var.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                        //    var.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + word.Length + 1, LogicalDirection.Backward);
                        //    var.Word = word;
                        //    strVar.Add(var);
                        //}

                        //if (isVariable == true && word.Contains(@"'|';"))
                        //{
                        //    isVariable = false;
                        //    string variable = word;
                        //    stringVar var = new stringVar();
                        //    var.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                        //    var.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + word.Length + 1, LogicalDirection.Backward);
                        //    var.Word = word;
                        //    strVar.Add(var);
                        //}

                        if (word.Contains("@@"))
                        {
                            SysVar sysVar = new SysVar();
                            sysVar.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                            sysVar.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                            sysVar.Word = word;
                            sysVariables.Add(sysVar);
                        }
                        if (word.Length != 0)
                        {
                            if (JSSyntaxProvider.IsKnownTag(word))
                            {
                                Tag t = new Tag();
                                t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                                t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                                t.Word = word;
                                m_tags.Add(t);
                            }
                            if (word.Length != 0)
                            {
                                if (word.Substring(word.Length - 1).Contains(':'))
                                {
                                    SQLLabel t = new SQLLabel();
                                    t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                                    t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + 1, LogicalDirection.Backward);
                                    t.Word = word;
                                    labels.Add(t);
                                }
                            }
                        }
                    }
                    sIndex = i + 1;
                }
            }

            string lastWord = text.Substring(sIndex, text.Length - sIndex);
        if(lastWord.Length!=0)
         {
            if (JSSyntaxProvider.IsKnownTag(lastWord))
            {
                Tag t = new Tag();
                t.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                t.EndPosition = run.ContentStart.GetPositionAtOffset(eIndex + lastWord.Length, LogicalDirection.Backward);
                t.Word = lastWord;
                m_tags.Add(t);
            }

         }
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
            if (lastWord.Contains(@"*/"))
            {
                isComment = false;
                string comment = lastWord;
                Comment comm = new Comment();
                comm.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
                comm.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + comment.Length + 1, LogicalDirection.Backward);
                comm.Word = comment;
                comments.Add(comm);

            }

            //if (reg.Match(lastWord).Success)
            //{
            //    string variable = lastWord;
            //    stringVar var = new stringVar();
            //    var.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
            //    var.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + lastWord.Length + 1, LogicalDirection.Backward);
            //    var.Word = lastWord;
            //    strVar.Add(var);
            //}

            //if (!reg.Match(lastWord).Success && isVariable == true && lastWord.Contains("'|';"))
            //{
            //    isVariable = false;
            //    string variable = lastWord;
            //    stringVar var = new stringVar();
            //    var.StartPosition = run.ContentStart.GetPositionAtOffset(sIndex, LogicalDirection.Forward);
            //    var.EndPosition = run.ContentStart.GetPositionAtOffset(sIndex + lastWord.Length + 1, LogicalDirection.Backward);
            //    var.Word = lastWord;
            //    strVar.Add(var);
            //}
        }

    }
}
