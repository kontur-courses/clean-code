using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class TextParser
    {
        private readonly List<ILexerRule> rules;

        public TextParser(IEnumerable<ILexerRule> rules = null)
        {
            this.rules = rules?.ToList() ?? new List<ILexerRule>();
        }

        public void AddRule(ILexerRule rule) => rules.Add(rule);

        /// <summary>
        ///     Тут будет какой-то конечный автомат по символам строки.
        ///     Для каждого нового символа будем выбирать обработчик из LexerRules.
        ///     Идея для парных разделителей в том, чтобы сделать нечто вроде алгоритма Дейкстры для ОПН -
        ///     - складывать в стек и "закрывать", как скобки.
        /// </summary>
        public IEnumerable<Token> Parse(string text)
        {
            var delimiters = GetDelimiterPositions(text);
            delimiters = RemoveEscapedDelimiters(delimiters, text);
            delimiters = RemoveNonValidDelimiters(delimiters, text);
            delimiters = ValidatePairs(delimiters, text);

            return GetTokensFromDelimiters(delimiters, text);
        }

        internal List<Delimiter> ValidatePairs(List<Delimiter> delimiters, string text)
        {
            var stack =  new Stack<Delimiter>();
            foreach (var delimiter in delimiters)
                if (stack.Count > 0 &&
                    stack.Peek()
                         .Value ==
                    delimiter.Value)
                {
                    var firstDelimiter = stack.Pop();
                    firstDelimiter.Partner = delimiter;
                    delimiter.Partner = firstDelimiter;
                    delimiter.IsLast = firstDelimiter.IsFirst = true;
                }
                else
                {
                    stack.Push(delimiter);
                }

            var rest = stack.ToArray()
                            .ToHashSet();
            delimiters.RemoveAll(d => rest.Contains(d));

            return delimiters;
        }

        internal List<Delimiter> RemoveNonValidDelimiters(List<Delimiter> delimiters, string text) =>
            delimiters.Where(d => GetSuitableRule(d)
                                 .IsValid(d, text))
                      .ToList();

        internal List<Delimiter> RemoveEscapedDelimiters(List<Delimiter> delimiters, string text)
        {
            return delimiters.Select(d => GetSuitableRule(d)
                                         .Escape(d, text))
                             .Where(p => p != null)
                             .ToList();
        }

        private ILexerRule GetSuitableRule(Delimiter delimiter)
        {
            return rules.FirstOrDefault(r => r.Check(delimiter));
        }

        internal List<Token> GetTokensFromDelimiters(List<Delimiter> delimiters, string text)
        {
            if (!delimiters.Any())
                return new List<Token> {new StringToken(0, text.Length, text)};

            var tokens = new LinkedList<Token>();
            Token currentParent = null;
            Delimiter endOfParent = null;
            foreach (var delimiter in delimiters)
            {
                var rule = GetSuitableRule(delimiter);
                var token = rule.GetToken(delimiter, text);

                if (token != null)
                {
                    if (currentParent == null)
                    {
                        currentParent = token;
                        endOfParent = delimiter.Partner;
                        tokens.AddLast(token);

                    }

                    if (delimiter.Partner.Position < endOfParent.Position)
                    {
                        token.ParentToken = currentParent;
                        if (currentParent.InnerTokens == null)
                            currentParent.InnerTokens = new List<Token>();
                        currentParent.InnerTokens.Add(token);
                    }
                    


                }

                else if (currentParent != null)
                {
                    if (delimiter.Position >= endOfParent.Position)
                    {
                        currentParent = null;
                        endOfParent = null;
                    }
                }
            }

            var currentToken = tokens.First;

            tokens.AddLast(new UnderscoreToken(text.Length, 0, ""));
            var start = 0;
            while (currentToken != null)
            {
                var end = currentToken.Value.Position;
                var length = end - start;
                var value = text.Substring(start, length);
                if (end != start)
                {
                    tokens.AddBefore(currentToken, new StringToken(start, length, value));
                }

                start = end + currentToken.Value.Length;
                currentToken = currentToken.Next;
            }
            tokens.RemoveLast();

            return tokens.ToList();
        }

        internal List<Delimiter> GetDelimiterPositions(string text)
        {
            var delimiters = new List<Delimiter>();
            foreach (var (symbol, position) in text.Select((symbol, i) => (symbol, i)))
            {
                var rule = GetSuitableRule(symbol);
                if (rule == null)
                    continue;
                var delimiter =
                    rule.ProcessIncomingChar(position, delimiters.LastOrDefault(), out var shouldRemovePrevious);
                if (shouldRemovePrevious)
                    delimiters.RemoveAt(delimiters.Count - 1);
                delimiters.Add(delimiter);
            }

            return delimiters;
        }

        internal ILexerRule GetSuitableRule(char symbol)
        {
            return rules.FirstOrDefault(rule => rule.Check(symbol));
        }
    }
}
