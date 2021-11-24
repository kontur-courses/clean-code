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
        private Dictionary<string, Tag> TagByMdRepresentation => _specification.TagByMdStringRepresentation;

        public MarkdownParser(IMdSpecification specification)
        {
            _specification = specification;
        }

        public List<Token> ParseToTokens(string mdText)
        {
            var tokens = new List<Token>();
            var lastClosePosition = 0;
            for (int position = 0; position < mdText.Length; position++)
            {
                var open = FindNextTag(mdText, position).FirstOrDefault();
                if (open == null)
                    break;

                var closeTags = FindNextTag(mdText, open.Index + open.Tag.OpenMdTag.Length, open.Tag).ToList();
                if (closeTags.Count == 0)
                    position = open.Index + open.Tag.OpenMdTag.Length;
                foreach (var close in closeTags)
                {
                    if (close != null && CanUnionByToken(mdText, open.Index, close.Index, close.Tag))
                    {
                        var tag = open.Tag;
                        tokens.Add(new StringToken(mdText.Substring(lastClosePosition, open.Index - lastClosePosition, _specification.EscapeReplaces)));

                        var contentStart = open.Index + tag.OpenMdTag.Length;
                        var contentLength = close.Index - contentStart;

                        tokens.Add(new TagToken(mdText.Substring(contentStart, contentLength, _specification.EscapeReplaces), tag));
                        lastClosePosition = position = close.Index + tag.CloseMdTag.Length;
                        break;
                    }
                }
            }
            if (lastClosePosition != mdText.Length)
                tokens.Add(new StringToken(mdText.Substring(lastClosePosition, mdText.Length - lastClosePosition, _specification.EscapeReplaces)));
            return tokens;
        }

        private IEnumerable<IndexedTag> FindNextTag(string mdText, int startPosition, Tag openTag = null)
        {
            var openTagSeek = openTag == null;
            var tagBuilder = new StringBuilder();
            var stack = new Stack<Tag>();
            for (int position = startPosition; position < mdText.Length; position++)
            {
                tagBuilder.Append(mdText[position]);
                if (IsPartialTag(mdText, tagBuilder.ToString(), position))
                    continue;
                var tagPosition = position - tagBuilder.Length + 1;

                if (TagByMdRepresentation.TryGetValue(tagBuilder.ToString(), out Tag tag)
                    && !Escaped(mdText, tagPosition))
                {
                    if (openTagSeek && tag.IsCorrectOpenTag(mdText, tagPosition))
                        yield return new IndexedTag(tag, tagPosition);
                    else if (!openTagSeek)
                    {
                        if (tag == openTag && !stack.Any() && tag.IsCorrectCloseTag(mdText, tagPosition))
                            yield return new IndexedTag(tag, tagPosition);
                        else if (tag != openTag)
                        {
                            if (stack.Any() && stack.Peek() == tag)
                                stack.Pop();
                            else
                                stack.Push(tag);
                        }
                    }
                }
                tagBuilder.Clear();
            }
        }

        private bool Escaped(string mdText, int position)
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

            return !string.IsNullOrWhiteSpace(content)
                   && !content.Contains("\n")
                   && !content.ContainsDigit()
                   && (string.IsNullOrWhiteSpace(behindOpenSymbol)
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
