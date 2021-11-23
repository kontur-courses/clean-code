using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownProcessor
{
    public class TagValidator
    {
        private readonly Tokenizer tokenizer;
        private readonly Dictionary<string, int> doubleTagsRanks;

        private readonly HashSet<char> digits = new HashSet<char>
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public TagValidator(Dictionary<string, int> doubleTagsRanks, HashSet<string> singleTags,
            HashSet<string> screeners)
        {
            this.doubleTagsRanks = doubleTagsRanks;
            tokenizer = new Tokenizer(singleTags, doubleTagsRanks, screeners);
        }

        public IEnumerable<IToken> GetValidTokens(string input)
        {
            return input.Split('\n').SelectMany(GetValidTokensFromParagraph);
        }

        private IEnumerable<IToken> GetValidTokensFromParagraph(string input)
        {
            var result = new List<IToken>();
            var foundTokenTypeCounter = new Dictionary<string, List<IToken>>();
            var isTokenScreened = false;
            var tokens = tokenizer.GetTokens(input);

            for (var i = 0; i < tokens.Count; i++)
                if (isTokenScreened)
                {
                    result.Add(new TextToken(tokens[i].Value));
                    isTokenScreened = false;
                }
                else if (tokens[i] is TextToken textToken)
                {
                    result.Add(textToken);
                }
                else if (tokens[i] is SingleTagToken singleTagToken)
                {
                    HandlePotentialSingleTag(i, result, singleTagToken);
                }
                else if (tokens[i] is DoubleTagToken doubleTagToken)
                {
                    HandlePotentialDoubleTag(i, tokens, foundTokenTypeCounter, doubleTagToken, result);
                }
                else if (tokens[i] is ScreenerToken screenerToken)
                {
                    HandlePotentialScreener(tokens, i, screenerToken, result, out isTokenScreened);
                }

            RemoveNonPairedDoubleTagsFromResult(foundTokenTypeCounter, result);
            HandleDoubleTagsIntersections(result);
            result.Add(new TextToken("\n"));

            return result;
        }

        private void HandleDoubleTagsIntersections(List<IToken> result)
        {
            var openedDoubleTags = new HashSet<string>();
            for (var i = 0; i < result.Count; i++)
            {
                if (!(result[i] is DoubleTagToken token))
                    continue;

                if (token.IsOpening)
                {
                    if (openedDoubleTags.Count == 0)
                    {
                        openedDoubleTags.Add(token.Value);
                    }
                    else
                    {
                        var nextDoubleTagIndex = GetNextDoubleTagIndex(result, i);
                        var previousDoubleTagIndex = GetPreviousDoubleTagIndex(result, i);

                        if (!(result[nextDoubleTagIndex] is DoubleTagToken nextDoubleTag))
                            throw new Exception("Next tag is not double!");

                        if (!(result[previousDoubleTagIndex] is DoubleTagToken previousTag))
                            throw new Exception("Previous tag is not double!");

                        if (PreviousAndNextDoubleTagsHaveSameValue(result, nextDoubleTagIndex, previousDoubleTagIndex))
                        {
                            result[nextDoubleTagIndex] = new TextToken(result[nextDoubleTagIndex].Value);
                            result[previousDoubleTagIndex] = new TextToken(result[previousDoubleTagIndex].Value);

                            var nextIndexOfDoubleTagWithSameValue =
                                GetIndexOfNextDoubleTagWithSameValue(result, i, result[i].Value);

                            result[nextIndexOfDoubleTagWithSameValue] =
                                new TextToken(result[nextIndexOfDoubleTagWithSameValue].Value);
                            result[i] = new TextToken(result[i].Value);
                        }
                        else if (previousTag.Rank > token.Rank)
                        {
                            var nextIndexOfDoubleTagWithSameValue =
                                GetIndexOfNextDoubleTagWithSameValue(result, i, result[i].Value);

                            result[nextIndexOfDoubleTagWithSameValue] = new TextToken(nextDoubleTag.Value);
                            result[i] = new TextToken(token.Value);
                        }
                        else
                        {
                            openedDoubleTags.Add(token.Value);
                        }
                    }
                }
                else
                {
                    openedDoubleTags.Remove(token.Value);
                }
            }
        }

        private static bool PreviousAndNextDoubleTagsHaveSameValue(IReadOnlyList<IToken> result, int nextDoubleTagIndex,
            int previousDoubleTagIndex)
        {
            return result[nextDoubleTagIndex].Value.Equals(result[previousDoubleTagIndex].Value);
        }

        private static void HandlePotentialScreener(IList<IToken> tokens, int i, ScreenerToken screenerToken,
            ICollection<IToken> result, out bool isTokenScreened)
        {
            isTokenScreened = false;
            if (i < tokens.Count - 1 && !(tokens[i + 1] is TextToken))
                isTokenScreened = true;
            else
                result.Add(new TextToken(screenerToken.Value));
        }

        private void HandlePotentialDoubleTag(int i, IList<IToken> tokens,
            Dictionary<string, List<IToken>> foundTokenTypeCounter,
            DoubleTagToken doubleTagToken, ICollection<IToken> result)
        {
            if (DoubleTagCanBeValid(i, tokens, foundTokenTypeCounter))
            {
                if (foundTokenTypeCounter.ContainsKey(doubleTagToken.Value))
                    foundTokenTypeCounter[doubleTagToken.Value].Add(
                        new DoubleTagToken(doubleTagToken.Value,
                            foundTokenTypeCounter[doubleTagToken.Value].Count % 2 == 0,
                            doubleTagToken.Rank));
                else
                    foundTokenTypeCounter[doubleTagToken.Value] = new List<IToken>
                        { new DoubleTagToken(doubleTagToken.Value, true, doubleTagToken.Rank) };
                result.Add(foundTokenTypeCounter[doubleTagToken.Value].Last());
            }

            else
            {
                result.Add(new TextToken(tokens[i].Value));
            }
        }

        private bool DoubleTagCanBeValid(int i, IList<IToken> tokens,
            IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter)
        {
            return IsTagNotScreened(i, tokens)
                   && IsNotWhiteSpaceBetweenSameTags(foundTokenTypeCounter, tokens, i)
                   && IsTagNotInsideDigits(i, tokens, foundTokenTypeCounter)
                   && (!IsTagInsideWord(tokens, i) &&
                       (IsTagOpeningAndNoWhiteSpaceAfterTag(foundTokenTypeCounter, tokens, i)
                        || IsTagClosingAndNoWhiteSpaceBeforeTag(foundTokenTypeCounter, tokens, i))
                       || IsTagInsideWord(tokens, i) &&
                       IsTagNotInDifferentWordsWithPair(i, tokens, foundTokenTypeCounter));
        }

        private static bool IsTagNotInDifferentWordsWithPair(int i, IList<IToken> tokens,
            IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter)
        {
            return IsTagOpening(foundTokenTypeCounter, tokens, i) && !DoesContainWhiteSpace(tokens[i + 1].Value)
                   || IsTagClosing(foundTokenTypeCounter, tokens, i) && !DoesContainWhiteSpace(tokens[i - 1].Value);
        }

        private bool IsTagNotInsideDigits(int i, IList<IToken> tokens,
            IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter)
        {
            return IsTagOpening(foundTokenTypeCounter, tokens, i) && i < tokens.Count - 1 &&
                   !IsDigit(tokens[i + 1].Value.First())
                   || IsTagClosing(foundTokenTypeCounter, tokens, i) &&
                   !IsDigit(tokens[i - 1].Value.Last());
        }

        private static bool IsTagNotScreened(int i, IList<IToken> tokens)
        {
            return i == 0 || i > 0 && !(tokens[i - 1] is ScreenerToken);
        }

        private bool IsDigit(char value)
        {
            return digits.Contains(value);
        }

        private static bool DoesContainWhiteSpace(string value)
        {
            return value.Contains(' ');
        }

        private static void HandlePotentialSingleTag(int i, ICollection<IToken> result, SingleTagToken singleTagToken)
        {
            if (i == 0)
                result.Add(singleTagToken);
            else
                result.Add(new TextToken(singleTagToken.Value));
        }

        private void RemoveNonPairedDoubleTagsFromResult(Dictionary<string, List<IToken>> foundTokenTypeCounter,
            IList<IToken> result)
        {
            foreach (var tag in foundTokenTypeCounter.Keys.Where(x => doubleTagsRanks.Keys.Contains(x)))
                if (foundTokenTypeCounter[tag].Count % 2 != 0)
                {
                    var invalidToken = foundTokenTypeCounter[tag][foundTokenTypeCounter[tag].Count - 1];
                    var invalidTokenIndex = result.IndexOf(invalidToken);
                    result[invalidTokenIndex] = new TextToken(invalidToken.Value);
                }
        }

        private int GetNextDoubleTagIndex(IList<IToken> tokens, int index)
        {
            for (var i = index + 1; i < tokens.Count; i++)
                if (doubleTagsRanks.ContainsKey(tokens[i].Value))
                    return i;

            throw new Exception($"Next double tag after index {index} not found!");
        }

        private int GetPreviousDoubleTagIndex(IList<IToken> tokens, int index)
        {
            for (var i = index - 1; i >= 0; i--)
                if (doubleTagsRanks.ContainsKey(tokens[i].Value))
                    return i;

            throw new Exception($"Next double tag before index {index} not found!");
        }

        private static int GetIndexOfNextDoubleTagWithSameValue(IList<IToken> tokens, int index, string value)
        {
            for (var i = index + 1; i < tokens.Count; i++)
                if (tokens[i].Value.Equals(value))
                    return i;

            throw new Exception($"Next double tag with value {value} after index {index} not found!");
        }

        private static bool IsNotWhiteSpaceBetweenSameTags(
            IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter,
            IList<IToken> tokens, int i)
        {
            return !(IsTagOpening(foundTokenTypeCounter, tokens, i) && i < tokens.Count - 1 &&
                     tokens[i + 1].Value == tokens[i].Value
                     || IsTagClosing(foundTokenTypeCounter, tokens, i) && tokens[i - 1].Value == tokens[i].Value);
        }

        private static bool IsTagInsideWord(IList<IToken> tokens, int i)
        {
            return i > 0 && i < tokens.Count - 1 && !tokens[i - 1].Value.EndsWith(" ")
                   && !tokens[i + 1].Value.StartsWith(" ");
        }

        private static bool IsTagClosingAndNoWhiteSpaceBeforeTag(
            IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter,
            IList<IToken> tokens, int i)
        {
            return IsTagClosing(foundTokenTypeCounter, tokens, i)
                   && !tokens[i - 1].Value.EndsWith(" ");
        }

        private static bool IsTagOpeningAndNoWhiteSpaceAfterTag(
            IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter,
            IList<IToken> tokens, int i)
        {
            return IsTagOpening(foundTokenTypeCounter, tokens, i)
                   && i < tokens.Count - 1
                   && !tokens[i + 1].Value.StartsWith(" ");
        }

        private static bool IsTagClosing(IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter,
            IList<IToken> tokens, int i)
        {
            return foundTokenTypeCounter.ContainsKey(tokens[i].Value)
                   && foundTokenTypeCounter[tokens[i].Value].Count % 2 != 0;
        }

        private static bool IsTagOpening(IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter,
            IList<IToken> tokens, int i)
        {
            return !foundTokenTypeCounter.ContainsKey(tokens[i].Value)
                   || foundTokenTypeCounter[tokens[i].Value].Count % 2 == 0;
        }
    }
}