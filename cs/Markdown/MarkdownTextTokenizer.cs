using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.BasicTextTokenizer;

namespace Markdown
{
    public class MarkdownTextTokenizer
    {
        private readonly TextTokenizer tokenizer;
        private const char EscapeSymbol = '\\';
        private readonly char[] escapableSymbols = {'_', EscapeSymbol, '[', ']', '(', ')'};
        private bool IsEscapeSequence(string text, int position) => position + 1 < text.Length
                                                  && text[position] == EscapeSymbol
                                                  && escapableSymbols.Contains(text[position + 1]);

        public MarkdownTextTokenizer()
        {
            var classifiers = new ITagClassifier[]
            {
                new ItalicTagClassifier(),
                new BoldTagClassifier(),
                new LinkTextTagClassifier(),
                new LinkUriTagClassifier()
            };
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
                if (token.Type != TokenType.Opening || token.Type == TokenType.Opening && token.PairedToken == null)
                    yield return FormattedToken.GetTextToken(token);
                else if (token.Type == TokenType.Opening)
                {
                    var (formattedToken, newPosition) = ConstructFormattedToken(tokens, i);
                    yield return formattedToken;
                    i = newPosition;
                }
            }
        }

        private (FormattedToken token, int newPosition) ConstructFormattedToken(
            List<Token> tokens, int openingPosition)
        {
            var token = tokens[openingPosition];
            var classifier = token.Classifier;
            if (classifier.HasFirstPart)
                return (FormattedToken.GetTextToken(token), openingPosition);
            var position = openingPosition;
            var subTokens = new List<FormattedToken>();
            while (true)
            {
                position++;
                var currentToken = tokens[position];
                if (currentToken == token.PairedToken)
                {
                    if (classifier.HasSecondPart)
                        return ConstructTwoPartedToken(tokens, openingPosition, position, 
                            subTokens, classifier);
                    break;
                }
                if (IsRawToken(currentToken) || !IsAllowedFormattedSubToken(currentToken, classifier))
                    subTokens.Add(FormattedToken.GetTextToken(currentToken));
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
            return (formattedToken, position);
        }

        private (FormattedToken token, int newPosition) ConstructTwoPartedToken(
            List<Token> tokens,
            int firstPartOpeningPosition,
            int firstPartClosingPosition, 
            List<FormattedToken> firstPartSubTokens, 
            ITagClassifier classifier)
        {
            var secondPartOpeningPosition = firstPartClosingPosition + 1;

            if (!HasSecondPart(secondPartOpeningPosition, tokens, classifier.SecondPartType))
            {
                firstPartSubTokens.Insert(0, FormattedToken.GetTextToken(tokens[firstPartOpeningPosition]));
                firstPartSubTokens.Add(FormattedToken.GetTextToken(tokens[firstPartClosingPosition]));
                var formattedToken = FormattedToken.GetTokenFromSubTokens(firstPartSubTokens, FormattedTokenType.Raw);
                return (formattedToken, firstPartClosingPosition);
            }

            var secondPartOpeningToken = tokens[secondPartOpeningPosition];
            var secondPartSubTokens = new List<FormattedToken>();
            var position = secondPartOpeningPosition + 1;
            var currentToken = tokens[position];
            while (currentToken != secondPartOpeningToken.PairedToken)
            {
                secondPartSubTokens.Add(FormattedToken.GetTextToken(currentToken));
                position++;
                currentToken = tokens[position];
            }

            var secondPartFormattedToken = FormattedToken.GetTokenFromSubTokens(
                secondPartSubTokens, secondPartOpeningToken.Classifier.Type);
           
            var firstPartFormattedToken = FormattedToken.GetTokenFromSubTokens(
                firstPartSubTokens, classifier.Type);
            firstPartSubTokens.Add(secondPartFormattedToken);

            return (firstPartFormattedToken, position);
        }

        private bool HasSecondPart(int secondPartPosition, List<Token> tokens, Type secondPartType)
        {
            return secondPartPosition < tokens.Count
                   && tokens[secondPartPosition].Classifier != null
                   && tokens[secondPartPosition].Classifier.GetType() == secondPartType
                   && tokens[secondPartPosition].PairedToken != null;
        }

        private static bool IsRawToken(Token token)
        {
            return token.Type == TokenType.Text ||
                   (token.Type == TokenType.Ending || token.Type == TokenType.Opening) && token.PairedToken == null;
        }

        private static bool IsAllowedFormattedSubToken(Token token, ITagClassifier fatherClassifier)
        {
            if (token.Classifier == null)
                return false;
            var allowedSubClassifiers = fatherClassifier.AllowedSubClassifiers;
            return allowedSubClassifiers != null && allowedSubClassifiers.Contains(token.Classifier.GetType());
        }
    }
}
