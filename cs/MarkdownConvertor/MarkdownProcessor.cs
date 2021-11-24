using System;
using System.Collections.Generic;
using System.Text;
using MarkdownConvertor.ITokens;

namespace MarkdownConvertor
{
    public class MarkdownProcessor
    {
        private readonly Dictionary<string, Tuple<string, string>> doubleTags =
            new Dictionary<string, Tuple<string, string>>
            {
                { "_", Tuple.Create("<em>", "</em>") },
                { "__", Tuple.Create("<strong>", "</strong>") }
            };

        private readonly Dictionary<string, Tuple<string, string>> listTags =
            new Dictionary<string, Tuple<string, string>>
            {
                { "- ", Tuple.Create("<ul>", "</ul>") }
            };

        private readonly Dictionary<string, Tuple<string, string>> singleTags =
            new Dictionary<string, Tuple<string, string>>
            {
                { "# ", Tuple.Create("<h1>", "</h1>") },
                { "- ", Tuple.Create("<li>", "</li>") }
            };

        private readonly HashSet<string> openedLists = new HashSet<string>();
        private readonly HashSet<string> openedSingleTags = new HashSet<string>();

        public string Render(List<IToken> tokens)
        {
            var builder = new StringBuilder();
            IToken previousToken = null;

            foreach (var token in tokens)
            {
                CloseOpenedListIfExists(previousToken, token, builder);

                if (token is TextToken textToken && token.Value != "\n")
                    builder.Append(GetString(textToken));
                else
                    builder.Append(GetTag(token));

                previousToken = token;
            }

            CloseOpenedListIfExists(previousToken, previousToken, builder);

            return builder.ToString();
        }

        private void CloseOpenedListIfExists(IToken previousToken, IToken token, StringBuilder builder)
        {
            if (AnyOfListsClosed(previousToken, token)) AddClosingListTag(builder);
        }

        private bool AnyOfListsClosed(IToken previousToken, IToken token)
        {
            return previousToken != null && previousToken.Value == "\n" && !openedLists.Contains(token.Value);
        }

        private void AddClosingListTag(StringBuilder builder)
        {
            foreach (var openedListTag in openedLists) builder.Append($"{listTags[openedListTag].Item2}\n");

            openedLists.Clear();
        }

        private string GetTag(IToken token)
        {
            var sb = new StringBuilder();

            if (token is TagToken tagToken)
                switch (tagToken.TokenType)
                {
                    case TokenType.DoubleTag:
                        return tagToken.IsOpening
                            ? doubleTags[tagToken.Value].Item1
                            : doubleTags[tagToken.Value].Item2;
                    case TokenType.SingleTag:
                    {
                        if (listTags.ContainsKey(tagToken.Value) && !openedLists.Contains(tagToken.Value))
                        {
                            sb.Append($"{listTags[tagToken.Value].Item1}\n");
                            openedLists.Add(tagToken.Value);
                        }

                        openedSingleTags.Add(tagToken.Value);
                        sb.Append(singleTags[tagToken.Value].Item1);

                        return sb.ToString();
                    }
                    default:
                        throw new Exception($"Unknown TokenType {tagToken.TokenType}");
                }

            foreach (var openedSingleTag in openedSingleTags) sb.Append(singleTags[openedSingleTag].Item2);

            openedSingleTags.Clear();

            if (sb.Length <= 0) return token.Value;
            sb.Append("\n");

            return sb.ToString();
        }

        private static string GetString(IToken token)
        {
            return token.Value;
        }
    }
}