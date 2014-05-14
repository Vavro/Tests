using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Pdf.Facades;
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
                PdfExtractor pdfExtractor = new PdfExtractor();
                pdfExtractor.BindPdf(fileName);

                //use parameterless ExtractText method
                pdfExtractor.ExtractText();

                MemoryStream tempMemoryStream = new MemoryStream();
                pdfExtractor.GetText(tempMemoryStream);

                string text = "";
                //specify Unicode encoding type in StreamReader constructor
                using (StreamReader streamReader = new StreamReader(tempMemoryStream, Encoding.Unicode))
                {
                    streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                    text = streamReader.ReadToEnd();
                }

                Console.WriteLine(text);
            }

            Console.ReadLine();
        }
    }
}
