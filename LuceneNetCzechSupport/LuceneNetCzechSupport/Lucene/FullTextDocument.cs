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

        public FullTextDocument Copy()
        {
            return new FullTextDocument()
            {
                Id = this.Id,
                FileFulltextInfo = new FileFulltextInfo()
                                   {
                                       FileName = this.FileFulltextInfo.FileName,
                                       FileTextReader = this.FileFulltextInfo.FileTextReader
                                   }
            };
        }
    }
}