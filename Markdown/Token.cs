namespace Markdown
{
    public class Token
    {
        public readonly string Text;
        public readonly IShell Shell;
        public Token(string text, IShell shell = null)
        {
            Text = text;
            Shell = shell;
        }

        public bool HasShell()
        {
            return Shell != null;
        }
    }
}
