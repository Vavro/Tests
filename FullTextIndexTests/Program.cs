using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Pdf.Text;
using Aspose.Pdf.Text.TextOptions;

namespace FullTextIndexTests
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var dialog = new OpenFileDialog()
                         {
                             Multiselect = false
                         };
            dialog.ShowDialog();

            var fileName = dialog.FileName;

            var extension = Path.GetExtension(fileName);
            if (extension == ".pdf")
            {
                var doc = new Aspose.Pdf.Document(fileName);
                var textAbsorber = new TextAbsorber();
                doc.Pages.Accept(textAbsorber);

                var text = textAbsorber.Text;

                Console.WriteLine(text);
            }

            Console.ReadLine();
        }
    }
}
