using Markdown.MdTags;
using Markdown.MdTags.PairTags;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.MdTagsParsers
{
    class PairTagsParser
    {
        private class MdPairTagToken
        {
            public MdPairTagBase MdPairTag { get; set; }
            public Token Token { get; set; }
        }

        private readonly MdPairTagBase[] supportedTagsPairs;

        public PairTagsParser()
        {
            supportedTagsPairs = new MdPairTagBase[]
            {
                new SingleUnderline(),
                new DoubleUnderline()
            };
        }

        public (TagToken open, TagToken close)[] ParsePairTags(string text, IEnumerable<int> ignorableIndexes)
        {
            if (text == null || ignorableIndexes == null)
                throw new ArgumentNullException();
            var tagsPairs = new List<(TagToken open, TagToken close)>();
            var openTags = new Stack<MdPairTagToken>();
            var busyIndexes = new HashSet<int>();
            AddEnumerableToHashSet(ignorableIndexes, busyIndexes);
            foreach (var group in GetSortedTagsTokensGroups(text))
            {
                if (ShouldSkipGroup(group, busyIndexes, out var freeTagTokens))
                    continue;
                var (openTag, closeTag) = CalculateOpenAndCloseTags(openTags, freeTagTokens);
                if (openTag != default && closeTag != default)
                {
                    AddTokenToHashSet(closeTag.Token, busyIndexes);
                    AddTagsPairToResult(openTag, closeTag, tagsPairs);
                    openTags.Pop();
                }
                else
                {
                    var openTagWithMaxLength = CalculateTagWithMaxTokenLength(freeTagTokens);
                    if (openTag == default || openTagWithMaxLength.MdPairTag.Open != openTag.MdPairTag.Open)
                    {
                        if (openTagWithMaxLength.MdPairTag.IsTokenOpenTag(openTagWithMaxLength.Token))
                        {
                            AddTokenToHashSet(openTagWithMaxLength.Token, busyIndexes);
                            openTags.Push(openTagWithMaxLength);
                        }
                    }
                    else
                        AddTokenToHashSet(openTagWithMaxLength.Token, busyIndexes);
                }
            }
            return tagsPairs.ToArray();
        }

        private MdPairTagToken CalculateTagWithMaxTokenLength(IEnumerable<MdPairTagToken> freeTagTokens)
        {
            var maxLength = freeTagTokens.Max(tt => tt.Token.Length);
            return freeTagTokens.First(tt => tt.Token.Length == maxLength);
        }

        private (MdPairTagToken openTag, MdPairTagToken closeTag) CalculateOpenAndCloseTags(
            Stack<MdPairTagToken> openTags,
            IEnumerable<MdPairTagToken> freeTagTokens)
        {
            var openTag = openTags.FirstOrDefault();
            var closeTag = openTag == 
                default ?
                default
                :
                freeTagTokens.FirstOrDefault(tt => openTag.MdPairTag.IsTokenCloseTag(tt.Token));
            return (openTag, closeTag);
        }

        private bool ShouldSkipGroup(
            IGrouping<int, MdPairTagToken> group,
            HashSet<int> busyIndexes,
            out IEnumerable<MdPairTagToken> freeTagTokens)
        {
            freeTagTokens = null;
            if (busyIndexes.Contains(group.Key))
                return true;
            freeTagTokens = group.Where(tt => !HashSetContainsAnyTokenIndex(tt.Token, busyIndexes));
            if (freeTagTokens.FirstOrDefault() == default)
                return true;
            return false;
        }

        private void AddTagsPairToResult(
            MdPairTagToken open,
            MdPairTagToken close,
            List<(TagToken open, TagToken close)> tagsPairs)
        {
            tagsPairs.Add((
                open: new TagToken { Tag = open.MdPairTag.Open, Token = open.Token },
                close: new TagToken { Tag = close.MdPairTag.Close, Token = close.Token }
                ));
        }

        private bool HashSetContainsAnyTokenIndex(Token token, HashSet<int> hashSet)
        {
            for (var i = token.StartIndex; i < token.StartIndex + token.Length; i++)
                if (hashSet.Contains(i))
                    return true;
            return false;
        }

        private void AddEnumerableToHashSet(IEnumerable<int> enumerable, HashSet<int> hashSet)
        {
            foreach (var i in enumerable)
                hashSet.Add(i);
        }

        private void AddTokenToHashSet(Token token, HashSet<int> hashSet)
        {
            for (var i = token.StartIndex; i < token.StartIndex + token.Length; i++)
                hashSet.Add(i);
        }

        private IEnumerable<IGrouping<int, MdPairTagToken>> GetSortedTagsTokensGroups(string text)
        {
            var result = new List<MdPairTagToken>();
            foreach (var tag in supportedTagsPairs)
                result.AddRange(
                    text
                    .GetTokensWithSubStrsEntries(new[] { tag.Open.Value, tag.Close.Value })
                    .Select(t => new MdPairTagToken { MdPairTag = tag, Token = t }));
            return
                result
                .GroupBy(tt => tt.Token.StartIndex)
                .OrderBy(g => g.Key);
        }
    }
}