using Markdown.MdTags;
using Markdown.MdTags.PairTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MdTagsParsers
{
    class PairTagsParser
    {
        private class MdPairTagToken
        {
            public MdPairTagBase MdPairTag { get; set; }
            public Token Token { get; set; }
        }

        private readonly MdPairTagBase[] supportedTags;

        public PairTagsParser()
        {
            supportedTags = new MdPairTagBase[]
            {
                new SingleUnderline(),
                new DoubleUnderline()
            };
        }

        public (TagToken open, TagToken close)[] ParsePairTags(string text)
        {
            if (text == null)
                throw new ArgumentNullException();
            var tagsPairs = new List<(TagToken open, TagToken close)>();
            var openTags = new Stack<MdPairTagToken>();
            var busyIndexes = new HashSet<int>();
            foreach (var group in GetSortedTagsTokensGroups(text))
            {
                if (busyIndexes.Contains(group.Key))
                    continue;
                if (openTags.Count == 0)
                {
                    var tagWithMaxLength = group.First(tt => tt.Token.Count == group.Max(z => z.Token.Count));
                    AddTokenToHashSet(tagWithMaxLength.Token, busyIndexes);
                    if (tagWithMaxLength.MdPairTag.IsTokenOpenTag(tagWithMaxLength.Token))
                        openTags.Push(tagWithMaxLength);
                }
                else
                {
                    var openTag = openTags.Peek();
                    var closeTag = group.FirstOrDefault(tt => openTag.MdPairTag.IsTokenCloseTag(tt.Token));
                    if (closeTag != default)
                    {
                        tagsPairs.Add(
                                (
                                     open: new TagToken() { Tag = openTag.MdPairTag.Open, Token = openTag.Token },
                                     close: new TagToken() { Tag = closeTag.MdPairTag.Close, Token = closeTag.Token }
                                ));
                        openTags.Pop();
                        AddTokenToHashSet(closeTag.Token, busyIndexes);
                    }
                    else
                    {
                        var tagWithMaxLength = group.First(tt => tt.Token.Count == group.Max(z => z.Token.Count));
                        AddTokenToHashSet(tagWithMaxLength.Token, busyIndexes);
                        if (!openTag.MdPairTag.IsTokenOpenTag(tagWithMaxLength.Token) &&
                            tagWithMaxLength.MdPairTag.IsTokenOpenTag(tagWithMaxLength.Token))
                            openTags.Push(tagWithMaxLength);
                    }
                }
            }
            return tagsPairs.ToArray();
        }

        private void AddTokenToHashSet(Token token, HashSet<int> hashSet)
        {
            for (var i = token.StartIndex; i < token.StartIndex + token.Count; i++)
                hashSet.Add(i);
        }

        private IEnumerable<IGrouping<int, MdPairTagToken>> GetSortedTagsTokensGroups(string text)
        {
            var result = new List<MdPairTagToken>();
            foreach (var tag in supportedTags)
                result.AddRange(
                    text
                    .GetAllSubStrEntries(new[] { tag.Open.Value, tag.Close.Value })
                    .Select(t => new MdPairTagToken() { MdPairTag = tag, Token = t }));
            return
                result
                .GroupBy(tt => tt.Token.StartIndex)
                .OrderBy(g => g.Key);
        }
    }
}