using System.Linq;

namespace Markdown
{
    public class Md
    {
        private readonly ITokenParser parser;
        private readonly ITagTranslator translator;

        public Md()
        {
            var underscoreTag = Tag.GetOrAddSymmetricTag("_");
            var emTag = Tag.GetOrAddPairTag("<em>", "</em>");
            var doubleUnderscoreTag = Tag.GetOrAddSymmetricTag("__");
            var strongTag = Tag.GetOrAddPairTag("<strong>", "</strong>");
            var firstLineSharpTag = Tag.GetOrAddSingleTag("# ");
            var h1Tag = Tag.GetOrAddPairTag("<h1>", "</h1>");
            var shieldTag = Tag.GetOrAddSingleTag("\\");
            var emptyTag = Tag.GetOrAddSingleTag("");
            var unixNewLineTag = Tag.GetOrAddSingleTag("\n\n");
            var windowsNewLineTag = Tag.GetOrAddSingleTag("\r\n");
            
            translator = TagTranslatorConfigurator
                .CreateTokenTranslator()
                .SetReference()
                    .From(underscoreTag).To(emTag)
                .SetReference()
                    .From(doubleUnderscoreTag).To(strongTag)
                .SetReference()
                    .From(firstLineSharpTag).To(h1Tag)
                .SetReference()
                    .From(shieldTag).To(emptyTag)
                .Configure();

            var forbiddenInnerTextSymbols = "1234567890 ".ToCharArray();

            parser = TokenParserConfigurator
                .CreateTokenParser()
                .SetShieldingSymbol(shieldTag)
                .AddTagInterruptToken(unixNewLineTag)
                .AddTagInterruptToken(windowsNewLineTag)
                .AddToken(underscoreTag).That
                    .CanBeNestedIn(doubleUnderscoreTag).And
                    .CanBeNestedIn(firstLineSharpTag).And
                    .CantContain(forbiddenInnerTextSymbols)
                .AddToken(doubleUnderscoreTag).That
                    .CanBeNestedIn(firstLineSharpTag).And
                    .CantContain(forbiddenInnerTextSymbols)
                .AddToken(firstLineSharpTag).That
                    .CanBeInFrontOnly()
                .Configure();
        }

        public string Render(string input)
        {
            var tokenSegments = parser
                .FindAllTokens(input)
                .SelectValid()
                .ToTokenSegments(parser.GetRules())
                .ToList();

            return parser.ReplaceTokens(input, tokenSegments, translator);
        }
    }
}