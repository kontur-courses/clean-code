using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.MdTags;
using Markdown.SyntaxTree;

namespace Markdown.Converters
{
    public static class Md
    {
        private static readonly List<char> TagsSymbols = new List<char>() {'_', '+', '*', '#'};

        public static string Render(string mdText)
        {
            if (string.IsNullOrEmpty(mdText))
            {
                return "";
            }

            var convertedParagraphs = mdText.Split('\n').Select(paragraph =>
                HandleEscapedSymbols(ConvertToHtml(paragraph, TreeConverter.ConvertToTree(paragraph))));
            return string.Join("\n", convertedParagraphs);
        }

        private static string PrintSingleTag(Tag tag, string text)
        {
            return tag.Type switch
            {
                TagType.None => text.Substring(tag.Start, tag.End - tag.Start),
                TagType.Italics => "<em>" + text.Substring(tag.Start + 1, tag.End - tag.Start - 1) + "</em>",
                TagType.StrongText => "<strong>" + text.Substring(tag.Start + 2, tag.End - tag.Start - 3) + "</strong>",
                TagType.UnnumberedList => "<ul>" + text.Substring(tag.Start + 1, tag.End - tag.Start - 1) + "</ul>",
                TagType.ListElement => "<li>" + text.Substring(tag.Start + 1, tag.End - tag.Start - 1) + "</li>",
                _ => "<h1>" + text.Substring(tag.Start + 1, tag.End - tag.Start - 1) + "</h1>"
            };
        }

        private static string ConvertToHtml(string paragraph, Tree currentTree)
        {
            var result = new StringBuilder();
            if (currentTree.Children.Count == 0)
            {
                return PrintSingleTag(currentTree.Root, paragraph);
            }

            for (var i = currentTree.Root.Start; i <= currentTree.Root.End && i < paragraph.Length; i++)
            {
                if (currentTree.Children.All(node => node.Root.Start != i))
                {
                    result.Append(paragraph[i]);
                }
                else
                {
                    result.Append(ConvertToHtml(paragraph, currentTree.Children.First(node => node.Root.Start == i)));
                    i = currentTree.Children.First(node => node.Root.Start == i).Root.End;
                }
            }

            var currentResult = result.ToString();
            return currentTree.Root.Type == TagType.None
                ? currentResult
                : PrintSingleTag(TagsConverter.GetAllTags(currentResult).Pop(), currentResult);
        }

        private static string HandleEscapedSymbols(string paragraph)
        {
            var slashesNumber = 0;
            var result = new StringBuilder();
            foreach (var symbol in paragraph)
            {
                if (!symbol.Equals('\\'))
                {
                    if (TagsSymbols.Contains(symbol))
                    {
                        if (slashesNumber != 0 && slashesNumber % 2 == 0)
                        {
                            result.Append('\\');
                        }

                        slashesNumber = 0;
                        result.Append(symbol);
                    }
                    else
                    {
                        if (slashesNumber != 0)
                        {
                            result.Append('\\');
                        }

                        result.Append(symbol);
                        slashesNumber = 0;
                    }
                }
                else
                {
                    slashesNumber++;
                }
            }

            if (slashesNumber != 0)
            {
                result.Append('\\');
            }

            return result.ToString();
        }
    }
}