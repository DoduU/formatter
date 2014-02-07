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
using System.Windows.Shapes;

namespace CodeFormatter
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        #region UICheckedListeners
        private void SpacingControlChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Space1.IsEnabled = true;
                Space2.IsEnabled = true;
                Space3.IsEnabled = true;
                Space4.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
                
            }
        }

        private void SpacingControlUnckecked(object sender, RoutedEventArgs e)
        {
            Space1.IsEnabled = false;
            Space2.IsEnabled = false;
            Space3.IsEnabled = false;
            Space4.IsEnabled = false;
        }


        private void OperatorControlChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Operator1.IsEnabled = true;
                Operator2.IsEnabled = true;
                Operator3.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {

            }
        }

        private void OperatorControlUnchecked(object sender, RoutedEventArgs e)
        {
            Operator1.IsEnabled = false;
            Operator2.IsEnabled = false;
            Operator3.IsEnabled = false;
        }

        private void EmptyLineUnchecked(object sender, RoutedEventArgs e)
        {
            empty1.IsEnabled = false;
            empty2.IsEnabled = false;
        }

        private void EmptyLineChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                empty1.IsEnabled = true;
                empty2.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {

            }
        }

        private void CommentsControlChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                BlockComment1.IsEnabled = true;
                BlockComment2.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {

            }
        }

        private void CommentsControlUnchecked(object sender, RoutedEventArgs e)
        {
            BlockComment1.IsEnabled = false;
            BlockComment2.IsEnabled = false;
        }

        private void LineCommentUnchecked(object sender, RoutedEventArgs e)
        {
            LineComment1.IsEnabled = false;
            LineComment2.IsEnabled = false;
            LineComment3.IsEnabled = false;
            LineComment4.IsEnabled = false;           
        }

        private void LineCommentChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                LineComment1.IsEnabled = true;
                LineComment2.IsEnabled = true;
                LineComment3.IsEnabled = true;
                LineComment4.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void LogExprChecked(object sender, RoutedEventArgs e)
        {
            try
            {

                LogicalExpression.IsEnabled = true;
                if (LogicalExpression.IsChecked == true)
                {
                    expradio1.IsEnabled = true;
                    expradio2.IsEnabled = true;
                    expradio3.IsEnabled = true;
                }
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void LogExrpUnchecked(object sender, RoutedEventArgs e)
        {
            LogicalExpression.IsEnabled = false;
            expradio1.IsEnabled = false;
            expradio2.IsEnabled = false;
            expradio3.IsEnabled = false;
        }

        private void ColumnListChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ColumnList1.IsEnabled = true;
                ColumnList2.IsEnabled = true;
                if (ColumnList1.IsChecked == true)
                {
                    clradio1.IsEnabled = true;
                    clradio2.IsEnabled = true;
                    clradio3.IsEnabled = true;
                }
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void ColumnListUnchecked(object sender, RoutedEventArgs e)
        {
            ColumnList1.IsEnabled = false;
            ColumnList2.IsEnabled = false;
            clradio1.IsEnabled = false;
            clradio2.IsEnabled = false;
            clradio3.IsEnabled = false;
        }

        private void DataStatementsChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataStatements1.IsEnabled = true;
                if (DataStatements1.IsChecked == true)
                {
                    dsradio1.IsEnabled = true;
                    dsradio2.IsEnabled = true;
                    AlignDS.IsEnabled = true;
                    Postfix1.IsEnabled = true;
                }
            }
            catch (NullReferenceException ex)
            {
            }

        }

        private void DataStatementsUnchecked(object sender, RoutedEventArgs e)
        {
            DataStatements1.IsEnabled = false;
            dsradio1.IsEnabled = false;
            dsradio2.IsEnabled = false;
            AlignDS.IsEnabled = false;
            Postfix1.IsEnabled = false;

        }

        private void JoinUnchecked(object sender, RoutedEventArgs e)
        {
            Join1.IsEnabled = false;
            Join2.IsEnabled = false;
            jradio1.IsEnabled = false;
            jradio2.IsEnabled = false;
            jradio3.IsEnabled = false;
            jradio4.IsEnabled = false;
            jradio5.IsEnabled = false;
            AlignJ2.IsEnabled = false;
            AlignJ.IsEnabled = false;
            space2.IsEnabled = false;
            space3.IsEnabled = false;
        }

        private void JoinChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Join1.IsEnabled = true;
                Join2.IsEnabled = true;
                if (Join1.IsChecked == true)
                {
                    jradio1.IsEnabled = true;
                    jradio2.IsEnabled = true;
                    jradio3.IsEnabled = true;
                    AlignJ.IsEnabled = true;
                    space2.IsEnabled = true;
                }
                if (Join2.IsChecked == true)
                {
                    jradio4.IsEnabled = true;
                    jradio5.IsEnabled = true;
                    AlignJ2.IsEnabled = true;
                    space3.IsEnabled = true;
                }
            }
            catch (NullReferenceException ex)
            {
            }
                
        }

        private void LogicalExprChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                expradio1.IsEnabled = true;
                expradio2.IsEnabled = true;
                expradio3.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void LogicalExprUnchecked(object sender, RoutedEventArgs e)
        {
            expradio1.IsEnabled = false;
            expradio2.IsEnabled = false;
            expradio3.IsEnabled = false;
        }

        private void ColList2Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                clradio1.IsEnabled = true;
                clradio2.IsEnabled = true;
                clradio3.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void ColList2Unchecked(object sender, RoutedEventArgs e)
        {
            clradio1.IsEnabled = false;
            clradio2.IsEnabled = false;
            clradio3.IsEnabled = false;
        }

        private void OnUnchecked(object sender, RoutedEventArgs e)
        {
            jradio4.IsEnabled = false;
            jradio5.IsEnabled = false;
            AlignJ2.IsEnabled = false;
            space3.IsEnabled = false;
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                jradio4.IsEnabled = true;
                jradio5.IsEnabled = true;
                AlignJ2.IsEnabled = true;
                space3.IsEnabled = true;
            }

            catch (NullReferenceException ex)
            {
            }
        }

        private void Join1Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                jradio1.IsEnabled = true;
                jradio2.IsEnabled = true;
                jradio3.IsEnabled = true;
                AlignJ.IsEnabled = true;
                space2.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void Join1Unchecked(object sender, RoutedEventArgs e)
        {
            jradio1.IsEnabled = false;
            jradio2.IsEnabled = false;
            jradio3.IsEnabled = false;
            AlignJ.IsEnabled = false;
            space2.IsEnabled = false;
        }

        private void ValueChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Value1.IsEnabled = true;
                Value2.IsEnabled = true;
                if (Value1.IsChecked == true)
                {

                    valradio1.IsEnabled = true;
                    valradio2.IsEnabled = true;
                    AlignVa.IsEnabled = true;
                    space4.IsEnabled = true;
                }
                if(Value2.IsChecked == true)
                {
                valradio3.IsEnabled = true;
                valradio4.IsEnabled = true;
                }

            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void ValueUnchecked(object sender, RoutedEventArgs e)
        {
            Value1.IsEnabled = false;
            Value2.IsEnabled = false;
            if (Value1.IsEnabled == false)
            {
                valradio1.IsEnabled = false;
                valradio2.IsEnabled = false;
                AlignVa.IsEnabled = false;
                space4.IsEnabled = false;
            }
            if (Value2.IsEnabled == false)
            {
                valradio3.IsEnabled = false;
                valradio4.IsEnabled = false;
            }
        }

        private void PlaceValueUnchecked(object sender, RoutedEventArgs e)
        {
            valradio1.IsEnabled = false;
            valradio2.IsEnabled = false;
            AlignVa.IsEnabled = false;
            space4.IsEnabled = false;
        }

        private void PlaceValueChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                AlignVa.IsEnabled = true;
                space4.IsEnabled = true;
                valradio1.IsEnabled = true;
                valradio2.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void PlaceRowChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                valradio3.IsEnabled = true;
                valradio4.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void PlaceRowUnchecked(object sender, RoutedEventArgs e)
        {
            valradio3.IsEnabled = false;
            valradio4.IsEnabled = false;
        }

        private void KeywordChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                dsradio1.IsEnabled = true;
                dsradio2.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void KeywordUnchecked(object sender, RoutedEventArgs e)
        {
            dsradio1.IsEnabled = false;
            dsradio2.IsEnabled = false;
        }

        private void VariablesChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Variable1.IsEnabled = true;
                if (Variable1.IsChecked == true)
                {
                    varradio1.IsEnabled = true;
                    varradio2.IsEnabled = true;
                }
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void VariablesUnchecked(object sender, RoutedEventArgs e)
        {
            Variable1.IsEnabled = false;
            varradio1.IsEnabled = false;
            varradio2.IsEnabled = false;
        }

        private void PlaceVarUnchecked(object sender, RoutedEventArgs e)
        {
            varradio1.IsEnabled = false;
            varradio2.IsEnabled = false;
        }

        private void PlaceVarChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                varradio1.IsEnabled = true;
                varradio2.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void CondKeywordsChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Prog1.IsEnabled = true;
                if (Prog1.IsChecked == true)
                {

                    progradio2.IsEnabled = true;
                    AlignProg.IsEnabled = true;
                    space5.IsEnabled = true;
                }
            }
            catch (NullReferenceException ex)
            {
            }
            
        }

        private void CondKeywordsUnchecked(object sender, RoutedEventArgs e)
        {
            Prog1.IsEnabled = false;
            progradio2.IsEnabled = false;
            AlignProg.IsEnabled = false;
            space5.IsEnabled = false;
        }

        private void PlaceCondChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                progradio2.IsEnabled = true;
                AlignProg.IsEnabled = true;
                space5.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void PlaceCondUnchecked(object sender, RoutedEventArgs e)
        {
            progradio2.IsEnabled = false;
            AlignProg.IsEnabled = false;
            space5.IsEnabled = false;
        }

        private void BEChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Prog2.IsEnabled = true;
                Prog3.IsEnabled = true;
            }
            catch (NullReferenceException ex)
            {
            }
        }

        private void BEUnchecked(object sender, RoutedEventArgs e)
        {
            Prog2.IsEnabled = false;
            Prog3.IsEnabled = false;
        }

        private void ConfigFormCancelClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
        #endregion
}
