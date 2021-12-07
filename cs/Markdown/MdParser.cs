//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Markdown.TagEvents;

//namespace Markdown
//{
//    public class MdParser
//    {
//        private static HashSet<char> _tagSymbols =
//            new HashSet<char> {'#', '_', '\n'};

//        private readonly Stack<TagEvent> _openedTags;
//        private readonly List<TagEvent> _parsedTags;

//        public MdParser()
//        {
//            _openedTags = new Stack<TagEvent>();
//            _parsedTags = new List<TagEvent>();
//            _openedTags.Push(null);
//        }

//        public IReadOnlyList<TagEvent> Parse(string input)
//        {
//            var plainText = new StringBuilder();
//            var tag = new StringBuilder();
//            for (var symbolPos = 0; symbolPos < input.Length; symbolPos++)
//            {
//                var currentSymbol = input[symbolPos];
//                if (_tagSymbols.Contains(currentSymbol))
//                {
//                    if (plainText.Length > 0)
//                    {
//                        _parsedTags.Add(new TagEvent(Side.None, Mark.Text, plainText.ToString()));
//                        plainText.Clear();
//                    }

//                    tag.Append(currentSymbol);
//                    if (symbolPos == input.Length - 1)
//                    {
//                        var tagEvent = GetTagEvent(input, symbolPos, tag.ToString());
//                        _parsedTags.Add(tagEvent);
//                        tag.Clear();
//                    }
//                }
//                else
//                {
//                    if (tag.Length > 0)
//                    {
//                        var tagEvent = GetTagEvent(input, symbolPos, tag.ToString());
//                        _parsedTags.Add(tagEvent);
//                        tag.Clear();
//                    }

//                    plainText.Append(currentSymbol);
//                    if (symbolPos == input.Length - 1)
//                    {
//                        _parsedTags.Add(new Tag_Classes.TagEvent(Side.None, Mark.Text, plainText.ToString()));
//                        plainText.Clear();
//                    }
//                }
//            }
//            _parsedTags.Add(GetEndOfInputTagEvent());
//            return _parsedTags;
//        }

//        private Tag_Classes.TagEvent GetTagEvent(string input, int symbolPos, string tag)
//        {
//            if (tag == "#")
//                return GetHashtagTagEvent(tag);
//            if (tag == "_")
//                return GetOneLineTagEvent(input, symbolPos, tag);
//            if (tag == "__")
//                return GetTwoLineTagEvent(input, symbolPos, tag);
//            return GetNewLineTagEvent(tag);
//        }

//        private Tag_Classes.TagEvent GetEndOfInputTagEvent()
//        {
//            const string fakeNewLineSymbol = "";
//            return GetNewLineTagEvent(fakeNewLineSymbol);
//        }

//        private Tag_Classes.TagEvent GetNewLineTagEvent(string tag)
//        {
//            TurnOpenedTagsToTextUpToHeader();
//            if (_openedTags.Count == 0 || _openedTags.Pop() == null)
//                return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//            _openedTags.Pop();
//            return new Tag_Classes.TagEvent(Side.Right, Mark.Header, tag);
//        }

//        private void TurnOpenedTagsToTextUpToHeader()
//        {
//            while (StackContainsTagsToTurn())
//            {
//                var openedTag = _openedTags.Pop();
//                ChangeTagToText(openedTag);
//            }
//        }

//        private bool StackContainsTagsToTurn()
//        {
//            return _openedTags.Count > 0 
//                   && _openedTags.Peek() != null
//                   && _openedTags.Peek().TagContent != "#";
//        }

//        private Tag_Classes.TagEvent GetTwoLineTagEvent(string input, int symbolPos, string tag)
//        {
//            if (SymbolIsLeftLiner(input, symbolPos, tag))
//            {
//                return GetLeftTwoLineTagEvent(tag);
//            }

//            if (SymbolIsRightLiner(input, symbolPos, tag))
//            {
//                return GetRightTwoLineTagEvent(tag);
//            }

//            return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//        }

//        private Tag_Classes.TagEvent GetRightTwoLineTagEvent(string tag)
//        {
//            var topTag = _openedTags.Peek();
//            if (topTag == null || TagIsLeftHeader(topTag))
//            {
//                return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//            }

//            if (TagIsLeftOneliner(topTag))
//            {
//                ChangeTagToText(topTag);
//                _openedTags.Pop();
//                return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//            }

//            if (TagIsLeftTwoliner(topTag))
//            {
//                topTag = _openedTags.Pop();
//                var tagBeforeLastOne = _openedTags.Peek();
//                if (tagBeforeLastOne == null || TagIsLeftOneliner(tagBeforeLastOne))
//                    return new Tag_Classes.TagEvent(Side.Right, Mark.DoubleUnderliner, tag);

//                ChangeTagToText(topTag);
//                return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//            }

//            throw new Exception("unknow mark in stack!");
//        }

//        private Tag_Classes.TagEvent GetLeftTwoLineTagEvent(string tag)
//        {
//            var lastTag = _openedTags.Peek();
//            if (lastTag == null || TagIsLeftHeaderOrOneLiner(lastTag))
//            {
//                var leftTwoLineTag = new Tag_Classes.TagEvent(Side.Left, Mark.DoubleUnderliner, tag);
//                _openedTags.Push(leftTwoLineTag);
//                return leftTwoLineTag;
//            }

//            if (lastTag.Side == Side.Left && lastTag.Mark == Mark.DoubleUnderliner)
//                return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//            throw new Exception("unknown mark in stack!");
//        }

//        private bool TagIsLeftHeaderOrOneLiner(Tag_Classes.TagEvent lastTag)
//        {
//            return (lastTag.Mark == Mark.Header || lastTag.Mark == Mark.Underliner) 
//                   && lastTag.Side == Side.Left;
//        }

//        private Tag_Classes.TagEvent GetOneLineTagEvent(string input, int symbolPos, string tag)
//        {
//            if (SymbolIsLeftLiner(input, symbolPos, tag))
//            {
//                return GetLeftOneLineTagEvent(tag);
//            }

//            if (SymbolIsRightLiner(input, symbolPos, tag))
//            {
//                return GetRightOneLineTagEvent(tag);
//            }
//            return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//        }

//        private Tag_Classes.TagEvent GetRightOneLineTagEvent(string tag)
//        {
//            var lastTag = _openedTags.Peek();
//            if (lastTag == null || TagIsHeader(lastTag))
//                return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//            if (lastTag.Side == Side.Left && lastTag.Mark == Mark.Underliner)
//            {
//                _openedTags.Pop();
//                return new Tag_Classes.TagEvent(Side.Right, Mark.Underliner, tag);
//            }

//            if (lastTag.Side == Side.Left && lastTag.Mark == Mark.DoubleUnderliner)
//            {
//                ChangeTagToText(lastTag);
//                return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//            }

//            throw new Exception($"unknown mark on the stack: {lastTag}");
//        }

//        private static bool TagIsHeader(Tag_Classes.TagEvent lastTag)
//        {
//            return lastTag.Mark == Mark.Header;
//        }

//        private static void ChangeTagToText(Tag_Classes.TagEvent lastTag)
//        {
//            lastTag.Mark = Mark.Text;
//            lastTag.Side = Side.None;
//        }

//        private Tag_Classes.TagEvent GetLeftOneLineTagEvent(string tag)
//        {
//            var lastTag = _openedTags.Peek();
//            if (lastTag == null || LeftHeaderOrTwolinerOnTop(lastTag))
//            {
//                var leftOneLineTag = new Tag_Classes.TagEvent(Side.Left, Mark.Underliner, tag);
//                _openedTags.Push(leftOneLineTag);
//                return leftOneLineTag;
//            }

//            if (lastTag.Mark == Mark.Underliner && lastTag.Side == Side.Left)
//                return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//            throw new Exception("not left mark in the tagStack!");
//        }

//        private static bool LeftHeaderOrTwolinerOnTop(Tag_Classes.TagEvent lastTag)
//        {
//            return (lastTag.Side == Side.Left && (lastTag.Mark == Mark.Header || lastTag.Mark == Mark.DoubleUnderliner));
//        }

//        private Tag_Classes.TagEvent GetHashtagTagEvent(string tag)
//        {
//            if (HashtagIsHeader())
//            {
//                var headerTagEvent = new Tag_Classes.TagEvent(Side.Left, Mark.Header, tag);
//                _openedTags.Push(headerTagEvent);
//                return headerTagEvent;
//            }

//            return new Tag_Classes.TagEvent(Side.None, Mark.Text, tag);
//        }

//        private static bool TagIsLeftTwoliner(Tag_Classes.TagEvent topTag)
//        {
//            return topTag.Side == Side.Left & topTag.Mark == Mark.DoubleUnderliner;
//        }

//        private static bool TagIsLeftOneliner(Tag_Classes.TagEvent tag)
//        {
//            return tag.Side == Side.Left && tag.Mark == Mark.Underliner;
//        }

//        private static bool TagIsLeftHeader(Tag_Classes.TagEvent lastTag)
//        {
//            return lastTag.Side == Side.Left && lastTag.Mark == Mark.Header;
//        }

//        private bool SymbolIsLeftLiner(string input, int symbolPos, string tag)
//        {
//            return !(symbolPos == input.Length - 1 || input[symbolPos + 1] == ' ');
//        }

//        private bool SymbolIsRightLiner(string input, int symbolPos, string tag)
//        {
//            return !(symbolPos == tag.Length - 1 || input[symbolPos - tag.Length] == ' ');
//        }

//        private bool HashtagIsHeader()
//        {
//            return _parsedTags.Count == 0 || _parsedTags.Last().TagContent.EndsWith("\n");
//        }
//    }
//}
