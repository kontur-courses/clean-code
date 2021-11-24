using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public class TextParser : IParser
    {
        private readonly IReadOnlyCollection<Tag> availableTags;
        private const char EscapeChar = '\\';

        public TextParser(IReadOnlyCollection<Tag> availableTags)
        {
            this.availableTags = availableTags;
        }
        
        public List<IToken> GetTokens(string text)
        {
            var tokens = new List<IToken>();
            var openedTags = new Stack<OpenTag>();

            for (var i = 0; i < text.Length; i++)
            {
                var nonPairToken = TryGetNonPairToken(text, i);
                if (nonPairToken != null)
                {
                    tokens.Add(nonPairToken);
                    continue;
                }

                var openTag = TryGetOpenTag(text, i, openedTags);
                if (openTag != null)
                {
                    openedTags.Push(openTag);
                    i += openTag.Tag.MdTag.Length - 1;
                    continue;
                }

                if (IsEndPairTag(text, i))
                {
                    var tag = GetTokenTag(text, i);
                    i += tag.MdTag.Length - 1;

                    if (IsCorrectEndTag(tag, openedTags))
                    {
                        var openedTag = openedTags.Pop();
                        var tokenValue = text.Substring(openedTag.OpenIndex, i - openedTag.OpenIndex + 1);
                        tokens.Add(new TagToken(tag, openedTag.OpenIndex, i, tokenValue));
                    }
                    else
                    {
                        if (openedTags.TryPop(out var openedTag))
                        {
                            if (openedTags.Any())
                                openedTags.Pop();
                            else
                                openedTags.Push(openedTag);
                        }
                    }
                }
            }

            return tokens;
        }

        private bool IsCorrectEndTag(Tag endTag, Stack<OpenTag> openedTags)
        {
            if (openedTags.Count == 0)
                return false;

            var openedTag = openedTags.Peek();
            return endTag.MdTag == openedTag.Tag.MdTag;
        }

        private OpenTag TryGetOpenTag(string text, int i, Stack<OpenTag> openedTags)
        {
            if (IsStartPairTag(text, i))
            {
                var tag = GetTokenTag(text, i);
                var openTag = new OpenTag(tag, i);
                if (openedTags.TryPeek(out var previousOpenTag))
                {
                    if (previousOpenTag.Tag.MdTag != tag.MdTag)
                    {
                        return openTag;
                    }
                }
                else
                {
                    return openTag;
                }
            }

            return null;
        }

        private TagToken TryGetNonPairToken(string text, int i)
        {
            if (IsNonPairTag(text, i))
            {
                var tag = GetTokenTag(text, i);
                var tokenEndIndex = text[i..].IndexOf("\n", StringComparison.Ordinal);

                if (tokenEndIndex == -1)
                {
                    tokenEndIndex = text.Length - 1;
                }

                var tokenValue = text.Substring(i, tokenEndIndex - i + 1);
                return new TagToken(tag, i, tokenEndIndex, tokenValue);
            }
            
            return null;
        }

        private bool IsNonPairTag(string text, int index)
        {
            return !IsEscaped(text, index) && 
                   availableTags.Where(tag => !tag.IsPairMdTag).Any(tag => tag.MdTag == text[index].ToString());
        }

        private bool IsStartPairTag(string text, int index)
        {
            var tagLength = GetPossibleTag(text, index).Length;
            
            return !IsEscaped(text, index) &&
                   index + tagLength < text.Length && !char.IsWhiteSpace(text[index + tagLength]) &&
                   availableTags.Any(tag => tag.MdTag.StartsWith(text[index].ToString()));
        }

        private bool IsEndPairTag(string text, int index)
        {
            return !IsEscaped(text, index) &&
                   index - 1 > 0 && !char.IsWhiteSpace(text[index - 1]) &&
                   availableTags.Any(tag => tag.MdTag.StartsWith(text[index].ToString()));
        }
        
        private bool IsEscaped(string text, int index)
        {
            return index - 1 >= 0 && text[index - 1] == EscapeChar && (index - 2 < 0 || text[index - 2] != EscapeChar);
        }

        private Tag GetTokenTag(string text, int index)
        {
            var mdTag = GetPossibleTag(text, index);

            return availableTags.FirstOrDefault(tag => tag.MdTag == mdTag);
        }

        private string GetPossibleTag(string text, int index)
        {
            var mdTag = text[index].ToString();

            if (index + 1 < text.Length && text[index + 1].ToString() == mdTag)
            {
                mdTag += text[index + 1];
            }

            return mdTag;
        }
    }
}