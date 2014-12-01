namespace LuceneNetCzechSupport.Lucene
{
    public class FullTextDocument
    {
        public FullTextDocument()
        {
            FileFulltextInfo = new FileFulltextInfo();
        }

        public string Id { get; set; }
        public FileFulltextInfo FileFulltextInfo { get; set; }
    }
}