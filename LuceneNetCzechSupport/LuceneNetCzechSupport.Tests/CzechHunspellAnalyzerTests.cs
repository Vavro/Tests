﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuceneNetCzechSupport.Analyzers;
using Xunit;

namespace LuceneNetCzechSupport.Tests
{
    public class CzechHunspellAnalyzerTests : AnalyzerTestsBase
    {
        public CzechHunspellAnalyzerTests()
            : base(SupportedAnalyzers.CzechHunspellAnalyzer)
        {
        }

        [Fact]
        public void IsAnalyzerWorking()
        {
            Fulltext.AddDocToFulltext(TestFullTextDocument);

            Fulltext.SearchIndex(TestFullTextDocumentSearchedText);
        }
    }
}
