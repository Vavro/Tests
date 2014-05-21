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
using FullTextIndexTests.Fulltext;
using FullTextIndexTests.IFilter;

namespace FullTextIndexTests
{
    class Program
    {
        private static ZaznamFulltext _zaznamFulltext = new ZaznamFulltext();

        [STAThread]
        private static void Main(string[] args)
        {
            Console.WriteLine("Awailible commands: a s q");

            string readCommand = null;
            bool exit = false;
            while (!exit)
            {
                if (readCommand != null)
                {
                    switch (readCommand)
                    {
                        case "a":
                            AddFileToIndex();
                            break;
                        case "b":
                            AddFilesToIndex();
                            break;
                        case "s":
                            SearchIndex();
                            break;
                        case "d":
                            AutoSearch();
                            break;
                        case "q":
                            exit = true;
                            Console.WriteLine("Press Enter to Exit");
                            break;
                        default:
                            Console.WriteLine("Known commands: a s q");
                            break;
                    }
                }

                readCommand = Console.ReadLine();
            }
        }

        private static void AutoSearch()
        {
            _zaznamFulltext.SearchIndex(null, DateTime.Now, "Protokol entity typu");
        }

        private static void AddFilesToIndex()
        {
            var fileName = @"c:\users\vavro\desktop\data.pdf"; //dialog.FileName;
            var zaznam = new ZaznamFulltextDocument()
                         {
                             Id = "1",
                             ZaznamMetadata =
                             {
                                 VytvoreniZaznamu = DateTime.Now
                             },
                             FileFulltextInfo =
                             {
                                 FileName = fileName,
                                 FileStream = new FileStream(fileName, FileMode.Open)
                             }
                         };
            _zaznamFulltext.AddZaznamToFulltext(zaznam);
        }

        private static void SearchIndex()
        {
        }

        private static void AddFileToIndex()
        {
            var dialog = new OpenFileDialog()
                         {
                             Multiselect = false
                         };
            dialog.ShowDialog();
        }
    }
}
