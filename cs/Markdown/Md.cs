namespace Markdown
{
    public class Md
    {
        private readonly ParsersHandler _handler;
        public Md(ParsersHandler handler)
        {
            _handler = handler;
        }
        
        public string Render(string markdownText)
        {
            return _handler.Handle(markdownText);
        }
        
        private string ConvertToHtml(string text)
        {
            throw new NotImplementedException();
        }
    }
}

