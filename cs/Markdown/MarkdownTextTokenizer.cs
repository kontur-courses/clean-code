using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.BasicTextTokenizer;

namespace Markdown
{
    public class MarkdownTextTokenizer
    {
        private readonly TextTokenizer tokenizer;

        public MarkdownTextTokenizer()
        {
            var classifiers = new ITagClassifier[] { new ItalicTagClassifier(), new BoldTagClassifier() };
            const char escapeSymbol = '\\';
            var escapableSymbols = new[] { '_', '\\' };
            bool IsEscapeSequence(string s, int i) => i + 1 < s.Length
                                                      && s[i] == escapeSymbol
                                                      && escapableSymbols.Contains(s[i + 1]);
            tokenizer = new TextTokenizer(classifiers, IsEscapeSequence);
        }

        public IEnumerable<FormattedToken> GetTokens(string rawText)
        {
            var tokens = tokenizer.GetTokens(rawText).ToList();
            return GetFormattedTokens(tokens);
        }

        private IEnumerable<FormattedToken> GetFormattedTokens(List<Token> tokens)
        {
            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (token.Type == TokenType.Text || token.Type == TokenType.Opening && token.PairedToken == null)
                    yield return FormattedToken.GetRawFormattedToken(token);
                else if (token.Type == TokenType.Opening)
                {
                    var (innerFormattedToken, newPosition) = ConstructFormattedToken(tokens, i);
                    yield return innerFormattedToken;
                    i = newPosition;
                }
            }
        }

        private Tuple<FormattedToken, int> ConstructFormattedToken(List<Token> tokens, int openingPosition)
        {
            var token = tokens[openingPosition];
            var classifier = token.Classifier;
            var position = openingPosition;
            var subTokens = new List<FormattedToken>();
            while (true)
            {
                position++;
                var currentToken = tokens[position];
                if (currentToken == token.PairedToken)
                    break;
                if (IsRawToken(currentToken) || !IsAllowedFormattedSubToken(currentToken, classifier))
                    subTokens.Add(FormattedToken.GetRawFormattedToken(currentToken));
                else if (currentToken.Type == TokenType.Opening)
                {
                    var (innerFormattedToken, newPosition) = ConstructFormattedToken(tokens, position);
                    position = newPosition;
                    subTokens.Add(innerFormattedToken);
                }
            }

            var formattedToken = new FormattedToken(
                subTokens, classifier.Type,
                subTokens[0].StartIndex, subTokens[subTokens.Count - 1].EndIndex);
            return Tuple.Create(formattedToken, position);
        }

        private bool IsRawToken(Token token)
        {
            return token.Type == TokenType.Text ||
                   (token.Type == TokenType.Ending || token.Type == TokenType.Opening) && token.PairedToken == null;
        }

        private bool IsAllowedFormattedSubToken(Token token, ITagClassifier fatherClassifier)
        {
            if (token.Classifier == null)
                return false;
            var allowedSubClassifiers = fatherClassifier.AllowedSubClassifiers;
            return allowedSubClassifiers != null && allowedSubClassifiers.Contains(token.Classifier.GetType());
        }
    }
}
