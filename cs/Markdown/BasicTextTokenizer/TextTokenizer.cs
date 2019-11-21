using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.BasicTextTokenizer
{
    public class TextTokenizer
    {
        private readonly ITagClassifier[] classifiers;

        private readonly Func<string, int, bool> isEscapeSequence;
        private readonly Func<string, int, bool> isOpeningSequence;
        private readonly Func<string, int, bool> isClosingSequence;
        private readonly Func<string, int, bool> isControllingSequence;

        public TextTokenizer(ITagClassifier[] classifiers, Func<string, int, bool> isEscapeSequence)
        {
            this.classifiers = classifiers;
            this.isEscapeSequence = isEscapeSequence;
            isOpeningSequence = (s, i) => classifiers.Select(c => c.IsOpeningSequence(s, i)).Any(t => t);
            isClosingSequence = (s, i) => classifiers.Select(c => c.IsClosingSequence(s, i)).Any(t => t);
            isControllingSequence = (s, i) => isOpeningSequence(s, i) || isClosingSequence(s, i);
        }

        public IEnumerable<Token> GetTokens(string rawText)
        {
            var openings = new Dictionary<ITagClassifier, Token>();
            var reader = new TokenReader(rawText);
            var tokens = new List<Token>();
            while (reader.HasData())
            {
                var newTokens = reader.ReadUntilWithEscapeProcessing(isControllingSequence, isEscapeSequence);
                tokens.AddRange(newTokens);
                if (isOpeningSequence(reader.Text, reader.Position))
                    ProcessOpeningSequence(reader, openings, tokens);
                else if (isClosingSequence(reader.Text, reader.Position))
                    ProcessClosingSequence(reader, openings, tokens);
            }
            return tokens.OrderBy(t => t.Position);
        }

        private void ProcessOpeningSequence(TokenReader reader, Dictionary<ITagClassifier, Token> openings,
            List<Token> tokens)
        {
            var classifier = classifiers.First(c => c.IsOpeningSequence(reader.Text, reader.Position));
            var sequence = ReadSequenceToken(reader);

            if (openings.ContainsKey(classifier))
            {
                tokens.Add(sequence);
            }
            else
            {
                var opening = new Token(sequence.Position, sequence.Length, TokenType.Opening, classifier);
                openings[classifier] = opening;
                tokens.Add(opening);
            }
        }

        private void ProcessClosingSequence(TokenReader reader, Dictionary<ITagClassifier, Token> openings,
            List<Token> tokens)
        {
            var classifier = classifiers.OrderBy(c => c.Priority).First(c => c.IsClosingSequence(reader.Text, reader.Position));
            var sequence = ReadSequenceToken(reader);

            if (!openings.ContainsKey(classifier))
            {
                tokens.Add(sequence);
            }
            else
            {
                var opening = openings[classifier];
                openings.Remove(classifier);
                var closing = new Token(sequence.Position, sequence.Length, TokenType.Ending, classifier, opening);
                opening.PairedToken = closing;
                tokens.Add(closing);
            }
        }

        private Token ReadSequenceToken(TokenReader reader)
        {
            return reader.ReadWhile(c => c == '_');
        }
    }
}