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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string curFile;
        string file;

        private void btn_Click(object sender, RoutedEventArgs e)
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

        private MatchCollection KeyWordSearch(string file)
        {
            string pattern = @"((?<=\s{1,})IF(?=(\s)))|((?<=\s{1,})ELSE(?=(\s)))|((?<=\s{1,})WHILE(?=(\s)))|((?<=\s{1,})FOR(?=(\s)))|
                                ((?<=\s{1,})INSERT(?=(\s)))|((?<=\s{1,})ON(?=(\s)))|((?<=\s{1,})MERGE(?=(\s)))|((?<=\s{1,})DROP(?=(\s)))|((?<=\s{1,})JOIN(?=(\s)))|((?<=\s{1,})LEFT(?=(\s)))|
                                ((?<=\s{1,})RIGHT(?=(\s)))|((?<=\s{1,})INNER(?=(\s)))|((?<=\s{1,})UPDATE(?=(\s)))|((?<=\s{1,})DELETE(?=(\s)))|((?<=\s{1,})BEGIN(?=(\s)))|((?<=\s{1,})END(?=(\s|;)))|
                                ((?<=\s{1,})TRANSACTION(?=(\s)))|((?<=\s{1,})COMMIT(?=(\s)))|((?<=\s{1,})GO(?=(\s|;)))|((?<=\s{1,})USE(?=(\s)))|((?<=\s{1,})EXECUTE(?=(\s)))|((?<=\s{1,})GOTO(?=(\s)))|
                                ((?<=\s{1,})EXEC(?=(\s)))|((?<=\s{1,})ROLLBACK(?=(\s)))|((?<=\s{1,})DECLARE(?=(\s)))|((?<=\s{1,})FROM(?=(\s)))|((?<=\s{1,})SELECT(?=(\s)))|((?<=\s{1,})WHERE(?=(\s)))|
                                ((?<=\s{1,})GROUP BY(?=(\s)))|((?<=\s{1,})ORDER BY(?=(\s)))|((?<=\s{1,})TOP(?=(\s)))|((?<=\s{1,})HAVING(?=(\s)))|((?<=\s{1,})@{2}\w+(?=(\s|;)))";
            Regex rx = new Regex(pattern, RegexOptions.IgnoreCase);
            return rx.Matches(file);
        }

        private void Highlight_sysVar()
        {
            MatchCollection mc = KeyWordSearch(file);
            foreach(Match m in mc)
            {
                for (TextPointer position = rtb.Document.ContentStart;
                    position != null && position.CompareTo(rtb.Document.ContentEnd) <= 0;
                    position = position.GetNextContextPosition(LogicalDirection.Forward))
                {
                if (position.CompareTo(rtb.Document.ContentEnd) == 0)
                {
                    break;
                }

                String textRun = position.GetTextInRun(LogicalDirection.Forward);
                StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;
                Int32 indexInRun = textRun.IndexOf(m.Value, stringComparison);
                if (indexInRun >= 0)
                {
                    position = position.GetPositionAtOffset(indexInRun);
                    if (position != null)
                    {
                        TextPointer nextPointer = position.GetPositionAtOffset(m.Value.Length);
                        TextRange textRange = new TextRange(position, nextPointer);
                        if (textRange.Text.Contains("@@"))
                        {
                            Color_Format_sysVar(textRange);
                        }
                        else 
                        {
                            Color_Format_keyWord(textRange);
                        }
                    }
                  }
                }
            }
        }
        /*private void Get_Range_by_index(string search)
        {
            for (TextPointer position = rtb.Document.ContentStart;
                 position != null && position.CompareTo(rtb.Document.ContentEnd) <= 0;
                 position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (position.CompareTo(rtb.Document.ContentEnd) == 0)
                {
                    break;
                }

                String textRun = position.GetTextInRun(LogicalDirection.Forward);
                StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;
                Int32 indexInRun = textRun.IndexOf(search, stringComparison);
                if (indexInRun >= 0)
                {
                    position = position.GetPositionAtOffset(indexInRun);
                    if (position != null)
                    {
                        TextPointer nextPointer = position.GetPositionAtOffset(search.Length);
                        TextRange textRange = new TextRange(position, nextPointer);
                        textRange.Text.ToUpper();
                        Color_Format_keyWord(textRange);
                    }
                }
            }
        }*/

        private void Color_Format_keyWord(TextRange got)
        {
            got.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            got.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }
        private void Color_Format_sysVar(TextRange got)
        {
            got.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.DarkViolet);
            got.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

       /* private void Color_Format_Clauses()
        {
            string[] clausesSQL = { "declare", "FROM", "SELECT", "WHERE", "GROUP BY", "ORDER BY", "TOP", "HAVING" };
                foreach (string clauseCounter in clausesSQL)
                {
                    /*Formatting_Methods format = new Formatting_Methods();
                    format.To_Upper_Case(file, clauseCounter);                   
                    Get_Range_by_index(clauseCounter);
                 }
        } 
*/
        private void Case_Format_clause(string script)
        {
          Formatting_Methods format = new Formatting_Methods();

                file = format.To_Upper_Case(script);

                
        }

        private void btn_apply_click(object sender, RoutedEventArgs e)
        {
            rtb.SelectAll();
            rtb.Cut();
            Case_Format_clause(file);
            rtb.AppendText(file);
            //Color_Format_Clauses();
            Highlight_sysVar();

        }

    }
}
