namespace Markdown.Tokens
{
    internal class Root : RawText
    {
        public string Document;

        public Root(ref string document)
        {
            Document = document;
            BeginIndex = 0;
            EndIndex = document.Length;
        }
    }
}
