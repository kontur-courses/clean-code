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

        public string Render(string input)
        {
            var allSeparatorsInInputString = SeparatorSearchTool.GetSeparators(input, HtmlReplacementRules.Keys.ToList());
            var onlyCorrectSeparators = SeparatorCleanTool.GetCorrectSeparators(input, allSeparatorsInInputString);

            var outputString = input;
            outputString = RemoveShieldSymbols(outputString);
            outputString = SwitchSeparatorsToHTMLTags(outputString, onlyCorrectSeparators);

            return outputString;
        }

        string RemoveShieldSymbols(string input) => input.Replace(@"\", "");

        string SwitchSeparatorsToHTMLTags(string input, List<Separator> separators)
        {
            if (separators.Count == 0) return input;

            var result = new StringBuilder();

            var startedIndex = 0;
            foreach (var separator in separators)
            {
                for (var i = startedIndex; i < separator.Index; i++)
                    result.Append(input[i]);

                result.Append(separator.Type == SeparatorType.Opening
                    ? $"<{HtmlReplacementRules[separator.Tag]}>"
                    : $"</{HtmlReplacementRules[separator.Tag]}>");

                startedIndex = separator.Index + separator.Tag.Length;
            }

            for (var i = separators.Last().Tag.Length + separators.Last().Index; i < input.Length; i++)
                result.Append(input[i]);

            return result.ToString();
        }
    }
}
