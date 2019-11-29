using System.Collections.Generic;
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

        public static string RenderHtml(string text)
        {
            var tokenizer = new Tokenizer(htmlWrapByMarkdownWrap.Keys, EscapeCharacter);
            var tokens = tokenizer.Process(text);

            ExcludeForbiddenChildTokens(tokens);

            return AstBuilder.BuildAst(tokens, htmlWrapByMarkdownWrap);
        }

        private static void ExcludeForbiddenChildTokens(IEnumerable<Token> tokens)
        {
            foreach (var token in tokens)
                if (!canWrapContainsOtherWrap[token.WrapType])
                    token.ChildTokens = null;
        }
    }
}