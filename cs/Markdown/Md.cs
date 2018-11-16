using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var resultLine = markdown
                .Split('\n')
                .Select(ParseLine)
                .ToList();
            return string.Join("\n", resultLine);
        }

        private static string ParseLine(string markdown)
        {
            var isInsideEmTags = false;
            var strongTagsPairs = new Stack<(Marker, Marker)>();
            var line = markdown;

            var tagStack = new Stack<Marker>();
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == '\\')
                {
                    line = line.Remove(i, 1);
                    i++;
                }

                var tag = Marker.CreateTag(line, i);

                if (tag == null) continue;
                i += tag.Length - 1;

                if (Marker.IsClosingTag(line, tag.Pos) &&
                    tagStack.Any(openTag => openTag.Type == tag.Type))
                {
                    var tags = Marker.GetTagsPair(line, tag, tagStack);
                    if (tags.openingTag.Type == MarkerType.Em)
                    {
                        isInsideEmTags = false;
                        strongTagsPairs.Clear();
                    }
                    else if (tags.openingTag.Type == MarkerType.Strong && isInsideEmTags)
                    {
                        strongTagsPairs.Push(tags);
                        continue;
                    }

                    line = Marker.ConvertToHtmlTag(line, tags.openingTag, tags.closingTag);
                }

                else if (Marker.IsOpeningTag(line, tag.Pos))
                {
                    if (tag.Type == MarkerType.Em)
                    {
                        isInsideEmTags = true;
                        line = ConvertStrongTagFromStack(strongTagsPairs, line);
                    }

                    tagStack.Push(tag);
                }
            }

            line = ConvertStrongTagFromStack(strongTagsPairs, line);
            return line;
        }

        private static string ConvertStrongTagFromStack(Stack<(Marker, Marker)> strongTagsPairs, string line)
        {
            while (strongTagsPairs.Count > 0)
            {
                var tagsPair = strongTagsPairs.Pop();
                line = Marker.ConvertToHtmlTag(line, tagsPair.Item1, tagsPair.Item2);
            }

            return line;
        }
    }
}