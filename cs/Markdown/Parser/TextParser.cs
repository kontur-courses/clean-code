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
                if (CheckIfNonPairTag(text, i, tokens)) continue;

                if (CheckIfStartPairTag(text, openedTags, ref i)) continue;

                i = CheckIfEndPairTag(text, i, openedTags, tokens);

            }

            return tokens;
        }

        private int CheckIfEndPairTag(string text, int i, Stack<OpenTag> openedTags, List<IToken> tokens)
        {
            if (IsEndPairTag(text, i))
            {
                var tagType = GetTokenTagType(text, i);

                i += tagType.MdTag.Length - 1;

                if (openedTags.Count == 0)
                    return i;

                var openedTag = openedTags.Pop();
                if (tagType.MdTag != openedTag.TagType.MdTag)
                {
                    if (openedTags.Any())
                        openedTags.Pop();
                    else
                        openedTags.Push(openedTag);

                    return i;
                }

                var tokenValue = text.Substring(openedTag.OpenIndex, i - openedTag.OpenIndex + 1);
                tokens.Add(new TagToken(tagType, openedTag.OpenIndex, i, tokenValue));
            }

            return i;
        }

        private bool CheckIfStartPairTag(string text, Stack<OpenTag> openedTags, ref int i)
        {
            if (IsStartPairTag(text, i))
            {
                var tagType = GetTokenTagType(text, i);
                var openTag = new OpenTag(tagType, i);
                if (openedTags.TryPeek(out var openedTag))
                {
                    if (openedTag.TagType.MdTag != tagType.MdTag)
                    {
                        i += tagType.MdTag.Length - 1;

                        openedTags.Push(openTag);
                        return true;
                    }
                }
                else
                {
                    i += tagType.MdTag.Length - 1;

                    openedTags.Push(openTag);
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfNonPairTag(string text, int i, List<IToken> tokens)
        {
            if (IsNonPairTag(text, i))
            {
                var tagType = GetTokenTagType(text, i);
                var tokenEndIndex = text[i..].IndexOf("\n", StringComparison.Ordinal);

                if (tokenEndIndex == -1)
                {
                    tokenEndIndex = text.Length - 1;
                }

                var tokenValue = text.Substring(i, tokenEndIndex - i + 1);
                tokens.Add(new TagToken(tagType, i, tokenEndIndex, tokenValue));
                return true;
            }

            return false;
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
            if (index - 1 >= 0 && text[index - 1] == EscapeChar)
            {
                if (index - 2 >= 0 && text[index - 2] == EscapeChar)
                {
                    return false;
                }
                
                return true;
            }

            return false;
        }

        private Tag GetTokenTagType(string text, int index)
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

        private class OpenTag
        {
            public Tag TagType { get; }
            public int OpenIndex { get; }

            public OpenTag(Tag tagType, int openIndex)
            {
                TagType = tagType;
                OpenIndex = openIndex;
            }
        }
    }
}