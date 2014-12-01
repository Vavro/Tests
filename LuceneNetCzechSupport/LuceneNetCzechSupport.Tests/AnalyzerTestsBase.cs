using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cz;
using LuceneNetCzechSupport.Analyzers;
using LuceneNetCzechSupport.Lucene;

namespace LuceneNetCzechSupport.Tests
{
    public class AnalyzerTestsBase
    {
        protected FullTextDocument TestFullTextDocument = new FullTextDocument()
                                                         {
                                                             Id = "testId/1",
                                                             FileFulltextInfo =
                                                             {
                                                                 FileName = "TestFileName",
                                                                 FileText = "Testovací text"
                                                             }
                                                         };

        protected string TestFullTextDocumentSearchedText = "Testovací";

        public AnalyzerTestsBase(Analyzer analyzer)
        {
            Fulltext = new Fulltext(analyzer);
        }

        protected Fulltext Fulltext { get; private set; }
    }
}