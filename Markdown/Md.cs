using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Parsers;

namespace Markdown
{
    class Md
    {
        private Configuration configuration;
        public Md(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public Md() : this(Configuration.GetDefaultMdToHtmlConfiguration())
        {
        }

        public string Render(string markdownString)
        {
            return GetTranslatedString(GetParsedMorphemes(markdownString));
        }

        public Stack<StringBuilder> GetParsedMorphemes(string markdownString)
        {
            throw new NotImplementedException();
        }

        public string GetTranslatedString(Stack<StringBuilder> parsedString)
        {
            var result = new StringBuilder();
            foreach (var morpheme in parsedString)
            {
                result.Append(morpheme);
            }

            return result.ToString();
        }
    }
}
