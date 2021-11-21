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
            _openedTags = new Stack<TagEvent>();
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
                        _parsedTags.Add(new TagEvent(TagSide.None,TagKind.PlainText, plainText.ToString()));
                        plainText.Clear();
                    }
                    tag.Append(input[symbolPos]);
                    continue;
                }

                if (tag.Length > 0)
                {
                    _parsedTags.Add(GetTagEventCheckingRules(input, symbolPos, tag.ToString()));
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
                return GetSingleUnderlineTagEvent(input, symbolPos, tag);
        }

        private TagEvent GetSingleUnderlineTagEvent(string input, int symbolPos, string tag)
        {
            if (TagIsNotOpeningUnderline(input, symbolPos))
                return new TagEvent(TagSide.None, TagKind.PlainText, tag);
        }

        private TagEvent GetHashtagTagEvent(string tag)
        {
            if (TagIsHeader())
                return new TagEvent(TagSide.Opening, TagKind.Header, tag);
            return new TagEvent(TagSide.None, TagKind.PlainText, tag);
        }

        private bool TagIsNotOpeningUnderline(string input, int symbolPos)
        {
            return symbolPos >= input.Length - 1 || input[symbolPos + 1] == ' ';
        }

        private bool TagIsHeader()
        {
            return _parsedTags.Count == 0 || _parsedTags.Last().TagContent.EndsWith("\n");
        }
    }
}
