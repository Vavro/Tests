using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfInsertBigData
{
    class Program
    {
        private const string InputFile = @"D:\TestTP\output.txt";

        static void Main(string[] args)
        {
            using (var reader = new StreamReader(InputFile))
            {
                var line = reader.ReadLine();
                //first line are headers

                line = reader.ReadLine();
                var split = line.Split(',');
                if (split.Length != 4)
                    throw new Exception("Wrong split part count");
                
                var tp = new TP_CEKAJICIZMENY()
                         {
                             ID = int.Parse(split[0].Trim('\'')),
                             DATUMSTART = DateTime.Parse(split[1].Trim('\'')),
                             DATUMEND = DateTime.Parse(split[2].Trim('\'')),
                             ZMENYXML = split[3].Trim('\'')
                         };

                using (var db = new TransactionProtocolContext())
                {
                    db.Set<TP_CEKAJICIZMENY>().Add(tp);
                    db.SaveChanges();
                }
            }


        }
    }
}
