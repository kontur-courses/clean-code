using System.Linq;
using System.Text;
using Markdown.Extensions;

namespace Markdown
{
    public class Md
    {
        private readonly ITokenParser parser;
        private readonly ITagTranslator translator;

        public Md()
        {
            var underscoreTag = Tag.RegisterSymmetricTag("_");
            var emTag = Tag.RegisterPairTag("<em>", "</em>");
            var doubleUnderscoreTag = Tag.RegisterSymmetricTag("__");
            var strongTag = Tag.RegisterPairTag("<strong>", "</strong>");
            var sharpTag = Tag.RegisterSingleTag("#");
            var h1Tag = Tag.RegisterPairTag("<h1>", "</h1>");
            
            translator = TagTranslatorConfigurator
                .CreateTokenTranslator()
                .SetReference()
                    .From(underscoreTag).To(emTag)
                .SetReference()
                    .From(doubleUnderscoreTag).To(strongTag)
                .SetReference()
                    .From(sharpTag).To(h1Tag)
                .Configure();

            parser = TokenParserConfigurator
                .CreateTokenParser()
                .SetShieldingSymbol('\\')
                .AddToken(underscoreTag.Start).That
                    .CanBeNestedIn(doubleUnderscoreTag).And
                    .CantIntersect()
                .AddToken(doubleUnderscoreTag.Start).That
                    .CantBeNestedIn(underscoreTag).And
                    .CantIntersect()
                .AddToken(sharpTag.Start).That
                    .CanIntersectWithAnyTokens().And
                    .CanBeNestedInAnyTokens()
                .Configure();
        }

        public string Render(string input)
        {
            var paragraphs = input.Split('\n');
            var parsedText = new StringBuilder();
            
            foreach (var paragraph in paragraphs)
            {
                var tokenSegments = parser
                    .FindAllTokens(paragraph)
                    .SelectValid()
                    .GroupBy(x => x.Token.ToString())
                    .Select(x => x.ToSegmentsCollection())
                    .ToList()
                    .ForEachPairs(parser.ValidatePairSetsByRules);

                parsedText.Append(parser.ReplaceTokens(paragraph, SegmentsCollection.Union(tokenSegments), translator));
            }
    
            return parsedText.ToString();
        }
    }
}