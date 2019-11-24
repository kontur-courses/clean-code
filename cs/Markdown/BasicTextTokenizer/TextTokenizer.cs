using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.BasicTextTokenizer
{
    public class TextTokenizer
    {
        private readonly ITagClassifier[] classifiers;
        private readonly Func<string, int, bool> isClosingSequence;
        private readonly Func<string, int, bool> isControllingSequence;

        private readonly Func<string, int, bool> isEscapeSequence;
        private readonly Func<string, int, bool> isOpeningSequence;

        public TextTokenizer(ITagClassifier[] classifiers, Func<string, int, bool> isEscapeSequence)
        {
            this.classifiers = classifiers;
            this.isEscapeSequence = isEscapeSequence;
            isOpeningSequence = (text, position) => classifiers
                .Any(classifier => classifier.IsOpeningSequence(text, position));
            isClosingSequence = (text, position) => classifiers
                .Any(classifier => classifier.IsClosingSequence(text, position));
            isControllingSequence = (text, position) =>
                isOpeningSequence(text, position) || isClosingSequence(text, position);
        }

        public IEnumerable<Token> GetTokens(string rawText)
        {
            var openings = new Dictionary<ITagClassifier, Token>();
            var reader = new TokenReader(rawText);
            var tokens = new List<Token>();
            while (reader.HasData)
            {
                var newTokens = ReadUntilWithEscapeProcessing(reader);
                tokens.AddRange(newTokens);
                if (isClosingSequence(reader.Text, reader.Position))
                    ProcessClosingSequence(reader, openings, tokens);
                else if (isOpeningSequence(reader.Text, reader.Position))
                    ProcessOpeningSequence(reader, openings, tokens);
            }

            return tokens.OrderBy(t => t.Position);
        }

        private void ProcessOpeningSequence(TokenReader reader, Dictionary<ITagClassifier, Token> openings,
            List<Token> tokens)
        {
            var classifier = classifiers
                .OrderBy(c => c.Priority)
                .First(c => c.IsOpeningSequence(reader.Text, reader.Position));

            var tag = reader.ReadCount(classifier.TagLength);

            if (openings.ContainsKey(classifier))
            {
                tokens.Add(tag);
                return;
            }

            var opening = Token.CreateControllingToken(
                tag.Position, tag.Length, TokenType.Opening, classifier, null);
            openings[classifier] = opening;
            tokens.Add(opening);
        }

        private void ProcessClosingSequence(TokenReader reader, Dictionary<ITagClassifier, Token> openings,
            List<Token> tokens)
        {
            var classifier = classifiers
                .OrderByDescending(c => c.Priority)
                .First(c => c.IsClosingSequence(reader.Text, reader.Position));

            var tag = reader.ReadCount(classifier.TagLength);

            if (!openings.ContainsKey(classifier))
            {
                tokens.Add(tag);
                return;
            }

            var opening = openings[classifier];
            openings.Remove(classifier);
            var closing = Token.CreateControllingToken(
                tag.Position, tag.Length, TokenType.Ending, classifier, opening);
            opening.PairedToken = closing;
            tokens.Add(closing);
        }

        private List<Token> ReadUntilWithEscapeProcessing(TokenReader reader)
        {
            bool IsStopPositionWithEscape(string text, int position)
            {
                return isEscapeSequence(text, position)
                       || isControllingSequence(text, position);
            }

            var tokens = new List<Token>();
            var firstTime = true;
            do
            {
                Token escapedToken = null;
                if (!firstTime)
                {
                    reader.SkipCount(1);
                    escapedToken = reader.ReadCount(1);
                }

                var token = reader.ReadUntil(IsStopPositionWithEscape);
                if (escapedToken != null)
                    token = escapedToken.Concat(token);
                if (token.Length != 0)
                    tokens.Add(token);
                firstTime = false;
            } while (isEscapeSequence(reader.Text, reader.Position));

            return tokens;
        }
    }
}