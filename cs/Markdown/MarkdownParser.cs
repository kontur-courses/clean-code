using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class MarkdownParser
    {
        private static int _inputPosition;

        public static MarkdownTree Parse(string rawInput)
        {
            _inputPosition = 0;
            var resultTree = new MarkdownTree(new Tag(TagKind.Root, TagSide.None), "");
            resultTree.AddChild(ParseText(rawInput));
            return resultTree;
        }

        private static MarkdownTree ParseText(string rawInput)
        {
            var child = new MarkdownTree(new Tag(TagKind.PlainText, TagSide.None), rawInput);
            return child;
        }

        private static void MoveToNextPos()
        {
            _inputPosition++;
        }
    }
}
