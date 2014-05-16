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
using FullTextIndexTests.IFilter;

namespace FullTextIndexTests
{
    class Program
    {
        
        private static void Main(string[] args)
        {
            //var dialog = new OpenFileDialog()
            //             {
            //                 Multiselect = false
            //             };
            //dialog.ShowDialog();

            var fileName = @"c:\users\vavro\desktop\data.pdf"; //dialog.FileName;

            var extension = Path.GetExtension(fileName);
            if (extension == ".pdf")
            {
                var text2 = ParseHelper.ParseIFilter(File.OpenRead(fileName));
                
                Console.WriteLine(text2);

                //PdfExtractor pdfExtractor = new PdfExtractor();
                //pdfExtractor.BindPdf(fileName);

                ////use parameterless ExtractText method
                //pdfExtractor.ExtractText();

                //MemoryStream tempMemoryStream = new MemoryStream();
                //pdfExtractor.GetText(tempMemoryStream);

                //string text = "";
                ////specify Unicode encoding type in StreamReader constructor
                //using (StreamReader streamReader = new StreamReader(tempMemoryStream, Encoding.Unicode))
                //{
                //    streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                //    text = streamReader.ReadToEnd();
                //}

                //Console.WriteLine(text);
            }

            Console.ReadLine();
        }
    }
}
