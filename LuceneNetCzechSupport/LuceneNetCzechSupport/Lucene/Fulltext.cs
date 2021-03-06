﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace LuceneNetCzechSupport.Lucene
{
    public class Fulltext
    {
        public class Statistics
        {
            public Statistics()
            {
                LastIndexingTime = new TimeSpan();
                LastSearchTime = new TimeSpan();
            }

            public TimeSpan LastIndexingTime { get; set; }
            public TimeSpan LastSearchTime { get; set; }

            public string LastSearchText { get; set; }
        }

        private readonly LuceneIndexer _indexer;

        public Fulltext(Analyzer analyzer)
        {
            _indexer = new LuceneIndexer(analyzer, "index_" + analyzer.GetType().Name);
            Stats = new Statistics();
        }

        public Statistics Stats { get; private set; }

        public void AddDocToFulltext(FullTextDocument insertedDocument)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var doc = LuceneIndexer.CreateDocument(insertedDocument);
           
            _indexer.AddDocument(doc);

            stopwatch.Stop();
            Stats.LastIndexingTime = stopwatch.Elapsed;
        }

        //todo: return type
        public List<string> SearchIndex(string searchedText)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //Console.WriteLine("Searching text: {0}", searchedText);
            var queryParser = new QueryParser(Version.LUCENE_30, LuceneIndexer.FieldNames.FileText, _indexer.Analyzer);
            var fullTextQuery = queryParser.Parse(searchedText);
            Stats.LastSearchText = fullTextQuery.ToString();

            var mainQuery = fullTextQuery;

            var searcher = new IndexSearcher(_indexer.Index);
            var topDocs = searcher.Search(mainQuery, LuceneIndexer.MaxResults);
            var resultIds = new List<string>();

            for (int i = 0; i < topDocs.TotalHits; i++)
            {
                Document doc = searcher.Doc(i);
                
                var id = doc.Get(LuceneIndexer.FieldNames.Id);

                //Console.WriteLine("id: {0}", id);
                resultIds.Add(id);
            }

            stopwatch.Stop();
            Stats.LastSearchTime = stopwatch.Elapsed;

            return resultIds;
        }


        public void Clear()
        {
            _indexer.Clear();
        }
    }
}
