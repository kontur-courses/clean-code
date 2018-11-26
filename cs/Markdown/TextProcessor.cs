using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class TextProcessor
    {
        private readonly List<ITextProcessorRule> rules;

        public readonly string Text;
        internal List<Delimiter> Delimiters;

        internal TextProcessor(string text, List<Delimiter> delimiters = null, List<ITextProcessorRule> rules = null)
        {
            Text = text;
            this.rules = rules ?? this.rules;
            Delimiters = delimiters ?? new List<Delimiter>();
        }

        internal ITextProcessorRule GetSuitableRule(int position, string text)
        {
            return rules.FirstOrDefault(rule => rule.Check(position, text));
        }

        internal ITextProcessorRule GetSuitableRule(Delimiter delimiter)
        {
            return rules.FirstOrDefault(r => r.Check(delimiter));
        }

        internal TextProcessor GetDelimiterPositions()
        {
            var text = Text;
            Delimiters = new List<Delimiter>();
            for (var position = 0; position < text.Length; position++)
            {
                var rule = GetSuitableRule(position, text);
                if (rule == null)
                    continue;
                var delimiter = rule.ProcessIncomingChar(position, text, out var amountToSkip);
                position += amountToSkip;
                Delimiters.Add(delimiter);
            }

            return this;
        }

        internal TextProcessor RemoveEscapedDelimiters()
        {
            Delimiters = Delimiters.Select(d => GetSuitableRule(d)
                                               .Escape(d, Text))
                                   .Where(p => p != null)
                                   .ToList();
            return this;
        }

        internal TextProcessor RemoveNonValidDelimiters()
        {
            var text = Text;
            Delimiters = Delimiters.Where(d => GetSuitableRule(d)
                                              .IsValid(d, text))
                                   .ToList();
            return this;
        }

        internal TextProcessor ValidatePairs()
        {
            var stacks = new Dictionary<string, Stack<Delimiter>>();
            foreach (var delimiter in Delimiters)
                stacks[delimiter.Value] = new Stack<Delimiter>();

            foreach (var delimiter in Delimiters)
            {
                var rule = GetSuitableRule(delimiter);
                var isValidSecond = rule.IsValidClosing(delimiter, Text);

                var stack = stacks[delimiter.Value];
                if (isValidSecond &&
                    stack.Count > 0 &&
                    stack.Peek()
                         .Value ==
                    delimiter.Value &&
                    rule.IsValidOpening(stack.Peek(), Text))
                {
                    var firstDelimiter = stack.Pop();
                    firstDelimiter.Partner = delimiter;
                    delimiter.Partner = firstDelimiter;
                    delimiter.IsClosing = firstDelimiter.IsOpening = true;
                }
                else
                {
                    stack.Push(delimiter);
                }
            }

            var rest = stacks.Values.SelectMany(s => s.ToArray())
                             .ToHashSet();
            Delimiters.RemoveAll(d => rest.Contains(d));

            return this;
        }

        internal IEnumerable<Token> GetTokensFromDelimiters()
        {
            if (!Delimiters.Any())
                return new List<Token> {new StringToken(0, Text.Length, Text)};

            var tokens = new LinkedList<Token>();
            Token currentParentToken = null;
            Delimiter endOfParent = null;
            foreach (var delimiter in Delimiters)
            {
                var rule = GetSuitableRule(delimiter);
                var token = rule.GetToken(delimiter, Text);

                if (token != null)
                {
                    if (currentParentToken == null)
                    {
                        currentParentToken = token;
                        endOfParent = delimiter.Partner;
                        tokens.AddLast(token);
                    }

                    if (delimiter.Partner.Position >= endOfParent.Position)
                        continue;
                    token.ParentToken = currentParentToken;
                    if (currentParentToken.InnerTokens == null)
                        currentParentToken.InnerTokens = new List<Token>();
                    currentParentToken.InnerTokens.Add(token);
                }

                else if (currentParentToken != null)
                {
                    if (delimiter.Position < endOfParent.Position)
                        continue;
                    currentParentToken = null;
                    endOfParent = null;
                }
            }

            return InsertStringTokens(tokens);
        }

        private IEnumerable<Token> InsertStringTokens(LinkedList<Token> tokens)
        {
            var currentToken = tokens.First;

            tokens.AddLast(new PairedTagToken(Text.Length, 0, "", ""));
            var start = 0;
            while (currentToken != null)
            {
                var end = currentToken.Value.Position;
                var length = end - start;
                var value = Text.Substring(start, length);
                if (end != start) tokens.AddBefore(currentToken, new StringToken(start, length, value));

                start = end + currentToken.Value.Length;
                currentToken = currentToken.Next;
            }

            tokens.RemoveLast();

            return tokens.ToList();
        }
    }
}
