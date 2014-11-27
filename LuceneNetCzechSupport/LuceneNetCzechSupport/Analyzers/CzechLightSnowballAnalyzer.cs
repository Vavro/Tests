using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Analysis.Standard;
using SF.Snowball.Ext;
using Snowball.Stemmer;
using Version = Lucene.Net.Util.Version;

namespace LuceneNetCzechSupport.Analyzers
{
    public class CzechLightSnowballAnalyzer : Analyzer
    {
        private readonly ISet<string> _stoptable;
        private readonly Version _matchVersion;

        public CzechLightSnowballAnalyzer(Version matchVersion, ISet<string> stopWords)
        {
            this._stoptable = CharArraySet.UnmodifiableSet(CharArraySet.Copy(stopWords));
            this._matchVersion = matchVersion;
        }

        /*
         * Creates a {@link TokenStream} which tokenizes all the text in the provided {@link Reader}.
         *
         * @return  A {@link TokenStream} built from a {@link StandardTokenizer} filtered with
         *             {@link StandardFilter}, {@link LowerCaseFilter}, and {@link StopFilter}
         */
        public override sealed TokenStream TokenStream(String fieldName, TextReader reader)
        {
            TokenStream result = new StandardTokenizer(_matchVersion, reader);
            result = new StandardFilter(result);
            result = new LowerCaseFilter(result);
            result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(_matchVersion), result, _stoptable);
            return result;
        }

        private class SavedStreams
        {
            protected internal Tokenizer Source;
            protected internal TokenStream Result;
        };

        /*
         * Returns a (possibly reused) {@link TokenStream} which tokenizes all the text in 
         * the provided {@link Reader}.
         *
         * @return  A {@link TokenStream} built from a {@link StandardTokenizer} filtered with
         *          {@link StandardFilter}, {@link LowerCaseFilter}, and {@link StopFilter}
         */
        public override TokenStream ReusableTokenStream(String fieldName, TextReader reader)
        {
            SavedStreams streams = (SavedStreams)PreviousTokenStream;
            if (streams == null)
            {
                streams = new SavedStreams();
                streams.Source = new StandardTokenizer(_matchVersion, reader);
                streams.Result = new StandardFilter(streams.Source);
                streams.Result = new LowerCaseFilter(streams.Result);
                streams.Result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(_matchVersion),
                                                streams.Result, _stoptable);
                streams.Result = new SnowballFilter(streams.Result, new CzechAggresiveStemmer());
                PreviousTokenStream = streams;
            }
            else
            {
                streams.Source.Reset(reader);
            }
            return streams.Result;
        }
    }
}
