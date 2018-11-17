using System.Collections.Generic;
using Markdown.TokenizerClasses.Scanners;

namespace Markdown.TokenizerClasses
{
    public class Tokenizer
    {
        private readonly List<IScanner> tokenScanners = new List<IScanner>
        {
            new TagScanner(),
            new PlainTextScanner()
        };

        public IEnumerable<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();
            Token token;

            do
            {
                token = GetNextToken(text);
                tokens.Add(token);
                text = text.Substring(token.Length, text.Length - token.Length);
            }
            while (token.Type != TokenType.EOF);

            if (tokens.Count > 0)
                tokens.RemoveAt(tokens.Count - 1);

            return tokens;
        }

        public Token GetNextToken(string text)
        {
            foreach (var scanner in tokenScanners)
            {
                if (scanner.TryScan(text, out var token))
                    return token;
            }

            return Token.EOF;
        }
    }
}