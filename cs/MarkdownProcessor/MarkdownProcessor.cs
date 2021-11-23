using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownProcessor
{
    public class MarkdownProcessor
    {
        private readonly Dictionary<string, Tuple<string, string>> doubleTags =
            new Dictionary<string, Tuple<string, string>>
            {
                { "_", Tuple.Create("<em>", "</em>") },
                { "__", Tuple.Create("<strong>", "</strong>") }
            };

        private readonly Dictionary<string, int> doubleTagsRanks = new Dictionary<string, int>
        {
            { "_", 2 },
            { "__", 1 }
        };

        private readonly HashSet<string> screeners = new HashSet<string>
        {
            @"\"
        };

        private readonly Dictionary<string, Tuple<string, string>> listTags =
            new Dictionary<string, Tuple<string, string>>
            {
                { "- ", Tuple.Create("<ul>", "</ul>") }
            };

        private readonly HashSet<string> openedLists = new HashSet<string>();

        private readonly Dictionary<string, Tuple<string, string>> singleTags =
            new Dictionary<string, Tuple<string, string>>
            {
                { "# ", Tuple.Create("<h1>", "</h1>") },
                { "- ", Tuple.Create("<li>", "</li>") }
            };

        private readonly HashSet<string> openedSingleTags = new HashSet<string>();
        private readonly TagValidator validator;

        public MarkdownProcessor()
        {
            validator = new TagValidator(doubleTagsRanks, singleTags.Keys.ToHashSet(), screeners);
        }

        public string Render(string input)
        {
            var builder = new StringBuilder();

            foreach (var token in validator.GetValidTokens(input))
                if (token is TextToken textToken && !token.Value.Equals("\n"))
                {
                    if (OpenedListIsClosed())
                        AddClosingListTag(builder);

                    builder.Append(GetString(textToken));
                }
                else
                {
                    builder.Append(GetTag(token));
                }

            if (OpenedListIsClosed())
                AddClosingListTag(builder);

            return builder.ToString();
        }

        private void AddClosingListTag(StringBuilder builder)
        {
            foreach (var openedListTag in openedLists) builder.Append($"{listTags[openedListTag].Item2}\n");

            openedLists.Clear();
        }

        private bool OpenedListIsClosed()
        {
            return openedLists.Count > 0 && openedSingleTags.Count == 0;
        }

        private string GetTag(IToken token)
        {
            var sb = new StringBuilder();

            if (token is DoubleTagToken doubleTagToken)
                return doubleTagToken.IsOpening
                    ? doubleTags[doubleTagToken.Value].Item1
                    : doubleTags[doubleTagToken.Value].Item2;

            if (token is SingleTagToken singleTagToken)
            {
                if (listTags.ContainsKey(singleTagToken.Value) && !openedLists.Contains(singleTagToken.Value))
                {
                    sb.Append($"{listTags[singleTagToken.Value].Item1}\n");
                    openedLists.Add(singleTagToken.Value);
                }

                openedSingleTags.Add(singleTagToken.Value);
                sb.Append(singleTags[singleTagToken.Value].Item1);

                return sb.ToString();
            }

            foreach (var openedSingleTag in openedSingleTags) sb.Append(singleTags[openedSingleTag].Item2);

            openedSingleTags.Clear();

            if (sb.Length > 0)
            {
                sb.Append("\n");
                return sb.ToString();
            }

            return token.Value;
        }

        private static string GetString(IToken token)
        {
            return token.Value;
        }
    }
}