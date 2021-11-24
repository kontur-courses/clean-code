using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlConvertor.ITokens;

namespace HtmlConvertor
{
    public class Tokenizer
    {
        private readonly HashSet<string> tags;

        private readonly HashSet<string> screeners = new HashSet<string>
        {
            @"\"
        };

        public Tokenizer()
        {
            tags = TagToken.GetTags();
        }

        public IEnumerable<IToken> GetTokens(string input)
        {
            return input.Split('\n').SelectMany(s => GetTokensFromParagraph($"{s}\n"));
        }

        private IEnumerable<IToken> GetTokensFromParagraph(string input)
        {
            var index = 0;
            var stringBuilder = new StringBuilder();
            var tokens = new List<IToken>();

            while (index < input.Length)
                if (TryGetLongestCollectionElementStartInIndex(input, tags, index, out var tagValue))
                {
                    index = CreatePossibleTokensAndUpdateIndex(stringBuilder, tokens, tagValue, index);
                }
                else if (TryGetLongestCollectionElementStartInIndex(input, screeners, index, out var screenerTagValue))
                {
                    index = CreatePossibleTokensAndUpdateIndex(stringBuilder, tokens, screenerTagValue, index);

                    if (index >= input.Length)
                        break;

                    if (!TryCreateToken(input[index + 1].ToString(), tokens)) stringBuilder.Append(input[index]);

                    index++;
                }
                else
                {
                    stringBuilder.Append(input[index]);
                    index++;
                }

            TryCreateTextToken(stringBuilder, tokens);

            return tokens;
        }

        private int CreatePossibleTokensAndUpdateIndex(StringBuilder stringBuilder, ICollection<IToken> tokens,
            string singleTagValue,
            int index)
        {
            TryCreateTextToken(stringBuilder, tokens);
            TryCreateToken(singleTagValue, tokens);
            index += singleTagValue.Length;
            return index;
        }

        private static void CreateTextToken(ICollection<IToken> tokens, StringBuilder stringBuilder)
        {
            tokens.Add(new TextToken(stringBuilder.ToString()));
            stringBuilder.Clear();
        }

        private bool TryCreateToken(string tagValue, ICollection<IToken> tokens)
        {
            if (tags.Contains(tagValue))
            {
                tokens.Add(new TagToken(tagValue));
                return true;
            }

            if (!screeners.Contains(tagValue)) return false;

            tokens.Add(new ScreenerToken(tagValue));

            return true;
        }

        private static void TryCreateTextToken(StringBuilder stringBuilder, ICollection<IToken> tokens)
        {
            if (stringBuilder.Length > 0)
                CreateTextToken(tokens, stringBuilder);
        }

        private static bool TryGetLongestCollectionElementStartInIndex(string line, IEnumerable<string> collection,
            int index, out string element)
        {
            var substring = line.Substring(index);

            element = collection
                .OrderByDescending(x => x.Length)
                .Where(substring.StartsWith).FirstOrDefault();

            return element != null;
        }
    }
}