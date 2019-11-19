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
                new DoubleUnderline(),
                new SingleBlockquote()
            };
        }

        public (TagToken open, TagToken close)[] ParsePairTags(string text, IEnumerable<int> ignorableIndexes)
        {
            if (text == null || ignorableIndexes == null)
                throw new ArgumentNullException();
            var tagsPairs = new List<(TagToken open, TagToken close)>();
            var openTags = new Stack<MdPairTagToken>();
            var busyIndexes = ignorableIndexes.ToHashSet();
            foreach (var group in GetSortedTagsTokensGroups(text))
            {
                if (ShouldSkipGroup(group, busyIndexes, out var freeTagTokens))
                    continue;
                var (openTag, closeTag) = CalculateOpenAndCloseTags(openTags, freeTagTokens);
                if (openTag != default && closeTag != default)
                {
                    busyIndexes.UnionWith(closeTag.Token.GetOccupiedIndexes());
                    tagsPairs.Add(CreateOpenAndCloseTagsTokens(openTag, closeTag));
                    openTags.Pop();
                }
                else
                {
                    var openTagWithMaxLength = CalculateTagWithMaxTokenLength(freeTagTokens);
                    if (openTag == default || openTagWithMaxLength.MdPairTag.Open.Value != openTag.MdPairTag.Close.Value)
                    {
                        if (openTagWithMaxLength.MdPairTag.IsTokenOpenTag(openTagWithMaxLength.Token))
                        {
                            busyIndexes.UnionWith(openTagWithMaxLength.Token.GetOccupiedIndexes());
                            openTags.Push(openTagWithMaxLength);
                        }
                    }
                    else
                        busyIndexes.UnionWith(openTagWithMaxLength.Token.GetOccupiedIndexes());
                }
            }
            tagsPairs.AddRange(GetTagsPairsWithNotClosedTags(openTags, text));
            return tagsPairs.ToArray();
        }

        private IEnumerable<(TagToken open, TagToken close)> GetTagsPairsWithNotClosedTags(
            Stack<MdPairTagToken> notClosedTags,
            string text)
        {
            while (notClosedTags.Count != 0)
            {
                var openTag = notClosedTags.Pop();
                if (openTag.MdPairTag.CloseTagIfNotFoundClosingTag)
                {
                    var indexToInsertCloseTag = text.Length;
                    text += openTag.MdPairTag.Close.Value;
                    yield return
                    (
                        new TagToken
                        {
                            Tag = openTag.MdPairTag.Open,
                            Token = openTag.Token
                        },
                        new TagToken
                        {
                            Tag = openTag.MdPairTag.Close,
                            Token = new Token
                            {
                                StartIndex = indexToInsertCloseTag,
                                Length = openTag.MdPairTag.Close.Value.Length,
                                Str = text
                            }
                        }
                    );
                }
            }
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
            var closeTag =
                openTag == default ?
                default :
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
            freeTagTokens = group.Where(tt => !busyIndexes.Overlaps(tt.Token.GetOccupiedIndexes()));
            return freeTagTokens.FirstOrDefault() == default;
        }

        private (TagToken open, TagToken close) CreateOpenAndCloseTagsTokens(MdPairTagToken open, MdPairTagToken close) =>
            (open: new TagToken { Tag = open.MdPairTag.Open, Token = open.Token },
             close: new TagToken { Tag = close.MdPairTag.Close, Token = close.Token });

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