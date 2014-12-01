using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using LuceneNetCzechSupport.Analyzers;
using Xunit;

namespace LuceneNetCzechSupport.Tests
{
    public class CzechLightSnowballAnalyzerTests : AnalyzerTestsBase
    {
        public CzechLightSnowballAnalyzerTests() : base(SupportedAnalyzers.CzechLightSnowballAnalyzer)
        {
        }
    }
}
