﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            private readonly string _luceneDir;
            private FSDirectory _directoryTemp;

            public LuceneIndexDir(string directoryName)
            {
                _luceneDir = directoryName;
            }

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

        public LuceneIndexer(Analyzer analyzer, string directoryName)
        {
            Analyzer = analyzer;
            var luceneDir = new LuceneIndexDir(directoryName);
            Index = luceneDir.Directory;
        }
        public const int MaxResults = 1000;

        public readonly Directory Index;


        public Analyzer Analyzer { get; private set; }

        public void AddDocument(Document doc)
        {

            //todo: use indexwriter for longer than one write
            using (var w = new IndexWriter(Index, Analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                w.AddDocument(doc);
                w.Optimize();
            }
        }

        public static Document CreateDocument(FullTextDocument insertedDocument)
        {
            var doc = new Document();
            doc.Add(new Field(FieldNames.Id, insertedDocument.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));

            var fileTextReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(insertedDocument.FileFulltextInfo.FileText)), Encoding.UTF8);

            doc.Add(new Field(FieldNames.FileText, fileTextReader));

            return doc;
        }

        public void Clear()
        {
            using (var w = new IndexWriter(Index,Analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                w.DeleteAll();
                w.Optimize();
            }
        }
    }
}
