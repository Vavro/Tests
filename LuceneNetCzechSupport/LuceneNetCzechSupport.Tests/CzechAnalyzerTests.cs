using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Cz;
using Lucene.Net.Documents;
using LuceneNetCzechSupport.Analyzers;
using LuceneNetCzechSupport.Lucene;
using Xunit;

namespace LuceneNetCzechSupport.Tests
{
    public class CzechAnalyzerTests : AnalyzerTestsBase
    {
        public CzechAnalyzerTests()
            : base(SupportedAnalyzers.CzechAnalyzer)
        {
        }
    }
}
