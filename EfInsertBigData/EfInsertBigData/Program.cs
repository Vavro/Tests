using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace EfInsertBigData
{
    class Program
    {
        private const string InputFile = @"D:\TestTP\output.txt";
        private const int batchSize = 100;

        static void Main(string[] args)
        {
            InsertBigData();
            Console.WriteLine("Press enter for compression");
            Console.ReadLine();
            CompressBigData();
            Console.WriteLine("Pres enter to exit");
            Console.ReadLine();
        }

        private static void CompressBigData()
        {
            using (var db = new TransactionProtocolContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM TP_ZPRACOVANE");
            }

            var batchesProcesed = 0;
            var timer = new Stopwatch();
            timer.Start();

            //first line are headers
            using (var transation = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                while (true)
                {

                    using (var db = new TransactionProtocolContext())
                    {
                        var cekajici = db.TP_CEKAJICIZMENY.OrderBy(t => t.DATUMEND).Take(batchSize).ToList();

                        foreach (var tpCekajicizmeny in cekajici)
                        {
                            var compressedXml = CompressXml(tpCekajicizmeny.ZMENYXML);
                            var zpracTp = new TP_ZPRACOVANE()
                                          {
                                              ID = tpCekajicizmeny.ID,
                                              DATUMSTART = tpCekajicizmeny.DATUMSTART,
                                              DATUMEND = tpCekajicizmeny.DATUMEND,
                                              ZMENYXMLCOMP = compressedXml
                                          };

                            db.TP_CEKAJICIZMENY.Remove(tpCekajicizmeny);
                            db.TP_ZPRACOVANE.Add(zpracTp);
                        }

                        db.SaveChanges();

                        if (cekajici.Count < batchSize)
                        {
                            break;
                        }
                    }

                    batchesProcesed++;
                    Console.WriteLine("{0:N0} items converted, time {1:T}", batchesProcesed * batchSize, timer.Elapsed);

                }
                transation.Complete();
            }

            Console.WriteLine("FINISHED, time {0:T}", timer.Elapsed);
        }

        private static void InsertBigData()
        {
            using (var db = new TransactionProtocolContext())
            {
                List<object> ids = db.TP_CEKAJICIZMENY.Take(100).Select(cz => cz.ID).ToList().ConvertAll(d => (object)d);
                
                var sqlCommand = new StringBuilder("DELETE FROM TP_CEKAJICIZMENY WHERE ID IN (");
                for (int i = 0; i < 100; i++)
                {
                    sqlCommand.Append("@p");
                    sqlCommand.Append(i.ToString());
                    sqlCommand.Append(", ");
                }
                sqlCommand.Length--;
                sqlCommand.Length--;
                //odstraneni posledniho ', '
                sqlCommand.Append(")");
                db.Database.ExecuteSqlCommand(sqlCommand.ToString(), ids.ToArray());

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
                    for (var processedLines = 0;
                        (line = reader.ReadLine()) != null && processedLines < batchSize;
                        processedLines++)
                    {
                        var split = line.Split(new string[] { "','" }, StringSplitOptions.None);
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

        private static byte[] CompressXml(string zmenyXml)
        {
            byte[] fileToSaveToDul;
            var xmlAsBytes = XmlAsBytes(zmenyXml);
            using (var inputZmenyXml = new MemoryStream(xmlAsBytes))
            using (MemoryStream outputMemStream = new MemoryStream())
            {
                var internalFileName = "Test.xml";
                using (ZipOutputStream zipStream = new ZipOutputStream(outputMemStream))
                {
                    zipStream.SetLevel(6); //0-9, 9 being the highest level of compression

                    ZipEntry newEntry = new ZipEntry(internalFileName);
                    newEntry.DateTime = DateTime.Now;

                    zipStream.PutNextEntry(newEntry);

                    StreamUtils.Copy(inputZmenyXml, zipStream, new byte[4096]);
                    zipStream.CloseEntry();

                    zipStream.IsStreamOwner = false;
                    zipStream.Close();
                }
                outputMemStream.Position = 0;

                // Alternative outputs:
                // ToArray is the cleaner and easiest to use correctly with the penalty of duplicating allocated memory.
                return outputMemStream.ToArray();
            }
        }

        public static byte[] XmlAsBytes(string xml)
        {
            return Encoding.UTF8.GetBytes(xml);
        }
    }


}
