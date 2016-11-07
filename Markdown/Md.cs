using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class Md
    {
        private List<Shell> mdShells; 
        public Md()
        {
            throw new NotImplementedException();
        }

        public string Render(string text)
        {
            return GetHtmlCode(text, mdShells);
        }

        private static string GetFramedText(string text, Shell shell)
        {
            throw new NotImplementedException();
        }

        private static string GetHtmlCode(string text, List<Shell> shells)
        {
            var result = new StringBuilder();
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.SplitToTokens(text, shells);
            foreach (var token in tokens)
            {
                if (token.HasShell())
                {
                    var shellsInToken = shells.Where(s => s.IncludedInsideShell(token.Shell)).ToList();
                    var resultTextToken = GetHtmlCode(token.Text, shellsInToken);
                    result.Append(GetFramedText(resultTextToken, token.Shell));
                }
                else
                {
                    result.Append(token.Text);
                }
            }
            return result.ToString();
        }
    }
}
