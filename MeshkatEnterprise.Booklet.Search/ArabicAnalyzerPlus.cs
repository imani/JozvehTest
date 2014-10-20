using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.AR;
using Lucene.Net.Util;

namespace MeshkatEnterprise.Booklet.Search
{
    public class ArabicAnalyzerPlus : Analyzer
    {
        private readonly bool _doStem;
        private readonly ISet<string> _stopWords;
        private readonly Version _version;

        public ArabicAnalyzerPlus(Version version, ISet<string> sw, bool stem = true)
        {
            _version = version;
            _stopWords = sw;
            _doStem = stem;
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            TokenStream result = new ArabicLetterTokenizer(reader);
            result = new StopFilter(true, result, _stopWords);
            result = new ArabicPlusNormalizationFilter(result);
            result = new ArabicNormalizationFilter(result);
            if (_doStem)
                result = new ArabicStemFilter(result);
            return result;
        }
    }
}