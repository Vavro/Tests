
using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Hunspell;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Snowball.Stemmer;
using Version = Lucene.Net.Util.Version;

namespace LuceneNetCzechSupport.Analyzers
{
    public class CzechHunspellAnalyzer : Analyzer
    {
        private readonly ISet<string> _stoptable;
        private readonly Version _matchVersion;

        public CzechHunspellAnalyzer(Version matchVersion, ISet<string> stopWords)
        {
            this._stoptable = CharArraySet.UnmodifiableSet(CharArraySet.Copy(stopWords));
            this._matchVersion = matchVersion;
        }

        public override sealed TokenStream TokenStream(string fieldName, TextReader reader)
        {
            TokenStream result = new StandardTokenizer(_matchVersion, reader);
            result = new StandardFilter(result);
            result = new LowerCaseFilter(result);
            result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(_matchVersion), result, _stoptable);
            result = new HunspellStemFilter(result, new HunspellDictionary(File.OpenRead("Stemmers/Hunspell/cs_CZ.aff"), File.OpenRead("Stemmers/Hunspell/cs_CZ.dic")));
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
        public override TokenStream ReusableTokenStream(string fieldName, TextReader reader)
        {
            var streams = (CzechHunspellAnalyzer.SavedStreams)PreviousTokenStream;
            if (streams == null)
            {
                streams = new CzechHunspellAnalyzer.SavedStreams();
                streams.Source = new StandardTokenizer(_matchVersion, reader);
                streams.Result = new StandardFilter(streams.Source);
                streams.Result = new LowerCaseFilter(streams.Result);
                streams.Result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(_matchVersion),
                                                streams.Result, _stoptable);
                streams.Result = new HunspellStemFilter(streams.Result, new HunspellDictionary(File.OpenRead("Stemmers/Hunspell/cs_CZ.aff"), File.OpenRead("Stemmers/Hunspell/cs_CZ.dic")));
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