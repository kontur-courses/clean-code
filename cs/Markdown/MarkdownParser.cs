using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class MarkdownParser
    {
        private static int _inputPosition;
        private static string _parsingString;

        public static MarkdownTree Parse(string rawInput)
        {
            _inputPosition = 0;
            _parsingString = rawInput;
            var resultTree = new MarkdownTree(new Tag(TagKind.Root, TagSide.None), "");
            if (rawInput != "")
                resultTree.AddChildren(ParseInput());
            return resultTree;
        }

        private static List<MarkdownTree> ParseInput()
        {
            var children = new List<MarkdownTree>();
            while (_inputPosition < _parsingString.Length)
            {
                var plainText = ParseText();
                var taggedText = ParseHeader();
                if (plainText.Content != "") children.Add(plainText);
                if (taggedText.Content != "") children.Add(taggedText);
            }

            return children;
        }

        private static MarkdownTree ParseText()
        {
            var content = new StringBuilder();
            while (IsTextParsingPossible())
            {
                var currentSymbol = _parsingString[_inputPosition];
                if (IsSymbolBeginsTag(currentSymbol))
                    break;
                content.Append(currentSymbol);
                MovePosForward();
            }
            return new MarkdownTree(new Tag(TagKind.PlainText, TagSide.None), content.ToString());
        }

        private static MarkdownTree ParseHeader()
        {
            MovePosForward();
            var content = "";
            if (_inputPosition < _parsingString.Length - 1)
            {
                var contentEnd = _parsingString.IndexOf('\n');
                var headerContentLength = contentEnd - _inputPosition + 1;
                content = _parsingString.Substring(_inputPosition, headerContentLength);
                _inputPosition += headerContentLength - 1;
            }

            return new MarkdownTree(new Tag(TagKind.Header, TagSide.Opening), content);
        }

        private static bool IsTextParsingPossible()
        {
            return _inputPosition < _parsingString.Length;
        }

        private static bool IsSymbolBeginsTag(char symbol)
        {
            return symbol == '#' || symbol == '_';
        }

        private static void MovePosForward()
        {
            _inputPosition++;
        }
    }
}
