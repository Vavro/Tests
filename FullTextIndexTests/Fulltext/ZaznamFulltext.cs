﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Aspose.Pdf;
using Aspose.Pdf.Generator;
using FullTextIndexTests.IFilter;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cz;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Vectorhighlight;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Document = Lucene.Net.Documents.Document;
using Version = Lucene.Net.Util.Version;

namespace FullTextIndexTests.Fulltext
{
    public class LuceneIndexer
    {
        public static class FieldNames
        {
            public const string FileText = "FileText";
            public const string Id = "Id";
        }

        public const int MaxResults = 1000;

        //todo: switch to persistent directory
        public readonly Directory Index = new RAMDirectory();
        
        //todo: add better czech analyzer with stemming
        public readonly CzechAnalyzer Analyzer = new CzechAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        
        public void AddDocument(Document doc)
        {
            
            //todo: use indexwriter for longer than one write
            using (IndexWriter w = new IndexWriter(Index, Analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                w.AddDocument(doc);
                w.Optimize();
            }
        }

        public static Document CreateDocument(FullTextDocument insertedDocument)
        {
            var doc = new Document();
            doc.Add(new Field(FieldNames.Id, insertedDocument.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));

            string text = ParseHelper.ParseIFilter(insertedDocument.FileFulltextInfo.FileStream, insertedDocument.FileFulltextInfo.FileName);

            doc.Add(new Field(FieldNames.FileText, text, Field.Store.YES, Field.Index.ANALYZED));

            return doc;
        }
    }

    public class ZaznamFulltext
    {
        public static class ZaznamFieldNames
        {
            public const string Vytvoreno = "Vytvoreno";
        }

        private readonly LuceneIndexer _indexer;

        public ZaznamFulltext()
        {
            _indexer = new LuceneIndexer();
        }

        public void AddZaznamToFulltext(ZaznamFulltextDocument insertedDocument)
        {
            var doc = LuceneIndexer.CreateDocument(insertedDocument);
            //todo: numeric index of date should be faster, but should try both
            //var dateField = DateTools.DateToString(insertedDocument.ZaznamMetadata.VytvoreniZaznamu, DateTools.Resolution.DAY); //todo: date resolution?
            doc.Add(new NumericField(ZaznamFieldNames.Vytvoreno, 1, Field.Store.YES, true).SetLongValue(insertedDocument.ZaznamMetadata.VytvoreniZaznamu.Ticks));

            _indexer.AddDocument(doc);
        }

        public void UpdateZaznamMetadata(string id, ZaznamMetadata metadata)
        {

        }

        public void UpdateZaznamInFulltext(ZaznamFulltextDocument updatedDocument)
        {

        }

        //todo: return type
        //todo: extending more metadata
        public void SearchIndex(DateTime? from, DateTime? to, string searchedText)
        {
            var fromLong = GetTicksOrNull(from);
            var toLong = GetTicksOrNull(to);

            var vytvorenoRangeQuery = NumericRangeQuery.NewLongRange(ZaznamFieldNames.Vytvoreno, fromLong, toLong, true, true);
            var queryParser = new QueryParser(Version.LUCENE_30, LuceneIndexer.FieldNames.FileText, _indexer.Analyzer);
            var fullTextQuery = queryParser.Parse(searchedText);

            var mainQuery = new BooleanQuery();
            mainQuery.Add(vytvorenoRangeQuery, Occur.MUST);
            mainQuery.Add(fullTextQuery, Occur.MUST);

            var searcher = new IndexSearcher(_indexer.Index);
            var topDocs = searcher.Search(mainQuery, LuceneIndexer.MaxResults);
            for (int i = 0; i < topDocs.TotalHits; i++)
            {
                Document doc = searcher.Doc(i);
                string ticksString = doc.Get(ZaznamFieldNames.Vytvoreno);
                long ticks = long.Parse(ticksString);
                var vytvoreno = new DateTime(ticks);

                var text = doc.Get(LuceneIndexer.FieldNames.FileText);
                var id = doc.Get(LuceneIndexer.FieldNames.Id);

                Console.WriteLine("id: {0}", id);
                Console.WriteLine("vytvoreno: {0}", vytvoreno);
                Console.WriteLine("text : {0}", text);

            }
        }

        private static long? GetTicksOrNull(DateTime? dateTime)
        {
            long? ticks = null;
            if (dateTime.HasValue)
            {
                ticks = dateTime.Value.Ticks;
            }
            return ticks;
        }
    }
}
