using System.Text;

namespace Markdown
{
    public class Markdown
    {
        private readonly SymbolParser parser = new SymbolParser();
        private readonly RulesChecker checker = new RulesChecker();

        public string Render(string mdText)
        {
            var tagPosition = parser.GetTagsPosition(mdText);
            var correctTagPosition = checker.CheckCorrectness(tagPosition);
            var htmlText = new StringBuilder();
            for (var i = 0; i < mdText.Length; i++)
            {
                if (correctTagPosition.ContainsKey(i))
                {
                    var tag = correctTagPosition[i];
                    if (tag == "backslash")
                        continue;
                    htmlText.Append(tag);
                    if (tag.EndsWith("strong>"))
                        i++;
                }
                else
                {
                    htmlText.Append(mdText[i]);
                }
            }

            return htmlText.ToString();
        }
    }
}