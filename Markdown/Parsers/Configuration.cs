using System;
using System.Collections.Generic;

namespace Markdown.Parsers
{
    class Configuration
    {
        private SortedDictionary<int, List<IMorphemeParser>> morphemeParsers;

        public static Configuration GetDefaultMdToHtmlConfiguration()
        {
            Configuration configuration = new Configuration();
            return configuration;
        }

        public IEnumerable<IMorphemeParser> GetMorphemeParsersByPriority()
        {
            throw new NotImplementedException();
        }

        public Configuration()
        {
            morphemeParsers = new SortedDictionary<int, List<IMorphemeParser>>();
        }

        public void Add(IMorphemeParser morphemeParser, int priority)
        {
            throw new NotImplementedException();
        }
    }
}
