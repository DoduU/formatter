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



        }


        private void Color_Format_Clauses()
        {


            rtb.SelectAll();
            string script = rtb.Selection.Text;
            int keyWordLength;


            string[] clausesSQL = { "select ", "from" };
                foreach (string clauseCounter in clausesSQL)
                {
                    int scriptLength = script.Length;
                    keyWordLength = clauseCounter.Length;
                    int clauseIndexStart = 0;
                    int clauseIndexEnd = 0;
                    int index = 0;
                    if(scriptLength>0 &&keyWordLength>0)
                        while (clauseIndexStart < scriptLength - keyWordLength)
                        {
                            TextPointer start_pointer, end_pointer;
                            clauseIndexStart = file.IndexOf(clauseCounter, index);
                            if (clauseIndexStart < 0)
                                break;

                            clauseIndexEnd = clauseIndexStart + keyWordLength;
                            start_pointer = rtb.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward).GetPositionAtOffset(clauseIndexStart, LogicalDirection.Forward);
                            end_pointer = rtb.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward).GetPositionAtOffset(clauseIndexEnd+1, LogicalDirection.Forward);
                            TextRange got = new TextRange(start_pointer, end_pointer);
                            got.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                            got.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                            index = clauseIndexEnd + 1;
                        }
                }
        }

        private void btn_apply_click(object sender, RoutedEventArgs e)
        {
            Color_Format_Clauses();
        }

    }
}
