using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Shell;
using Markdown.Tokenizer;

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
            var tokenizer = new StringTokenizer(text, shells);
            while (tokenizer.HasMoreTokens())
            {
                var token = tokenizer.NextToken();
                if (token.HasShell())
                {
                    var shellsInToken = shells.Where(s => token.Shell.Contains(s)).ToList();
                    var resultTextToken = GetHtmlCode(token.Text, shellsInToken);
                    result.Append(token.Shell.RenderToHtml(resultTextToken));
                }
                else
                {
                    var resultTextToken = token.RenderToHtml().RemoveEscapeСharacters();
                    result.Append(resultTextToken);
                }
            }
            return result.ToString();
        }
    }
}