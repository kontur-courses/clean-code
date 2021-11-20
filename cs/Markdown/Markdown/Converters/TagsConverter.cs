using System;
using System.Collections.Generic;
using Markdown.Tag;

namespace Markdown.Converters
{
    public static class TagsConverter
    {
        public static Stack<ITag> GetAllTags(string mdText)
        {
            if (mdText == null)
            {
                return new Stack<ITag>();
            }

            var result = new Stack<ITag>();
            var stack = new Stack<Tuple<TagType, int>>();
            for (var i = 0; i < mdText.Length; i++)
            {
                var typeAndBound = GetBoundOfTag(mdText, i);
                switch (typeAndBound.Item1)
                {
                    case TagType.None:
                        continue;
                    case TagType.StrongText:
                        i++;
                        break;
                }

                if (stack.Count != 0 && stack.Peek().Item1 == typeAndBound.Item1)
                {
                    result.Push(TagBuilder.OfType(typeAndBound.Item1).WithBounds(stack.Pop().Item2, i));
                }
                else
                {
                    stack.Push(Tuple.Create(typeAndBound.Item1, i));
                }
            }

            var lastBoundary = Tuple.Create(TagType.Title, mdText.Length);
            while (stack.Count != 0)
            {
                if (stack.Count == 0)
                {
                    break;
                }

                if (lastBoundary.Item1 != stack.Peek().Item1)
                {
                    stack.Pop();
                }
                else
                {
                    var index = stack.Pop().Item2;
                    result.Push(TagBuilder.OfType(GetBoundOfTag(mdText, index).Item1)
                        .WithBounds(index, lastBoundary.Item2));
                }
            }

            return result;
        }

        private static Tuple<TagType, string> GetBoundOfTag(string text, int index)
        {
            var symbol = text[index];
            switch (symbol)
            {
                case '_' when index != text.Length - 1 && text[index + 1].Equals('_'):
                    return Tuple.Create(TagType.StrongText, "__");
                case '_':
                    return Tuple.Create(TagType.Italics, "_");
                case '#':
                case '\n':
                    return Tuple.Create(TagType.Title, symbol.ToString());
                default:
                    return Tuple.Create(TagType.None, "");
            }
        }
    }
}