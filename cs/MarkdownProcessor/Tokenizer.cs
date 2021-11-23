using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkdownProcessor
{
    public class Tokenizer
    {
        private readonly HashSet<string> singleTags;
        private readonly HashSet<string> doubleTags;
        private readonly HashSet<string> screeners;
        private readonly Dictionary<string, int> doubleTagsRanks;

        public Tokenizer(HashSet<string> singleTags, Dictionary<string, int> doubleTagsRanks, HashSet<string> screeners)
        {
            this.screeners = screeners;
            this.singleTags = singleTags;
            doubleTags = doubleTagsRanks.Keys.ToHashSet();
            this.doubleTagsRanks = doubleTagsRanks;
        }

        public IList<IToken> GetTokens(string input)
        {
            var index = 0;
            var stringBuilder = new StringBuilder();
            var result = new List<IToken>();

            while (index < input.Length)
                if (IsLineContainsCollectionElementInIndex(input, singleTags, index))
                {
                    index = CreatePossibleTokensAndUpdateIndex(input, singleTags, stringBuilder, result, index);
                }
                else if (IsLineContainsCollectionElementInIndex(input, doubleTags, index))
                {
                    index = CreatePossibleTokensAndUpdateIndex(input, doubleTags, stringBuilder, result, index);
                }
                else if (IsLineContainsCollectionElementInIndex(input, screeners, index))
                {
                    index = CreatePossibleTokensAndUpdateIndex(input, screeners, stringBuilder, result, index);

                    if (index >= input.Length)
                        break;

                    CreateTokenFromNextChar(input, index, result, stringBuilder);

                    index++;
                }
                else
                {
                    stringBuilder.Append(input[index]);
                    index++;
                }

            if (stringBuilder.Length > 0) CreateTextToken(result, stringBuilder);

            return result;
        }

        private static void CreateTextToken(List<IToken> result, StringBuilder stringBuilder)
        {
            result.Add(new TextToken(stringBuilder.ToString()));
            stringBuilder.Clear();
        }

        private void CreateTokenFromNextChar(string input, int index, List<IToken> result, StringBuilder stringBuilder)
        {
            var nextChar = input[index].ToString();

            if (singleTags.Contains(nextChar))
                result.Add(new SingleTagToken(nextChar));
            else if (doubleTags.Contains(nextChar))
                result.Add(new DoubleTagToken(nextChar, false, doubleTagsRanks[nextChar]));
            else if (screeners.Contains(nextChar))
                result.Add(new ScreenerToken(nextChar));
            else
                stringBuilder.Append(nextChar);
        }

        private int CreatePossibleTokensAndUpdateIndex(string input, HashSet<string> collection,
            StringBuilder stringBuilder, List<IToken> result, int index)
        {
            TryCreateTextToken(stringBuilder, result);

            var tagValue = GetPossibleLongestValueInCollectionFromLineInIndex(input, collection, index);
            if (collection.Equals(singleTags))
                result.Add(new SingleTagToken(tagValue));
            else if (collection.Equals(doubleTags))
                result.Add(new DoubleTagToken(tagValue, false, doubleTagsRanks[tagValue]));
            else
                result.Add(new ScreenerToken(tagValue));

            index += tagValue.Length;
            return index;
        }

        private static void TryCreateTextToken(StringBuilder stringBuilder, List<IToken> result)
        {
            if (stringBuilder.Length > 0)
                CreateTextToken(result, stringBuilder);
        }

        private static string GetPossibleLongestValueInCollectionFromLineInIndex(string input,
            IEnumerable<string> collection, int index)
        {
            return collection
                .OrderByDescending(x => x.Length).First(x => input.Substring(index).StartsWith(x));
        }

        private static bool IsLineContainsCollectionElementInIndex(string line, IEnumerable<string> collection,
            int index)
        {
            return collection
                .OrderByDescending(x => x.Length)
                .Any(line.Substring(index).StartsWith);
        }
    }
}