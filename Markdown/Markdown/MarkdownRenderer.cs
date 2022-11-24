using System.ComponentModel.DataAnnotations;
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
                LocalMarkdownMaker mdMaker = null;
                if (line[i] == '_')
                {
                    if (i + 1 < line.Length && line[i + 1] == '_')
                    {
                        mdMaker = new BoldMarkdownMaker(line, i, line.Length - 1);
                    }
                    else
                    {
                        mdMaker = new CursiveMarkdownMaker(line, i, line.Length - 1);
                    }
                }

                if (mdMaker != null)
                {
                    mdMaker.MakeSubstringMarkdown(actionList);
                    i = mdMaker.EndIndex;
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
                    case MarkdownActionType.NotRendered:
                        continue;
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