using System.Collections.Generic;
using Markdown.Core.Infrastructure;
using Markdown.Core.Normalizer;
using Markdown.Core.Readers;
using Markdown.Core.Translators;

namespace Markdown.Core
{
    public class MdRenderer
    {
        public string Render(string source, List<IgnoreInsideRule> ignoreInsideRules)
        {
            var tokens = new ParagraphTokenReader().ReadTokens(source);

            tokens = new MdNormalizer().NormalizeTokens(tokens, ignoreInsideRules);

            return new MdToHtmlTranslator().TranslateTokensToHtml(tokens);
        }
    }
}