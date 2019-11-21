using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class MarkdownTransformerToHtml
    {
        internal static readonly Dictionary<string, IMarkdownTagInfo> TagsInfo =
            new Dictionary<string, IMarkdownTagInfo>
            {
                ["_"] = new EmphasisTag(),
                ["__"] = new StrongTag(),
                ["`"] = new CodeTag(),
            };

        internal static string Render(string inputString)
        {
            var markdownTagDesignations = TagsInfo.Keys.ToArray();
            var tagParser = new TagParser(markdownTagDesignations);

            var correctTags = tagParser.Parse(inputString)
                .RemoveEscapedTags(inputString)
                .OrderBy(tag => tag.Index) //Не придумал как избавиться от сортировки :/
                .RemoveUnopenedTags()
                .OrderBy(tag => tag.Index) //Но даже с двумя сортировками программа работает относительно быстро
                .RemoveIncorrectNestingTags()
                .ToList();

            var outputString = inputString;
            outputString = StringRedactor.SwitchMarkdownTagsToHtml(outputString, correctTags);
            outputString = StringRedactor.RemoveEscapeSymbols(outputString);

            return outputString;
        }
    }
}
