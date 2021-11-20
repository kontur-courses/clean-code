using System.Linq;
using System.Text;
using Markdown.Extensions;

namespace Markdown
{
    public class Md
    {
        private readonly ITokenParser parser;
        private readonly ITokenTranslator translator;

        public Md()
        {
            var underscoreTag = Tag.RegisterSymmetricTag("_");
            var emTag = Tag.RegisterPairTag("<em>", "</em>");
            var doubleUnderscoreTag = Tag.RegisterSymmetricTag("__");
            var strongTag = Tag.RegisterPairTag("<strong>", "</strong>");
            var sharpTag = Tag.RegisterSingleTag("#");
            var h1Tag = Tag.RegisterPairTag("<h1>", "</h1>");
            
            translator = TokenTranslatorConfigurator
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
                .AddToken(new Token(underscoreTag.Start)).That
                    .CanBeNestedIn(doubleUnderscoreTag).And
                    .CantIntersect()
                .AddToken(new Token(doubleUnderscoreTag.Start)).That
                    .CantBeNestedIn(underscoreTag).And
                    .CantIntersect()
                .AddToken(new Token(sharpTag.Start)).That
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
                    .Validate()
                    .GroupToDictionaries(x => x.Value.Token.ToString())
                    .Select(TokenSegment.GetTokensSegments)
                    .ForEachPairs(parser.ValidatePairSetsByRules)
                    .Aggregate((f, s) => f.Union(s));

                parsedText.Append(parser.ReplaceTokens(tokenSegments, translator));
            }
    
            return parsedText.ToString();
        }
    }
}