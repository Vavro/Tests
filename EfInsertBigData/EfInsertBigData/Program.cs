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

            while (true)
            {

                using (var db = new TransactionProtocolContext())
                {
                    var cekajici = db.TP_CEKAJICIZMENY.OrderBy(t => t.DATUMEND).Take(batchSize).ToList();

                    foreach (var tpCekajicizmeny in cekajici)
                    {
                        var xmlAsBytes = XmlAsBytes(tpCekajicizmeny.ZMENYXML);
                        var sha256OfFile = ComputeHash(xmlAsBytes);
                        var compressedXml = CompressXml(tpCekajicizmeny.ZMENYXML);
                        var zpracTp = new TP_ZPRACOVANE()
                                      {
                                          ID = tpCekajicizmeny.ID,
                                          DATUMSTART = tpCekajicizmeny.DATUMSTART,
                                          DATUMEND = tpCekajicizmeny.DATUMEND,
                                          ZMENYXMLCOMP = compressedXml,
                                          ZMENYXMLHASH = sha256OfFile
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

            Console.WriteLine("FINISHED, time {0:T}", timer.Elapsed);
        }

        private static void InsertBigData()
        {
            using (var db = new TransactionProtocolContext())
            {
                var zmenyXmlToDeleteIds = db.TP_CEKAJICIZMENY.Select(cz => cz.ID).ToList();
                var xmlToDeleteEnumerator = zmenyXmlToDeleteIds.GetEnumerator();

                var archiveTpPageSize = 100;
                for (int i = 0; i * archiveTpPageSize < zmenyXmlToDeleteIds.Count; i++)
                {
                    var ids = new List<object>();
                    var sqlCommand = new StringBuilder("DELETE FROM TP_CEKAJICIZMENY WHERE ID IN (");
                    for (int j = 0; j < archiveTpPageSize && (i * archiveTpPageSize) + j < zmenyXmlToDeleteIds.Count; j++)
                    {
                        if (!xmlToDeleteEnumerator.MoveNext())
                        {
                            break;
                        }
                        sqlCommand.Append("@p");
                        sqlCommand.Append(j.ToString());
                        sqlCommand.Append(", ");
                        ids.Add(xmlToDeleteEnumerator.Current);
                        //var zmenaXmlId = zmenyXmlToDeleteIds.[i*archiveTpPageSize + j];
                        //var zmenaXml = new CekajiciTransProtokolZmeny() {Id = zmenaXmlId};
                        //context.Set<CekajiciTransProtokolZmeny>().Attach(zmenaXml);
                        //context.Set<CekajiciTransProtokolZmeny>().Remove(zmenaXml);

                    }
                    sqlCommand.Length--;
                    sqlCommand.Length--; //odstraneni poslednich ', '
                    sqlCommand.Append(")");
                    var array = ids.ToArray();
                    db.Database.ExecuteSqlCommand(sqlCommand.ToString(), array);

                    if (ids.Count < archiveTpPageSize)
                    {
                        break;
                    }
                    //context.SaveChanges();
                }
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
            return Encoding.Unicode.GetBytes(xml);
        }

        public static string ComputeHash(byte[] fileBytes)
        {
            using (var sha1 = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] hash = sha1.ComputeHash(fileBytes);
                var formatted = new StringBuilder(2 * hash.Length);
                foreach (byte b in hash)
                {
                    formatted.AppendFormat("{0:X2}", b);
                }
                return formatted.ToString();
            }
        }
    }


}
