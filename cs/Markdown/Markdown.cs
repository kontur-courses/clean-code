using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{ 
    class Markdown
    {
        static readonly Dictionary<string, string> HtmlReplacementRules = new Dictionary<string, string>
        {
            ["_"] = "em",
            ["__"] = "strong",
            ["**"] = "lul",
            ["*_*"] = "xax"
        };

        public string Render(string inputString)
        {
            var allMarkDownTagsInInputString = TagSearchTool.GetMarkdownTags(inputString, HtmlReplacementRules.Keys.ToList());
            var onlyCorrectTags = TagCleanTool.GetCorrectMarkdownTags(inputString, allMarkDownTagsInInputString);

            var outputString = inputString;
            outputString = RemoveShieldSymbols(outputString);
            outputString = ChangeTags(outputString, onlyCorrectTags, HtmlReplacementRules);

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
