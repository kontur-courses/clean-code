using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{ 
    class Markdown
    {
        public static readonly Dictionary<string, int> MarkDownTagsPriority = new Dictionary<string, int>
        {
            ["_"] = 1,
            ["__"] = 2,
        };

        static readonly Dictionary<string, string> HtmlReplacementRules = new Dictionary<string, string>
        {
            ["_"] = "em",
            ["__"] = "strong",
        };

        public string Render(string inputString)
        {
            var markdownTagSymbols = HtmlReplacementRules.Keys.ToList();

            var allMarkDownTagsInInputString = TagSearchTool.GetMarkdownTags(inputString, markdownTagSymbols);
            var onlyCorrectTags = TagCleanTool.GetCorrectMarkdownTags(inputString, allMarkDownTagsInInputString);

            var outputString = inputString;
            outputString = ChangeTags(outputString, onlyCorrectTags, HtmlReplacementRules);
            outputString = RemoveShieldSymbols(outputString);

            return outputString;
        }

        string RemoveShieldSymbols(string input) => input.Replace(@"\", "");

        string ChangeTags(string inputString, List<Tag> tagsForChange, Dictionary<string, string> replacementRules)
        {
            if (tagsForChange.Count == 0) return inputString;

            var result = new StringBuilder();

            var startedIndex = 0;
            foreach (var tag in tagsForChange)
            {
                for (var i = startedIndex; i < tag.Index; i++)
                    result.Append(inputString[i]);

                result.Append(tag.Type == TagType.Opening
                    ? $"<{replacementRules[tag.Symbol]}>"
                    : $"</{replacementRules[tag.Symbol]}>");

                startedIndex = tag.Index + tag.Symbol.Length;
            }

            for (var i = tagsForChange.Last().Symbol.Length + tagsForChange.Last().Index; i < inputString.Length; i++)
                result.Append(inputString[i]);

            return result.ToString();
        }
    }
}
