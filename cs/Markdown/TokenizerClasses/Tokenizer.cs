using System;
using System.Collections.Generic;
using Markdown.TokenizerClasses.Scanners;

namespace Markdown.TokenizerClasses
{
    public class Tokenizer
    {
        private List<Func<string, Token>> tokenScanners = new List<Func<string, Token>>
        {
            new TagScanner().Scan,
            new PlainTextScanner().Scan
        };

        public TokenList Tokenize(string text)
        {
            var tokens = TokenizeToList(text);

            return new TokenList(tokens);
        }

        public List<Token> TokenizeToList(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<Token> {new Token("EOF", "")};

            Token token;
            var tokens = new List<Token>();

            do
            {
                token = ScanToken(text);
                tokens.Add(token);
                text = text.Substring(token.Value.Length, text.Length - token.Value.Length);
            } while (!string.IsNullOrEmpty(text));

            return tokens;
        }

        public Token ScanToken(string text)
        {
            Token token = null;
            foreach (var scanner in tokenScanners)
            {
                token = scanner(text);

                if (token != null)
                    break;
            }

            return token;
        }
    }
}
