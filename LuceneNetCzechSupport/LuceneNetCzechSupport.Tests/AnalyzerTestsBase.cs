using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cz;
using LuceneNetCzechSupport.Analyzers;
using LuceneNetCzechSupport.Lucene;
using Xunit;

namespace LuceneNetCzechSupport.Tests
{
    public abstract class AnalyzerTestsBase
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

        [Fact]
        public void IsAnalyzerWorking()
        {
            Fulltext.AddDocToFulltext(TestFullTextDocument);

            var results = Fulltext.SearchIndex(TestFullTextDocumentSearchedText);
            
            Assert.NotEmpty(results);

            Console.WriteLine("found id: {0}", results[0]);
        }
    }
}