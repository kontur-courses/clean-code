using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class Md
    {
        public string Render(string paragraph)
        {
            var analyzer = new LexicalAnalyzer();
            var tokens = analyzer.Analyze(paragraph);
            var parser = new TokenParser(tokens, paragraph);
            var tags = parser.Parse();
            var tagInserter = new TagInserter();
            var HTMLtext = tagInserter.Insert(paragraph, tags.ToDictionary(x=>x.Item1, x=>x.Item2));
            return HTMLtext;
        }

    }

    //public enum Tag
    //{
    //    Em,
    //    Em_close,
    //    Strong,
    //    Strong_close,
    //    Empty

    //}

}
