using Markdown.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class MarkdownParser : IMdParser
    {
        private readonly IMdSpecification _specification;
        private List<string> EscapeSequence => _specification.EscapeSequences;
        private Dictionary<string, Tag> TagByMdRepresentation => _specification.TagByMdStringRepresentation;
        private List<int> tagLengths;

        public MarkdownParser(IMdSpecification specification)
        {
            _specification = specification;
            tagLengths = TagByMdRepresentation.Keys
                .Select(k => k.Length)
                .Distinct()
                .OrderByDescending(l => l)
                .ToList();
        }

        public List<Token> ParseToTokens(string mdText)
        {
            var tokens = new List<Token>();
            var lastCloseTagPosition = 0;
            for (int position = 0; position < mdText.Length; position++)
            {
                foreach (var len in tagLengths)
                {
                    if (mdText.TrySubstring(position, len, out string mdTag))
                        if (TagByMdRepresentation.TryGetValue(mdTag, out Tag tag))
                        {
                            if (tag.IsCorrectOpenTag(mdText, position))
                            {
                                var pairInd = FindPairTag(mdText, position + tag.OpenMdTag.Length, tag);
                                if (pairInd != -1 && CanUnionByToken(mdText, position, pairInd, tag))
                                {
                                    tokens.Add(new StringToken(mdText.Substring(lastCloseTagPosition, position - lastCloseTagPosition)));
                                    var contentStart = position + tag.OpenMdTag.Length;
                                    var contentLength = pairInd - contentStart;
                                    tokens.Add(new TagToken(mdText.Substring(contentStart, contentLength), tag));
                                    lastCloseTagPosition = pairInd + tag.CloseMdTag.Length;
                                    position = pairInd + tag.CloseMdTag.Length;
                                }
                                else
                                    position += tag.OpenMdTag.Length;
                            }
                            else
                                position += tag.OpenMdTag.Length;
                            break;
                        }
                }
            }
            if (lastCloseTagPosition != mdText.Length)
                tokens.Add(new StringToken(mdText.Substring(lastCloseTagPosition, mdText.Length - lastCloseTagPosition)));
            return tokens;
        }

        public List<Token> ParseToTokensDecompose(string mdText)
        {
            var tokens = new List<Token>();
            var lastClosePosition = 0;
            for (int position = 0; position < mdText.Length; position++)
            {
                var open = FindNextTag(mdText, position).FirstOrDefault();
                if (open == null)
                    break;

                if (open.Tag.IsCorrectOpenTag(mdText, open.Index))
                {
                    var closeTags = FindNextTag(mdText, open.Index + open.Tag.OpenMdTag.Length, open.Tag).ToList();
                    if (closeTags.Count == 0)
                        position = open.Index + open.Tag.OpenMdTag.Length;
                    foreach (var close in closeTags)
                    {
                        if (close != null && CanUnionByToken(mdText, open.Index, close.Index, close.Tag))
                        {
                            var tag = open.Tag;
                            tokens.Add(new StringToken(mdText.Substring(lastClosePosition, open.Index - lastClosePosition)));

                            var contentStart = open.Index + tag.OpenMdTag.Length;
                            var contentLength = close.Index - contentStart;

                            tokens.Add(new TagToken(mdText.Substring(contentStart, contentLength), tag));
                            lastClosePosition = position = close.Index + tag.CloseMdTag.Length;
                            break;
                        }
                    }
                }
                else
                    position = open.Index + open.Tag.OpenMdTag.Length;
            }
            if (lastClosePosition != mdText.Length)
                tokens.Add(new StringToken(mdText.Substring(lastClosePosition, mdText.Length - lastClosePosition)));
            return tokens;
        }

        private IEnumerable<IndexedTag> FindNextTag(string mdText, int position, Tag openTag = null)
        {
            var openTagSeek = openTag == null;
            var tagBuilder = new StringBuilder();
            var stack = new Stack<string>();
            for (int i = position; i < mdText.Length; i++)
            {
                tagBuilder.Append(mdText[i]);
                if (i != mdText.LastIndex()
                    && TagByMdRepresentation.Keys.Any(t => t.StartsWith($"{tagBuilder}{mdText[i + 1]}")))
                    continue;
                if (openTagSeek && tagBuilder)
            }
        }

        private IEnumerable<IndexedTag> FindNextTag(string mdText, int position)
        {
            for (int i = position; i < mdText.Length; i++)
                foreach (var len in tagLengths)
                {
                    if (mdText.TrySubstring(i, len, out string mdTag)
                        && TagByMdRepresentation.TryGetValue(mdTag, out Tag tag))
                        yield return new IndexedTag(tag, i);
                }
        }

        private int FindPairTag(string mdText, int position, Tag tag)
        {
            for (int i = position; i < mdText.Length; i++)
            {
                mdText.TrySubstring(i, tag.CloseMdTag.Length, out string pair);
                if (pair == tag.CloseMdTag && tag.IsCorrectCloseTag(mdText, i))
                    return i;
            }
            return -1;
        }

        private bool Escaped(string mdText, int position, string mdTag)
        {
            if (mdText.TryGetCharsBehind(position, 2, out char[] behindChars))
                return behindChars[1] == '\\' && behindChars[0] != '\\';
            return mdText.TryGetCharsBehind(position, 1, out char[] behindChar) && behindChar[0] == '\\';
        }

        private bool CanUnionByToken(string mdText, int openIndex, int closeIndex, Tag tag)
        {
            var behindOpenSymbol = mdText.InRange(openIndex - 1) ? mdText[openIndex - 1].ToString() : null;
            var content = mdText.Substring(openIndex + tag.OpenMdTag.Length,
               closeIndex - (openIndex + tag.OpenMdTag.Length));

            return !content.IsNullOrWhiteSpace()
                   && !content.Contains("\n")
                   //&& !TagByMdRepresentation.Keys.Any(t => content.Contains(t))
                   && !content.ContainsDigit()
                   && (behindOpenSymbol.IsNullOrWhiteSpace()
                   || !content.ContainsWhiteSpace());
        }

        private bool IsPartialTag(string mdText, string mdTag, int position)
        {
            return position != mdText.LastIndex()
                && TagByMdRepresentation.Any(t => t.Key.StartsWith($"{mdTag}{mdText[position + 1]}"));
        }
    }

    public class IndexedTag
    {
        public readonly Tag Tag;
        public readonly int Index;

        public IndexedTag(Tag tag, int index)
        {
            Tag = tag;
            Index = index;
        }
    }
}
