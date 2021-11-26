using System;

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
        private static readonly Tag MdList1Tag = TagFactory.GetOrAddSingleTag("* ");
        private static readonly Tag MdList2Tag = TagFactory.GetOrAddSingleTag("+ ");
        private static readonly Tag MdList3Tag = TagFactory.GetOrAddSingleTag("- ");
        
        private static readonly Tag HtmlEmTag = TagFactory.GetOrAddPairTag("<em>", "</em>");
        private static readonly Tag HtmlStrongTag = TagFactory.GetOrAddPairTag("<strong>", "</strong>");
        private static readonly Tag HtmlH1Tag = TagFactory.GetOrAddPairTag("<h1>", "</h1>");
        private static readonly Tag HtmlLiTag = TagFactory.GetOrAddPairTag("<li>", "</li>");
        private static readonly Tag HtmlUlTag = TagFactory.GetOrAddPairTag("<ul>\n", "\n</ul>");
        
        private static readonly Tag EmptyTag = TagFactory.GetOrAddSingleTag("");
        private static readonly Tag UnixNewLineTag = TagFactory.GetOrAddSingleTag("\n\n");
        private static readonly Tag WindowsNewLineTag = TagFactory.GetOrAddSingleTag("\r\n\r\n");
        private static readonly Tag UnixIndentTag = TagFactory.GetOrAddSingleTag("\n");
        private static readonly Tag WindowsIndentTag = TagFactory.GetOrAddSingleTag("\r\n");

        static Md()
        {
            MdToHtmlTagTranslator = TagTranslatorConfigurator
                .CreateTokenTranslator()
                .SetReference().From(MdUnderscoreTag).To(HtmlEmTag)
                .SetReference().From(MdDoubleUnderscoreTag).To(HtmlStrongTag)
                .SetReference().From(MdSharpTag).To(HtmlH1Tag)
                .SetReference().From(MdShieldTag).To(EmptyTag)
                .SetReference().From(MdList1Tag).To(HtmlLiTag)
                .SetReference().From(MdList2Tag).To(HtmlLiTag)
                .SetReference().From(MdList3Tag).To(HtmlLiTag)
                .Configure();

            var forbiddenInnerTextSymbols = "1234567890 ".ToCharArray();

            MdToHtmlParser = TokenParserConfigurator
                .CreateTokenParser()
                .AddTagsShell(HtmlUlTag, MdList1Tag, MdList2Tag, MdList3Tag)
                .SetShieldingSymbol(MdShieldTag)
                .AddNewLineToken(UnixIndentTag)
                .AddTagInterruptToken(UnixNewLineTag)
                .AddNewLineToken(WindowsIndentTag)
                .AddTagInterruptToken(WindowsNewLineTag)
                .AddToken(MdUnderscoreTag).That
                    .CanBeNestedIn(MdDoubleUnderscoreTag).And
                    .CanBeNestedIn(MdList1Tag).And
                    .CanBeNestedIn(MdList2Tag).And
                    .CanBeNestedIn(MdList3Tag).And
                    .CantContain(forbiddenInnerTextSymbols).And
                    .CanBeShielded()
                .AddToken(MdDoubleUnderscoreTag).That
                    .CanBeNestedIn(MdSharpTag).And
                    .CanBeNestedIn(MdList1Tag).And
                    .CanBeNestedIn(MdList2Tag).And
                    .CanBeNestedIn(MdList3Tag).And
                    .CantContain(forbiddenInnerTextSymbols).And
                    .CanBeShielded()
                .AddToken(MdSharpTag).That
                    .CanBeNestedIn(MdList1Tag).And
                    .CanBeNestedIn(MdList2Tag).And
                    .CanBeNestedIn(MdList3Tag).And
                    .CanBeInFrontOnly().And
                    .CanBeShielded()
                .AddToken(MdList1Tag).That
                    .CanBeInFrontOnly().And
                    .CanBeShielded()
                .AddToken(MdList2Tag).That
                    .CanBeInFrontOnly().And
                    .CanBeShielded()
                .AddToken(MdList3Tag).That
                    .CanBeInFrontOnly().And
                    .CanBeShielded()
                .Configure();
        }

        public static string Render(string? input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
            
            var tokenSegments = MdToHtmlParser
                .FindAllTokens(input)
                .ToTokenSegments(MdToHtmlParser.GetRules());
            return MdToHtmlParser.ReplaceTokens(input, tokenSegments, MdToHtmlTagTranslator);
        }
    }
}