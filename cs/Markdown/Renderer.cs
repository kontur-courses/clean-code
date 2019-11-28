namespace Markdown
{
    public class Renderer
    {        
        public string Render(string text)
        {
            var tokenizer = new Tokenizer();            
            var m = tokenizer.GetTokens(text);            
            return new RendererToHTML().ToHTML(text, m);            
        }
    }
}
