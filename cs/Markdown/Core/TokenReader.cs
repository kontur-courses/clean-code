using Markdown.Extentions;
using System.Collections.Generic;
namespace Markdown
{
    public class TokenReader
    {
        private static HashSet<string> formattingCharacters = new HashSet<string> { "_", "__", "#", "\\" };

        public IEnumerable<Token> ReadTokens(string text)
        {
            var parserOperator = new ParserOperator();
            var splittedText = text
                            .SplitKeepSeparators(new[] { '_', '#', '\\' })
                            .UnionSameStringByTwo("#")
                            .OperateEscaped(formattingCharacters);
            foreach (var bigram in splittedText.GetBigrams())
                parserOperator.AddTokenPart(bigram);
            return parserOperator.GetTokens();
        }

        public static bool IsFormattingString(string c)
        {
            return formattingCharacters.Contains(c);
        }
    }
}
