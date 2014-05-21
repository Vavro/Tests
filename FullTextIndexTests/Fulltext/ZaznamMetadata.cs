using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullTextIndexTests.Fulltext
{
    public class ZaznamMetadata
    {
        public DateTime VytvoreniZaznamu { get; set; }
        //todo: add PP, ORG, USR identifikaci
    }

    public class ZaznamFulltextDocument
    {
        public string Id { get; set; }
        public ZaznamMetadata ZaznamMetadata { get; set; }
        public FileFulltextInfo FileFulltextInfo { get; set; }
    }

    public class FileFulltextInfo
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
