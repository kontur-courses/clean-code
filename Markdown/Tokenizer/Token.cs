using Markdown.Shell;

namespace Markdown.Tokenizer
{
    public class Token
    {
        public string Text { get; }
        public IShell Shell { get; }
        public Token(string text, IShell shell = null)
        {
            Text = text;
            Shell = shell;
        }

        public bool HasShell()
        {
            return Shell != null;
        }

        public string RenderToHtml()
        {
            return Shell?.RenderToHtml(Text) ?? Text;
        }
    }
}
