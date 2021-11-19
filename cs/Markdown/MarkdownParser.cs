using System;

namespace Markdown
{
    public static class MarkdownParser
    {
        private static int _inputPosition;

        public static MarkdownTree Parse(string rawInput)
        {
            _inputPosition = 0;
            var resultTree = new MarkdownTree(new Tag(TagKind.Root, TagSide.None));
            return resultTree;
        }

        private static void MoveToNextPos()
        {
            _inputPosition++;
        }
    }
}
