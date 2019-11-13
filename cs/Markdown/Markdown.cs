using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{ 
    class Markdown
    {
        static Dictionary<string, string> HTML_ReplacementRules = new Dictionary<string, string>
        {
            ["_"] = "em",
            ["__"] = "strong",
            ["**"] = "lul",
            ["*_*"] = "xax"
        };

        public string Render(string input)
        {
            var allSeparatorsInInputString = SeparatorSearchTool.GetSeparators(input, HTML_ReplacementRules.Keys.ToList());
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

            int startedindex = 0;
            foreach (var separator in separators)
            {
                for (int i = startedindex; i < separator.Index; i++)
                    result.Append(input[i]);

                if (separator.Type == SeparatorType.Opening)
                    result.Append($"<{HTML_ReplacementRules[separator.Tag]}>");
                else
                    result.Append($"</{HTML_ReplacementRules[separator.Tag]}>");

                startedindex = separator.Index + separator.Tag.Length;
            }

            for (int i = separators.Last().Tag.Length + separators.Last().Index; i < input.Length; i++)
                result.Append(input[i]);

            return result.ToString();
        }
    }
}
