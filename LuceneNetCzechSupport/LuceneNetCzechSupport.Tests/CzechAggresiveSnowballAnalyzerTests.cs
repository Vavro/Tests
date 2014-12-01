using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuceneNetCzechSupport.Analyzers;
using Xunit;

namespace LuceneNetCzechSupport.Tests
{
    public class CzechAggresiveSnowballAnalyzerTests : AnalyzerTestsBase
    {
        public CzechAggresiveSnowballAnalyzerTests()
            : base(SupportedAnalyzers.CzechAggresiveSnowballAnalyzer)
        {
        }
    }
}
