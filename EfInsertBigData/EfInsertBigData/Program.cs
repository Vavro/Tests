using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfInsertBigData
{
    class Program
    {
        private const string InputFile = @"D:\TestTP\output.txt";
        private const int batchSize = 100;

        static void Main(string[] args)
        {
            using (var db = new TransactionProtocolContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM TP_CEKAJICIZMENY");
            }

            var batchesProcesed = 0;
            var timer = new Stopwatch();
            timer.Start();
            using (var reader = new StreamReader(InputFile))
            {
                var line = reader.ReadLine();
                //first line are headers
                while (true)
                {
                    var zmeny = new List<TP_CEKAJICIZMENY>(batchSize);
                    for (var processedLines = 0; (line = reader.ReadLine()) != null && processedLines < batchSize; processedLines++)
                    {
                        var split = line.Split(new string[] {"','"},StringSplitOptions.None);
                        if (split.Length != 4)
                            throw new Exception("Wrong split part count");

                        var tp = new TP_CEKAJICIZMENY()
                                 {
                                     ID = int.Parse(split[0].TrimStart('\'')),
                                     DATUMSTART = DateTime.Parse(split[1]),
                                     DATUMEND = DateTime.Parse(split[2]),
                                     ZMENYXML = split[3].TrimEnd('\'')
                                 };

                        zmeny.Add(tp);
                    }

                    batchesProcesed++;
                    Console.WriteLine("{0:##,###} items loaded, time {1:T}", batchesProcesed * batchSize, timer.Elapsed);

                    using (var db = new TransactionProtocolContext())
                    {
                        foreach (var tp in zmeny)
                        {
                            db.Set<TP_CEKAJICIZMENY>().Add(tp);
                        }
                        db.SaveChanges();
                    }

                    Console.WriteLine("{0:##,###} items saved, time {1:T}", batchesProcesed * batchSize, timer.Elapsed);

                    if (zmeny.Count < batchSize)
                    {
                        break;
                    }
                }
            }


            using (var db = new TransactionProtocolContext())
            {
                var elementsCont = db.Set<TP_CEKAJICIZMENY>().Count();
                var first = db.Set<TP_CEKAJICIZMENY>().First();
            }

        }
    }
}
