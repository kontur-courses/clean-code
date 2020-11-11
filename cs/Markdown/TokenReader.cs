using Markdown.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TokenReader
    {
        private static HashSet<string> formattingCharacters = new HashSet<string> { "_", "__", "#" };

        public IEnumerable<Token> ReadTokens(string text)
        {
            var pareserOperator = new ParserOperator();
            foreach (var part in text.SplitKeppSeparators(new[] { '_', '#' }))
                pareserOperator.AddTokenPart(part);
            return pareserOperator.GetTokens();
        }

        public static bool IsFormattingString(string c)
        {
            return formattingCharacters.Contains(c);
        }
    }
}
