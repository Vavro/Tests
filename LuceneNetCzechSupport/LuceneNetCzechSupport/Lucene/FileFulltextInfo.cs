using System.IO;

namespace LuceneNetCzechSupport.Lucene
{
    public class FileFulltextInfo
    {
        public string FileName { get; set; }
        public TextReader FileTextReader { get; set; }
    }
}