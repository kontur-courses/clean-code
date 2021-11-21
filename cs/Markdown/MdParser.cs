using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
            //var rootTagEvent = new TagEvent(Side.None, Tag.Root, "");
            _openedTags = new Stack<TagEvent>();
            _openedTags.Push(null);
            _parsedTags = new List<TagEvent>();
        }

        public IReadOnlyList<TagEvent> Parse(string input)
        {
            var plainText = new StringBuilder();
            var tag = new StringBuilder();
            for (var symbolPos = 0; symbolPos < input.Length; symbolPos++)
            {
                var currentSymbol = input[symbolPos];
                if (_tagSymbols.Contains(currentSymbol))
                {
                    if (plainText.Length > 0)
                    {
                        _parsedTags.Add(new TagEvent(Side.None, Tag.Text, plainText.ToString()));
                        plainText.Clear();
                    }

                    tag.Append(currentSymbol);
                    if (symbolPos == input.Length - 1)
                    {
                        var tagEvent = GetTagEventCheckingRules(input, symbolPos, tag.ToString());
                        _parsedTags.Add(tagEvent);
                        tag.Clear();
                    }
                }
                else
                {
                    if (tag.Length > 0)
                    {
                        var tagEvent = GetTagEventCheckingRules(input, symbolPos, tag.ToString());
                        _parsedTags.Add(tagEvent);
                        tag.Clear();
                    }

                    plainText.Append(currentSymbol);
                    if (symbolPos == input.Length - 1)
                    {
                        _parsedTags.Add(new TagEvent(Side.None, Tag.Text, plainText.ToString()));
                        plainText.Clear();
                    }
                }
            }
            return _parsedTags;
        }

        private static bool NextSymbolIsTagged(string input, int nextPos)
        {
            return nextPos < input.Length && _tagSymbols.Contains(input[nextPos]);
        }

        private TagEvent GetTagEventCheckingRules(string input, int symbolPos, string tag)
        {
            if (tag == "#")
                return GetHashtagTagEvent(tag);
            if (tag == "_")
                return GetOneLineTagEvent(input, symbolPos, tag);
            if (tag == "__")
                return GetTwoLineTagEvent(input, symbolPos, tag);
            return GetEndOfLineTagEvent(tag);
        }

        private TagEvent GetEndOfLineTagEvent(string tag)
        {
            TurnAllOpenedTagsToText();
            var firstOpenedTag = _openedTags.Pop();
            if (firstOpenedTag == null)
                return new TagEvent(Side.None, Tag.Text, tag);
            _openedTags.Pop();
            return new TagEvent(Side.Right, Tag.Header, tag);
        }

        private void TurnAllOpenedTagsToText()
        {
            while (_openedTags.Peek() != null && _openedTags.Peek().TagContent != "#")
            {
                var openedTag = _openedTags.Pop();
                ChangeTagToText(openedTag);
            }
        }

        private TagEvent GetTwoLineTagEvent(string input, int symbolPos, string tag)
        {
            if (TagIsLeftLiner(input, symbolPos, tag))
            {
                return GetLeftTwoLineTagEvent(tag);
            }

            if (TagIsRightLiner(input, symbolPos, tag))
            {
                return GetRightTwoLineTagEvent(tag);
            }

            return new TagEvent(Side.None, Tag.Text, tag);
        }

        private TagEvent GetRightTwoLineTagEvent(string tag)
        {
            var lastTag = _openedTags.Peek();
            if (lastTag == null || (lastTag.Side == Side.Left && lastTag.Tag == Tag.Header))
            {
                return new TagEvent(Side.None, Tag.Text, tag);
            }

            if (lastTag.Side == Side.Left && lastTag.Tag == Tag.OneLine)
            {
                ChangeTagToText(lastTag);
                _openedTags.Pop();
                return new TagEvent(Side.None, Tag.Text, tag);
            }

            if (lastTag.Side == Side.Left & lastTag.Tag == Tag.TwoLines)
            {
                lastTag = _openedTags.Pop();
                var tagBeforeLastOne = _openedTags.Peek();
                if (tagBeforeLastOne == null || !TagIsNotLeftOneliner(tagBeforeLastOne))
                    return new TagEvent(Side.Right, Tag.TwoLines, tag);

                ChangeTagToText(lastTag);
                return new TagEvent(Side.None, Tag.Text, tag);
            }

            throw new Exception("unknow tag in stack!");
        }

        private static bool TagIsNotLeftOneliner(TagEvent beforeLastTag)
        {
            return beforeLastTag.Tag == Tag.OneLine && beforeLastTag.Side == Side.Left;
        }

        private TagEvent GetLeftTwoLineTagEvent(string tag)
        {
            var lastTag = _openedTags.Peek();
            if (lastTag == null || LeftHeaderOrOneLinerOnStack(lastTag))
            {
                var leftTwoLineTag = new TagEvent(Side.Left, Tag.TwoLines, tag);
                _openedTags.Push(leftTwoLineTag);
                return leftTwoLineTag;
            }

            if (lastTag.Side == Side.Left && lastTag.Tag == Tag.TwoLines)
                return new TagEvent(Side.None, Tag.Text, tag);
            throw new Exception("unknown tag in stack!");
        }

        private bool LeftHeaderOrOneLinerOnStack(TagEvent lastTag)
        {
            return lastTag.Side == Side.Left 
                   && (lastTag.Tag == Tag.Header || lastTag.Tag == Tag.OneLine);
        }

        private TagEvent GetOneLineTagEvent(string input, int symbolPos, string tag)
        {
            if (TagIsLeftLiner(input, symbolPos, tag))
            {
                return GetLeftOneLineTagEvent(tag);
            }

            if (TagIsRightLiner(input, symbolPos, tag))
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
            if (lastTag == null || LeftHeaderOrTwolinerOnTop(lastTag))
            {
                var leftOneLineTag = new TagEvent(Side.Left, Tag.OneLine, tag);
                _openedTags.Push(leftOneLineTag);
                return leftOneLineTag;
            }

            if (lastTag.Tag == Tag.OneLine && lastTag.Side == Side.Left)
                return new TagEvent(Side.None, Tag.Text, tag);
            throw new Exception("not left tag in the tagStack!");
        }

        private static bool LeftHeaderOrTwolinerOnTop(TagEvent lastTag)
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

        private bool TagIsLeftLiner(string input, int symbolPos, string tag)
        {
            return !(symbolPos == input.Length - 1 || input[symbolPos + 1] == ' ');
        }

        private bool TagIsRightLiner(string input, int symbolPos, string tag)
        {
            return !(symbolPos == tag.Length - 1 || input[symbolPos - tag.Length] == ' ');
        }

        private bool HashtagIsHeader()
        {
            return _parsedTags.Count == 0 || _parsedTags.Last().TagContent.EndsWith("\n");
        }
    }
}
