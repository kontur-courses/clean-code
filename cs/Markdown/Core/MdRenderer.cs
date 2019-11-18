using Markdown.Core.Readers;

namespace Markdown.Core
{
    public class MdRenderer
    {   
        public string Render(string source)
        {
            var tokens = new TokenReader().ReadTokens(source);
            
            tokens = new MdNormalizer().NormalizeTokens(tokens, "__");
                        
            return new MdToHtmlTranslator().TranslateTokensToHtml(tokens);
        }
    }
}