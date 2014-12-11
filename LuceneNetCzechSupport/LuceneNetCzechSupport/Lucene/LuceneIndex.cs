using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cz;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;

namespace LuceneNetCzechSupport.Lucene
{
    //todo: detect corrupted indexes
    //todo: add rebuild index possibilities - for corrupted indexes
    public class LuceneIndexer
    {
        private class LuceneIndexDir
        {
            private string _luceneDir = "lucene_index";
            private FSDirectory _directoryTemp;
            public FSDirectory Directory
            {
                get
                {
                    if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                    if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                    _directoryTemp.ClearLock("write.lock");

                    return _directoryTemp;
                }
            }
        }

        public static class FieldNames
        {
            public const string FileText = "FileText";
            public const string Id = "Id";
        }

        public LuceneIndexer(Analyzer analyzer)
        {
            Analyzer = analyzer;
            var luceneDir = new LuceneIndexDir();
            Index = luceneDir.Directory;
        }
        public const int MaxResults = 1000;

        public readonly Directory Index;


        public Analyzer Analyzer { get; private set; }

        public void AddDocument(Document doc)
        {

            //todo: use indexwriter for longer than one write
            using (var w = new IndexWriter(Index, Analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                w.AddDocument(doc);
                w.Optimize();
            }
        }

        public static Document CreateDocument(FullTextDocument insertedDocument)
        {
            var doc = new Document();
            doc.Add(new Field(FieldNames.Id, insertedDocument.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));

            string text = insertedDocument.FileFulltextInfo.FileText;

            doc.Add(new Field(FieldNames.FileText, text, Field.Store.NO, Field.Index.ANALYZED));

            return doc;
        }
    }
}
