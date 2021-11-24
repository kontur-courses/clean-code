using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

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
            var sharpTag = Tag.GetOrAddSingleTag("# ");
            var h1Tag = Tag.GetOrAddPairTag("<h1>", "</h1>");
            var shieldTag = Tag.GetOrAddSingleTag("\\");
            var emptyTag = Tag.GetOrAddSingleTag("");
            
            translator = TagTranslatorConfigurator
                .CreateTokenTranslator()
                .SetReference()
                    .From(underscoreTag).To(emTag)
                .SetReference()
                    .From(doubleUnderscoreTag).To(strongTag)
                .SetReference()
                    .From(sharpTag).To(h1Tag)
                .SetReference()
                    .From(shieldTag).To(emptyTag)
                .Configure();

            var forbiddenInnerTextSymbols = "1234567890 ".ToCharArray();

            parser = TokenParserConfigurator
                .CreateTokenParser()
                .SetShieldingSymbol(shieldTag)
                .AddToken(underscoreTag).That
                    .CanBeNestedIn(doubleUnderscoreTag).And
                    .CanBeNestedIn(sharpTag).And
                    .CantContain(forbiddenInnerTextSymbols)
                .AddToken(doubleUnderscoreTag).That
                    .CanBeNestedIn(sharpTag).And
                    .CantContain(forbiddenInnerTextSymbols)
                .AddToken(sharpTag).That
                    .CanBeInFrontOnly()
                .Configure();
        }

        public string Render(string input)
        {
            var paragraphs = input.Split('\n');
            var parsedText = new List<string>();
            
            foreach (var paragraph in paragraphs)
            {
                var tokenSegments = parser
                    .FindAllTokens(paragraph)
                    .SelectValid()
                    .ToTokenSegments(parser.GetRules())
                    .ToList();

                parsedText.Add(parser.ReplaceTokens(paragraph, new SegmentsCollection(tokenSegments), translator));
            }
    
            return string.Join('\n', parsedText);
        }
    }
}