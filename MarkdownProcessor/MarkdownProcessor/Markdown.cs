using System.Collections.Generic;
using System.Linq;
using Markdown.Wraps;

namespace MarkdownProcessor
{
    public static class Markdown
    {
        private const char EscapeCharacter = '\\';

        private static readonly IWrapType singleUnderscoreWrapType = new SingleUnderscoreWrapType();
        private static readonly IWrapType doubleUnderscoresWrapType = new DoubleUnderscoresWrapType();
        private static readonly IWrapType textWrapType = new TextWrapType();

        private static readonly IReadOnlyDictionary<IWrapType, IWrapType> htmlWrapByMarkdownWrap =
            new Dictionary<IWrapType, IWrapType>
            {
                [singleUnderscoreWrapType] = new HtmlEmphasisWrapType(),
                [doubleUnderscoresWrapType] = new HtmlStrongWrapType(),
                [textWrapType] = new TextWrapType()
            };

        private static readonly IReadOnlyDictionary<IWrapType, bool> canWrapContainsOtherWrap =
            new Dictionary<IWrapType, bool>
            {
                [singleUnderscoreWrapType] = false,
                [doubleUnderscoresWrapType] = true,
                [textWrapType] = true
            };

        public static string RenderHtml(string markdownText)
        {
            var tokenWrapTypes = htmlWrapByMarkdownWrap
                                 .Keys.Where(wrapType => !string.IsNullOrEmpty(wrapType.OpenWrapMarker) &&
                                                         !string.IsNullOrEmpty(wrapType.CloseWrapMarker))
                                 .ToArray();

            var tokenizer = new Tokenizer(tokenWrapTypes, EscapeCharacter);
            var tokens = tokenizer.Process(markdownText);

            ExcludeForbiddenChildTokens(tokens);

            return AstBuilder.BuildAst(tokens, wrapType => htmlWrapByMarkdownWrap[wrapType]);
        }

        private static void ExcludeForbiddenChildTokens(IEnumerable<Token> tokens)
        {
            foreach (var token in tokens)
                if (!canWrapContainsOtherWrap[token.WrapType])
                    token.ChildTokens = null;
        }
    }
}