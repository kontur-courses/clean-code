using Markdown.LocalMarkdown;
using System.Text;

namespace Markdown
{
    internal static class MarkdownRenderer
    {
        public static string RenderLine(string line)
        {
            var actionList = GenerateMarkdownActionList(line);
            return ConvertWithMarkdown(line, actionList);
        }

        private static MarkdownActionType[] GenerateMarkdownActionList(string line)
        {
            var actionList = new MarkdownActionType[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '_')
                {
                    var c = new CursiveMarkdownMaker(line, i, line.Length - 1);
                    c.MakeSubstringMarkdown(actionList);
                    i = c.EndIndex;
                }
            }

            return actionList;
        }

        private static string ConvertWithMarkdown(string line, MarkdownActionType[] actionList)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                switch (actionList[i])
                {
                    case MarkdownActionType.None:
                        if (!triggers.Contains(line[i]))
                            sb.Append(line[i]);
                        break;
                    default:
                        sb.Append(MarkdownToHTMLTagConverter.GetTagByMarkdownAction(actionList[i]));
                        break;
                }
            }

            return sb.ToString();
        }

        private static char[] triggers =
        {
            '-',
        };
    }
}