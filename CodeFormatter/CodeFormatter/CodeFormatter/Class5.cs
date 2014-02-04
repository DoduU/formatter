using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CodeFormatter
{
    class TextFormatting
    {
        void TabParameters(Run run)
        {
            string text = run.Text;
            text.Replace(",", System.Environment.NewLine + ",");
        }
    }
}
