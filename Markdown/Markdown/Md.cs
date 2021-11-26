using System.Linq;

namespace Markdown
{
    public static class Md
    {
        private static readonly ITokenParser MdToHtmlParser;
        private static readonly ITagTranslator MdToHtmlTagTranslator;
        
        private static readonly Tag MdUnderscoreTag = TagFactory.GetOrAddSymmetricTag("_");
        private static readonly Tag MdDoubleUnderscoreTag = TagFactory.GetOrAddSymmetricTag("__");
        private static readonly Tag MdSharpTag = TagFactory.GetOrAddSingleTag("# ");
        private static readonly Tag MdShieldTag = TagFactory.GetOrAddSingleTag("\\");
        
        private static readonly Tag HtmlEmTag = TagFactory.GetOrAddPairTag("<em>", "</em>");
        private static readonly Tag HtmlStrongTag = TagFactory.GetOrAddPairTag("<strong>", "</strong>");
        private static readonly Tag HtmlH1Tag = TagFactory.GetOrAddPairTag("<h1>", "</h1>");
        
        private static readonly Tag EmptyTag = TagFactory.GetOrAddSingleTag("");
        private static readonly Tag UnixNewLineTag = TagFactory.GetOrAddSingleTag("\n\n");
        private static readonly Tag WindowsNewLineTag = TagFactory.GetOrAddSingleTag("\r\n\r\n");

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
                    .CantContain(forbiddenInnerTextSymbols).And
                    .CanBeShielded()
                .AddToken(MdDoubleUnderscoreTag).That
                    .CanBeNestedIn(MdSharpTag).And
                    .CantContain(forbiddenInnerTextSymbols).And
                    .CanBeShielded()
                .AddToken(MdSharpTag).That
                    .CanBeInFrontOnly().And
                    .CanBeShielded()
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