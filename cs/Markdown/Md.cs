namespace Markdown
{
    public class Md
    {
   
        public string Render(string text)
        {
            // var tokens = TokenReader.ReadTokens(text);
            // var correctTokens = tokens.GetCorrectTokens();
            // return Converter.ToHtml(text, tokens)
            var tokens = TokenReader.ReadTokens(text);
            return Converter.ToHtml(text, tokens);
        }
    }
}
