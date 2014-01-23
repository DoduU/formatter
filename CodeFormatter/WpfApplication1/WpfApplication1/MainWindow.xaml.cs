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
            string pattern = @"is";
            int cnt = 1;

            Regex rx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection mc = rx.Matches(file);
            foreach (Match m in mc)
            {
                TextPointer start = rtb.Document.ContentStart;

                    TextPointer startPos = start.GetPositionAtOffset(file.IndexOf(m.Value) + 8);

                    TextPointer next = start.GetPositionAtOffset(file.IndexOf(m.Value) + 8 + m.Value.Length);
                    String textRun = startPos.GetTextInRun(LogicalDirection.Forward);
                    rtb.AppendText(System.Environment.NewLine);
                    rtb.AppendText(startPos.GetTextRunLength(LogicalDirection.Forward).ToString());
                    rtb.AppendText(System.Environment.NewLine);
                    rtb.AppendText(startPos.GetTextRunLength(LogicalDirection.Backward).ToString());
                    rtb.AppendText(System.Environment.NewLine);
                    rtb.AppendText(textRun);
                    TextRange tr = new TextRange(startPos, next);
                    Color_Format_keyWord(tr, cnt);
                   

            }


        }
        private void Color_Format_keyWord(TextRange got, int cnt)
        {
            if (cnt == 1)
            {
                got.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                got.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            else 
            {
                got.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.OrangeRed);
                got.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
        }
    }
}
