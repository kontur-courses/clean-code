using System.Linq;

namespace Markdown
{
    public static class Md
    {
        private static readonly ITokenParser MdToHtmlParser;
        private static readonly ITagTranslator MdToHtmlTagTranslator;
        
        private static readonly Tag MdUnderscoreTag = Tag.GetOrAddSymmetricTag("_");
        private static readonly Tag MdDoubleUnderscoreTag = Tag.GetOrAddSymmetricTag("__");
        private static readonly Tag MdSharpTag = Tag.GetOrAddSingleTag("# ");
        private static readonly Tag MdShieldTag = Tag.GetOrAddSingleTag("\\");
        
        private static readonly Tag HtmlEmTag = Tag.GetOrAddPairTag("<em>", "</em>");
        private static readonly Tag HtmlStrongTag = Tag.GetOrAddPairTag("<strong>", "</strong>");
        private static readonly Tag HtmlH1Tag = Tag.GetOrAddPairTag("<h1>", "</h1>");
        
        private static readonly Tag EmptyTag = Tag.GetOrAddSingleTag("");
        private static readonly Tag UnixNewLineTag = Tag.GetOrAddSingleTag("\n\n");
        private static readonly Tag WindowsNewLineTag = Tag.GetOrAddSingleTag("\r\n");

        static Md()
        {
            MdToHtmlTagTranslator = TagTranslatorConfigurator
                .CreateTokenTranslator()
                .SetReference().From(MdUnderscoreTag).To(HtmlEmTag)
                .SetReference().From(MdDoubleUnderscoreTag).To(HtmlStrongTag)
                .SetReference().From(MdSharpTag).To(HtmlH1Tag)
                .SetReference().From(MdShieldTag).To(EmptyTag)
                .Configure();

            var forbiddenInnerTextSymbols = "1234567890 ".ToCharArray();

            MdToHtmlParser = TokenParserConfigurator
                .CreateTokenParser()
                .SetShieldingSymbol(MdShieldTag)
                .AddTagInterruptToken(UnixNewLineTag)
                .AddTagInterruptToken(WindowsNewLineTag)
                .AddToken(MdUnderscoreTag).That
                    .CanBeNestedIn(MdDoubleUnderscoreTag).And
                    .CanBeNestedIn(MdSharpTag).And
                    .CantContain(forbiddenInnerTextSymbols)
                .AddToken(MdDoubleUnderscoreTag).That
                    .CanBeNestedIn(MdSharpTag).And
                    .CantContain(forbiddenInnerTextSymbols)
                .AddToken(MdSharpTag).That
                    .CanBeInFrontOnly()
                .Configure();
        }

        public static string Render(string input)
        {
            var tokenSegments = MdToHtmlParser
                .FindAllTokens(input)
                .ToTokenSegments(MdToHtmlParser.GetRules());
            return MdToHtmlParser.ReplaceTokens(input, tokenSegments, MdToHtmlTagTranslator);
        }
    }
}