using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly List<IShell> mdShells = new List<IShell>()
        {
            new SingleUnderline(),
            new DoubleUnderline()
        }; 

        public string Render(string text)
        {
            return GetHtmlCode(text, mdShells);
        }

        private static string GetHtmlCode(string text, List<IShell> shells)
        {
            var result = new StringBuilder();
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.SplitToTokens(text, shells);
            foreach (var token in tokens)
            {
                if (token.HasShell())
                {
                    var shellsInToken = shells.Where(s => token.Shell.Contains(s)).ToList();
                    var resultTextToken = GetHtmlCode(token.Text, shellsInToken);
                    result.Append(token.Shell.RenderToHtml(resultTextToken));
                }
                else
                {
                    result.Append(token.RenderToHtml());
                }
            }
            return result.ToString();
        }
    }
}
