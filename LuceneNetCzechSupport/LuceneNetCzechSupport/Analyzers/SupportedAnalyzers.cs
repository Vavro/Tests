using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cz;
using Version = Lucene.Net.Util.Version;

namespace LuceneNetCzechSupport.Analyzers
{
    public class SupportedAnalyzers
    {
        public static CzechAnalyzer CzechAnalyzer = new CzechAnalyzer(Version.LUCENE_30, CzechAnalyzer.getDefaultStopSet());

        public static CzechAggresiveSnowballAnalyzer CzechAggresiveSnowballAnalyzer = new CzechAggresiveSnowballAnalyzer(Version.LUCENE_30, CzechAnalyzer.getDefaultStopSet());

        public static CzechLightSnowballAnalyzer CzechLightSnowballAnalyzer = new CzechLightSnowballAnalyzer(Version.LUCENE_30, CzechAnalyzer.getDefaultStopSet());

        //todo: research KStem filter 
        //todo: research PorterStemFilter
        //todo: research HunspellStemmer

    }
}
