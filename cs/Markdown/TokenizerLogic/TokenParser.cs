using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TokenizerLogic
{
    public class TokenParser
    {
        private readonly string text;
        private StringBuilder builder;
        private bool isHeader;
        private bool isListItem;
        private static readonly Dictionary<char, Func<Token>> simpleTokens
            = new Dictionary<char, Func<Token>>
            {
                {'\\', () => new EscapeToken()},
                {'\n', () => new NewlineToken()},
                {'_', () => new ItalicToken()}
            };

        private TokenParser(string input)
        {
            text = input;
            builder = new StringBuilder();
        }

        public static IEnumerable<Token> ToRawTokens(string markdownText)
        {
            return new TokenParser(markdownText).Parse();
        }

        private IEnumerable<Token> Parse()
        {
            foreach (var ch in text)
            {
                if (simpleTokens.ContainsKey(ch))
                {
                    foreach (var toReturn in HandleSimpleToken(ch))
                        yield return toReturn;
                }
                else
                {
                    foreach (var toReturn in HandleComplexToken(ch))
                        yield return toReturn;
                }
            }

            foreach (var text in HandleText())
                yield return text;
        }

        private IEnumerable<Token> HandleComplexToken(char ch)
        {
            switch (ch)
            {
                case ' ':
                    foreach (var toReturn in HandleSpace())
                        yield return toReturn;
                    break;
                case '#':
                    HandleHeader();
                    break;
                case '-':
                    HandleListItem();
                    break;
                default:
                    HandleFailedTags();
                    builder.Append(ch);
                    break;
            }
        }

        private IEnumerable<Token> HandleSimpleToken(char ch)
        {
            HandleFailedTags();
            foreach (var text in HandleText())
                yield return text;
            yield return simpleTokens[ch]();
        }

        private IEnumerable<Token> HandleSpace()
        {
            foreach (var text in HandleText())
                yield return text;
            if (isHeader)
            {
                yield return new HeaderToken();
                isHeader = false;
            }
            else if (isListItem)
            {
                yield return new ListItemToken();
                isListItem = false;
            }
            else
                yield return new SpaceToken();
        }

        private IEnumerable<Token> HandleText()
        {
            if (builder.Length > 0)
            {
                yield return new TextToken(builder.ToString());
                builder.Clear();
            }
        }

        private void HandleFailedTags()
        {
            TryFailedHeader();
            TryFailedListItem();
        }

        private void HandleHeader()
        {
            if (isHeader)
                builder.Append('#');
            isHeader = true;
        }

        private void HandleListItem()
        {
            if (isListItem)
                builder.Append('-');
            isListItem = true;
        }

        private void TryFailedListItem()
        {
            if (isListItem)
            {
                isListItem = false;
                builder.Append('-');
            }
        }

        private void TryFailedHeader()
        {
            if (isHeader)
            {
                isHeader = false;
                builder.Append('#');
            }
        }
    }
}
