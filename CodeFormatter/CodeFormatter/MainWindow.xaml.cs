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

        private int Get_keyWord_lastIndex(int index, string keyWord)
        {
            return file.IndexOf(keyWord, index) + keyWord.Length;

        }

        private int Get_keyWord_firstIndex(int index, string keyWord) 
        {
            return file.IndexOf(keyWord, index);
        }
        private TextRange Get_Range_by_Index(int firstIndex, int lastIndex)
        {
            TextPointer start_pointer = rtb.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward).GetPositionAtOffset(firstIndex, LogicalDirection.Forward);
            TextPointer end_pointer = rtb.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward).GetPositionAtOffset(lastIndex + 1, LogicalDirection.Forward);
            TextRange got = new TextRange(start_pointer, end_pointer);
            return got;
        }

        private void Color_Format_keyWord(TextRange got)
        {
            got.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
            got.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        private void Highlight_keyWord(string keyWord, string script)
        {
            TextRange keyWordRange;
            int firstIndex;
            int index = 0;
            int lastIndex;
            int scriptLength = script.Length;
            int keyWordLength = keyWord.Length;
            while (scriptLength >= keyWordLength){
                if (keyWordLength == 0)
                    break;
                firstIndex = Get_keyWord_firstIndex(index, keyWord);
                lastIndex = Get_keyWord_lastIndex(index, keyWord);
                if (firstIndex < 0 || lastIndex < 0)
                    break;
                keyWordRange = Get_Range_by_Index(firstIndex, lastIndex);
                Color_Format_keyWord(keyWordRange);
                scriptLength -= lastIndex + 1;
                index += lastIndex + 1;
            }


        }
        private void Color_Format_Clauses()
        {

            string[] clausesSQL = {"where"};
                foreach (string clauseCounter in clausesSQL)
                {
                    Highlight_keyWord(clauseCounter, file);            
                 }
        }

        private void btn_apply_click(object sender, RoutedEventArgs e)
        {
            Color_Format_Clauses();
        }

    }
}
