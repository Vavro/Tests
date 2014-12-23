using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cz;
using LuceneNetCzechSupport.Analyzers;
using LuceneNetCzechSupport.Lucene;
using LuceneNetCzechSupport.Tests.Extensions;
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
                                                                 FileTextReader = "Testovací text".AsStreamReader()
                                                             }
                                                         };

        protected string TestFullTextDocumentSearchedText = "Testovací";

        public AnalyzerTestsBase(Analyzer analyzer)
        {
            Fulltext = new Fulltext(analyzer);
            Fulltext.Clear();
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

        [Fact]
        public void CanIndexAndRetrieveAllCzechWords()
        {
            var czechStopWords = new HashSet<string>(CzechAnalyzer.CZECH_STOP_WORDS);

            using (var reader = File.OpenText(@"TestFiles\Czech.3-2-3.txt"))
            {
                var text = reader;

                var doc = new FullTextDocument()
                          {
                              Id = @"TestFiles\Czech.3-2-3.txt", 
                              FileFulltextInfo =
                              {
                                  FileName = "Czech.3-2-3.txt", FileTextReader = text
                              }
                          };

                Fulltext.AddDocToFulltext(doc);
                
                Console.WriteLine("Indexing took: {0}", Fulltext.Stats.LastIndexingTime);
            }

            using (var reader = File.OpenText(@"TestFiles\Czech.3-2-3.txt"))
            {
                string line;
                var stats = new StatisticsCaputerer();
                while ((line = reader.ReadLine()) != null)
                {
                    if (czechStopWords.Contains(line))
                    {
                        //don't search for stop words
                        continue;
                    }

                    var results = Fulltext.SearchIndex(line);
                    stats.AddTime(Fulltext.Stats.LastSearchTime);

                    Assert.True(results.Any(), String.Format("Couldn't find line {0}", line));
                }
            }
        }
    }

    class StatisticsCaputerer
    {
        private List<TimeSpan> _spans;

        public StatisticsCaputerer()
        {
            _spans = new List<TimeSpan>();
            MaxSpan = new TimeSpan(0);
            MinSpan = new TimeSpan(long.MaxValue);
        }

        public void AddTime(TimeSpan span)
        {
            if (span > MaxSpan)
            {
                MaxSpan = span;
            }

            if (span < MinSpan)
            {
                MinSpan = span;
            }

            _spans.Add(span);
        }

        public TimeSpan MinSpan { get; set; }

        public TimeSpan MaxSpan { get; set; }

        public override string ToString()
        {
            return String.Format("Average: {0}, Max: {1}, Min: {2}", 
                new TimeSpan(_spans.Aggregate((s1, s2) => s1 + s2).Ticks/_spans.Count), MaxSpan, MinSpan);
        }
    }
}