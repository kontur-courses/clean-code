using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tag_Classes;

namespace Markdown
{
    public class MdParser
    {
        private static HashSet<char> _tagSymbols =
            new HashSet<char> {'#', '_', '\n'};

        private readonly Stack<TagEvent> _openedTags;
        private readonly List<TagEvent> _parsedTags;

        public MdParser()
        {
            _openedTags = new Stack<TagEvent>(null);
            _parsedTags = new List<TagEvent>();
        }

        public IReadOnlyList<TagEvent> Parse(string input)
        {
            var plainText = new StringBuilder();
            var tag = new StringBuilder();
            for (var symbolPos = 0; symbolPos < input.Length; symbolPos++)
            {
                if (_tagSymbols.Contains(input[symbolPos]))
                {
                    if (plainText.Length > 0)
                    {
                        _parsedTags.Add(new TagEvent(Side.None,Tag.Text, plainText.ToString()));
                        plainText.Clear();
                    }
                    tag.Append(input[symbolPos]);
                    continue;
                }

                if (tag.Length > 0)
                {
                    var tagEvent = GetTagEventCheckingRules(input, symbolPos, tag.ToString());
                    _parsedTags.Add(tagEvent);
                    tag.Clear();
                }
                plainText.Append(input[symbolPos]);
            }
            return _parsedTags;
        }

        private TagEvent GetTagEventCheckingRules(string input, int symbolPos, string tag)
        {
            if (tag == "#")
                return GetHashtagTagEvent(tag);
            if (tag == "_")
                return GetOneLineTagEvent(input, symbolPos, tag);
            if (tag == "--")
                return GetTwoLineTagEvent(input, symbolPos, tag);
            if (tag == "\n")
            {
                TurnAllOpenedTagsToText();
                var firstOpenedTag = _openedTags.Pop();
                if (firstOpenedTag == null)
                    return new TagEvent(Side.None, Tag.Text, tag);
                _openedTags.Pop();
                return new TagEvent(Side.Right, Tag.Header, tag);
            }
        }

        private void TurnAllOpenedTagsToText()
        {
            while (_openedTags.Peek() != null || _openedTags.Peek().TagContent != "#")
            {
                var openedTag = _openedTags.Pop();
                ChangeTagToText(openedTag);
            }
        }

        private TagEvent GetOneLineTagEvent(string input, int symbolPos, string tag)
        {
            if (TagIsLeftOneline(input, symbolPos))
            {
                return GetLeftOneLineTagEvent(tag);
            }

            if (TagIsRightOneline(input, symbolPos))
            {
                return GetRightOneLineTagEvent(tag);
            }
            return new TagEvent(Side.None, Tag.Text, tag);
        }

        private TagEvent GetRightOneLineTagEvent(string tag)
        {
            var lastTag = _openedTags.Peek();
            if (lastTag == null || lastTag.Tag == Tag.Header)
                return new TagEvent(Side.None, Tag.Text, tag);
            if (lastTag.Side == Side.Left && lastTag.Tag == Tag.OneLine)
            {
                _openedTags.Pop();
                return new TagEvent(Side.Right, Tag.OneLine, tag);
            }

            if (lastTag.Side == Side.Left && lastTag.Tag == Tag.TwoLines)
            {
                ChangeTagToText(lastTag);
                return new TagEvent(Side.None, Tag.Text, tag);
            }

            throw new Exception($"unknown tag on the stack: {lastTag}");
        }

        private static void ChangeTagToText(TagEvent lastTag)
        {
            lastTag.Tag = Tag.Text;
            lastTag.Side = Side.None;
        }

        private TagEvent GetLeftOneLineTagEvent(string tag)
        {
            var lastTag = _openedTags.Peek();
            if (lastTag == null || OnlyOtherLeftTagsInStack(lastTag))
            {
                var leftOneLineTag = new TagEvent(Side.Left, Tag.OneLine, tag);
                _openedTags.Push(leftOneLineTag);
                return leftOneLineTag;
            }

            if (lastTag.Tag == Tag.OneLine && lastTag.Side == Side.Left)
                return new TagEvent(Side.None, Tag.Text, tag);
            throw new Exception("not left tag in the tagStack!");
        }

        private static bool OnlyOtherLeftTagsInStack(TagEvent lastTag)
        {
            return (lastTag.Side == Side.Left && (lastTag.Tag == Tag.Header || lastTag.Tag == Tag.TwoLines));
        }

        private TagEvent GetHashtagTagEvent(string tag)
        {
            if (HashtagIsHeader())
            {
                var headerTagEvent = new TagEvent(Side.Left, Tag.Header, tag);
                _openedTags.Push(headerTagEvent);
                return headerTagEvent;
            }

            return new TagEvent(Side.None, Tag.Text, tag);
        }

        private bool TagIsLeftOneline(string input, int symbolPos)
        {
            return !(symbolPos >= input.Length - 1 || input[symbolPos + 1] == ' ');
        }

        private bool TagIsRightOneline(string input, int symbolPos)
        {
            return !(symbolPos == 0 || input[symbolPos - 1] == ' ');
        }

        private bool HashtagIsHeader()
        {
            return _parsedTags.Count == 0 || _parsedTags.Last().TagContent.EndsWith("\n");
        }
    }
}
