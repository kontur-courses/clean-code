using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private IStringTranslator stringTranslator;
        private ITokenTranslator tokenTranslator;

        public Md(IStringTranslator stringTranslator, ITokenTranslator tokenTranslator)
        {
            this.stringTranslator = stringTranslator;
            this.tokenTranslator = tokenTranslator;
        }

        public string Render(string markdown)
        {
            var tokens = stringTranslator.Translate(markdown);
            return tokenTranslator.Translate(tokens);
        }
    }
}
