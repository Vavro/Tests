using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigFileTransformation
{
    class Program
    {
        private const int FlushAfterLines = 100*1000;

        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();
            using (var reader = System.IO.File.OpenText(@"D:\TestTP\triada_export.txt"))
            using (var writer = new StreamWriter(@"D:\TestTP\output.txt", false))
            {
                string line = null;
                int processedLines = 0;
                int processed10kLinesCount = 0;
                //first line is column headers
                line = reader.ReadLine();
                writer.WriteLine(line);

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.EndsWith(">'"))
                    {
                        writer.WriteLine(line);
                    }
                    else
                    {
                        writer.Write(line);
                    }
                    processedLines++;
                    if (processedLines % FlushAfterLines == 0)
                    {
                        processed10kLinesCount++;
                        Console.WriteLine("Processed {0} lines. Elapsed time: {1:T}", processed10kLinesCount*FlushAfterLines, timer.Elapsed);
                        writer.Flush();
                    }
                }
            }
        }
    }
}
