namespace Markdown
{
    class Token
    {
        public readonly string Text;
        public readonly Shell Shell;
        public Token(string text, Shell shell = null)
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
