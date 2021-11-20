using System.Linq;
using System.Text;
using Markdown.Tag;
using Markdown.Tree;

namespace Markdown.Converters
{
    public static class Md
    {
        public static string Render(string mdText)
        {
            return ConvertToHtml(mdText, TreeConverter.ConvertToTree(mdText));
        }

        private static string PrintSingleTag(ITag tag, string text)
        {
            return tag.Type switch
            {
                TagType.None => text.Substring(tag.Start, tag.End - tag.Start),
                TagType.Italics => "<em>" + text.Substring(tag.Start + 1, tag.End - tag.Start - 1) + "</em>",
                TagType.StrongText => "<strong>" + text.Substring(tag.Start + 2, tag.End - tag.Start - 3) + "</strong>",
                _ => "<h1>" + text.Substring(tag.Start + 1, tag.End - tag.Start - 1) + "</h1>"
            };
        }

        private static string ConvertToHtml(string mdText, Node tree)
        {
            if (mdText == null)
            {
                return "";
            }

            var result = new StringBuilder();

            if (tree.Children.Count == 0)
            {
                return PrintSingleTag(tree.Tag, mdText);
            }

            for (var i = tree.Tag.Start; i <= tree.Tag.End && i < mdText.Length; i++)
            {
                if (tree.Children.All(node => node.Tag.Start != i))
                {
                    result.Append(mdText[i]);
                }
                else
                {
                    result.Append(ConvertToHtml(mdText, tree.Children.First(node => node.Tag.Start == i)));
                    i = tree.Children.First(node => node.Tag.Start == i).Tag.End;
                }
            }

            var currentResult = result.ToString();
            return tree.Tag.Type == TagType.None
                ? currentResult
                : PrintSingleTag(TagsConverter.GetAllTags(currentResult).Pop(), currentResult);
        }
    }
}