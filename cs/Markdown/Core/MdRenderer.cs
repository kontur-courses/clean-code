using Markdown.Core.Infrastructure;
using Markdown.Core.Readers;
using Markdown.Core.Translators;

namespace Markdown.Core
{
    public class MdRenderer
    {   
        public string Render(string source)
        {
            var tokens = new ParagraphTokenReader().ReadTokens(source);
            
            tokens = new MdNormalizer().NormalizeTokens(tokens, TagsUtils.GetMdTagByTagName("strong"));
                        
            return new MdToHtmlTranslator().TranslateTokensToHtml(tokens);
        }
    }
}