using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Shell;
using Markdown.Tokenizer;

namespace Markdown
{
    public class Md
    {

        private HtmlRenderer htmlRenderer;
        public string CssAtribute { get; set; }
        public string BaseUrl { get; set; }

        private readonly List<IShell> mdShells = new List<IShell>()
        {
            new SingleUnderline(),
            new DoubleUnderline(),
            new UrlShell()
        };


        public string Render(string text)
        {
            htmlRenderer = new HtmlRenderer(MakeCommonAttributes());
            return GetHtmlCode(text, mdShells);
        }

        private List<Attribute> MakeCommonAttributes()
        {
            var result = new List<Attribute>();
            if (CssAtribute != null)
            {
                result.Add(new Attribute(CssAtribute, AttributeType.Style));
            }
            return result;
        }

        private string GetHtmlCode(string text, List<IShell> shells)
        {
            var result = new StringBuilder();
            var tokenizer = new StringTokenizer(text, shells);
            while (tokenizer.HasMoreTokens())
            {
                var token = tokenizer.NextToken();
                if (token.HasShell())
                {
                    var shellsInToken = shells.Where(s => token.Shell.Contains(s)).ToList();
                    token.Text = GetHtmlCode(token.Text, shellsInToken);
                    if (BaseUrl != null)
                    {
                        AddBaseUrl(BaseUrl, token.Attributes.Where(a => a.Type == AttributeType.Url));
                    }
                    result.Append(htmlRenderer.Render(token));
                }
                else
                {
                    var resultTextToken = htmlRenderer.Render(token).RemoveEscapeСharacters();
                    result.Append(resultTextToken);
                }
            }
            return result.ToString();
        }

        private static bool IsAbsoluteUrl(string url)
        {
            return url.Contains("://");
        }

        private static void AddBaseUrl(string baseUrl, IEnumerable<Attribute> urlAttributes)
        {
            foreach (var urlAttribute in urlAttributes)
            {
                if (IsAbsoluteUrl(urlAttribute.Value))
                {
                    continue;
                }
                urlAttribute.Value = baseUrl + urlAttribute.Value;
            }
        }
    }
}