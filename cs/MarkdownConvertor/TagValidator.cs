using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownConvertor.ITokens;

namespace MarkdownConvertor
{
    public class TagValidator
    {
        public static IEnumerable<IToken> GetValidTokens(IEnumerable<List<IToken>> tokensInParagraphs)
        {
            return tokensInParagraphs.SelectMany(GetValidTokensFromParagraph);
        }

        private static IEnumerable<IToken> GetValidTokensFromParagraph(List<IToken> tokens)
        {
            var validTokens = new List<IToken>();
            var foundTokenTypeCounter = new Dictionary<string, List<IToken>>();
            var isTokenScreened = false;

            for (var i = 0; i < tokens.Count; i++)
                if (isTokenScreened)
                {
                    validTokens.Add(new TextToken(tokens[i].Value));
                    isTokenScreened = false;
                }
                else
                {
                    switch (tokens[i])
                    {
                        case TextToken textToken:
                            validTokens.Add(textToken);
                            break;
                        case TagToken tagToken when tagToken.TokenType is TokenType.SingleTag:
                            HandlePotentialSingleTag(i, validTokens, tagToken, tokens);
                            break;
                        case TagToken tagToken:
                            HandlePotentialDoubleTag(i, tokens, foundTokenTypeCounter, tagToken, validTokens);
                            break;
                        case ScreenerToken screenerToken:
                            HandlePotentialScreener(tokens, i, screenerToken, validTokens, out isTokenScreened);
                            break;
                    }
                }

            RemoveNonPairedDoubleTagsFromResult(foundTokenTypeCounter, validTokens);
            HandleDoubleTagsIntersections(validTokens);

            return validTokens;
        }

        private static void HandleDoubleTagsIntersections(List<IToken> validTokens)
        {
            var openedDoubleTags = new HashSet<string>();
            for (var i = 0; i < validTokens.Count; i++)
            {
                if (!(validTokens[i] is TagToken token && token.TokenType is TokenType.DoubleTag))
                    continue;

                if (token.IsOpening)
                {
                    if (openedDoubleTags.Count == 0)
                    {
                        openedDoubleTags.Add(token.Value);
                    }
                    else
                    {
                        var nextDoubleTagIndex = GetNextDoubleTagIndex(validTokens, i);
                        var previousDoubleTagIndex = GetPreviousDoubleTagIndex(validTokens, i);

                        if (!(validTokens[nextDoubleTagIndex] is TagToken nextDoubleTag
                              && nextDoubleTag.TokenType is TokenType.DoubleTag))
                            throw new Exception("Next tag is not double!");

                        if (!(validTokens[previousDoubleTagIndex] is TagToken previousTag
                              && previousTag.TokenType is TokenType.DoubleTag))
                            throw new Exception("Previous tag is not double!");

                        if (PreviousAndNextDoubleTagsHaveSameValue(validTokens, nextDoubleTagIndex,
                            previousDoubleTagIndex))
                        {
                            validTokens[nextDoubleTagIndex] = new TextToken(validTokens[nextDoubleTagIndex].Value);
                            validTokens[previousDoubleTagIndex] =
                                new TextToken(validTokens[previousDoubleTagIndex].Value);

                            var nextIndexOfDoubleTagWithSameValue =
                                GetIndexOfNextDoubleTagWithSameValue(validTokens, i, validTokens[i].Value);

                            validTokens[nextIndexOfDoubleTagWithSameValue] =
                                new TextToken(validTokens[nextIndexOfDoubleTagWithSameValue].Value);
                            validTokens[i] = new TextToken(validTokens[i].Value);
                        }
                        else if (previousTag.Rank > token.Rank)
                        {
                            var nextIndexOfDoubleTagWithSameValue =
                                GetIndexOfNextDoubleTagWithSameValue(validTokens, i, validTokens[i].Value);

                            validTokens[nextIndexOfDoubleTagWithSameValue] = new TextToken(nextDoubleTag.Value);
                            validTokens[i] = new TextToken(token.Value);
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

        private static bool PreviousAndNextDoubleTagsHaveSameValue(IReadOnlyList<IToken> validTokens,
            int nextDoubleTagIndex,
            int previousDoubleTagIndex)
        {
            return validTokens[nextDoubleTagIndex].Value.Equals(validTokens[previousDoubleTagIndex].Value);
        }

        private static void HandlePotentialScreener(IList<IToken> tokens, int i, ScreenerToken screenerToken,
            ICollection<IToken> validTokens, out bool isTokenScreened)
        {
            isTokenScreened = false;
            if (i < tokens.Count - 1 && !(tokens[i + 1] is TextToken))
                isTokenScreened = true;
            else
                validTokens.Add(new TextToken(screenerToken.Value));
        }

        private static void HandlePotentialDoubleTag(int i, IList<IToken> tokens,
            Dictionary<string, List<IToken>> foundTokenTypeCounter,
            IToken doubleTagToken, ICollection<IToken> validTokens)
        {
            if (DoubleTagCanBeValid(i, tokens, foundTokenTypeCounter))
            {
                if (foundTokenTypeCounter.ContainsKey(doubleTagToken.Value))
                    foundTokenTypeCounter[doubleTagToken.Value].Add(
                        new TagToken(doubleTagToken.Value,
                            foundTokenTypeCounter[doubleTagToken.Value].Count % 2 == 0));
                else
                    foundTokenTypeCounter[doubleTagToken.Value] = new List<IToken>
                        { new TagToken(doubleTagToken.Value, true) };
                validTokens.Add(foundTokenTypeCounter[doubleTagToken.Value].Last());
            }

            else
            {
                validTokens.Add(new TextToken(tokens[i].Value));
            }
        }

        private static bool DoubleTagCanBeValid(int i, IList<IToken> tokens,
            IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter)
        {
            return IsNotWhiteSpaceBetweenSameTags(foundTokenTypeCounter, tokens, i)
                   && IsTagNotInsideDigits(i, tokens, foundTokenTypeCounter)
                   && (IsTagOpeningAndNoWhiteSpaceAfterTag(foundTokenTypeCounter, tokens, i)
                       || IsTagClosingAndNoWhiteSpaceBeforeTag(foundTokenTypeCounter, tokens, i)
                       || IsTagNotInDifferentWordsWithPair(i, tokens, foundTokenTypeCounter));
        }

        private static bool IsTagNotInDifferentWordsWithPair(int i, IList<IToken> tokens,
            IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter)
        {
            return IsTagOpening(foundTokenTypeCounter, tokens, i) && !ContainsWhiteSpace(tokens[i + 1].Value)
                   || IsTagClosing(foundTokenTypeCounter, tokens, i) && !ContainsWhiteSpace(tokens[i - 1].Value);
        }

        private static bool IsTagNotInsideDigits(int i, IList<IToken> tokens,
            IReadOnlyDictionary<string, List<IToken>> foundTokenTypeCounter)
        {
            return IsTagOpening(foundTokenTypeCounter, tokens, i) && i < tokens.Count - 1 &&
                   !char.IsDigit(tokens[i + 1].Value.First())
                   || IsTagClosing(foundTokenTypeCounter, tokens, i) &&
                   !char.IsDigit(tokens[i - 1].Value.Last());
        }

        private static bool ContainsWhiteSpace(string value)
        {
            return value.Contains(' ');
        }

        private static void HandlePotentialSingleTag(int i, ICollection<IToken> result, IToken singleTagToken,
            IReadOnlyList<IToken> tokens)
        {
            if (i == 0 || tokens[i - 1].Value == "\n")
                result.Add(new TagToken(singleTagToken.Value));
            else
                result.Add(new TextToken(singleTagToken.Value));
        }

        private static void RemoveNonPairedDoubleTagsFromResult(Dictionary<string, List<IToken>> foundTokenTypeCounter,
            IList<IToken> validTokens)
        {
            foreach (var tag in foundTokenTypeCounter.Keys.Where(TagToken.IsValueDoubleTag))
                if (foundTokenTypeCounter[tag].Count % 2 != 0)
                {
                    var invalidToken = foundTokenTypeCounter[tag][foundTokenTypeCounter[tag].Count - 1];
                    var invalidTokenIndex = validTokens.IndexOf(invalidToken);
                    validTokens[invalidTokenIndex] = new TextToken(invalidToken.Value);
                }
        }

        private static int GetNextDoubleTagIndex(IList<IToken> tokens, int index)
        {
            for (var i = index + 1; i < tokens.Count; i++)
                if (tokens[i] is TagToken tagToken && tagToken.TokenType is TokenType.DoubleTag)
                    return i;

            throw new Exception($"Next double tag after index {index} not found!");
        }

        private static int GetPreviousDoubleTagIndex(IList<IToken> tokens, int index)
        {
            for (var i = index - 1; i >= 0; i--)
                if (tokens[i] is TagToken tagToken && tagToken.TokenType is TokenType.DoubleTag)
                    return i;

            throw new Exception($"Next double tag before index {index} not found!");
        }

        private static int GetIndexOfNextDoubleTagWithSameValue(IList<IToken> tokens, int index, string value)
        {
            for (var i = index + 1; i < tokens.Count; i++)
                if (tokens[i].Value == value)
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