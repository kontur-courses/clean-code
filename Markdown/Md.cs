using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly ITokenParser parser;
        private readonly ITokenTranslator translator;

        public Md()
        {
            var underscoreToken = Token.GetSymmetricToken("_");
            var doubleUnderscoreToken = Token.GetSymmetricToken("__");
            var sharpToken = Token.GetSingleToken("#");
            
            translator = TokenTranslatorConfigurator
                .CreateTokenTranslator()
                .SetReference()
                    .From(underscoreToken)
                    .To(Token.GetPairToken("<em>", Token.GetSingleToken("</em>")))
                .SetReference()
                    .From(doubleUnderscoreToken)
                    .To(Token.GetPairToken("<strong>", Token.GetSingleToken("</strong>")))
                .SetReference()
                    .From(sharpToken)
                    .To(Token.GetPairToken("<h1>", Token.GetSingleToken("</h1>")))
                .Configure();
            
            parser = TokenParserConfigurator
                .CreateTokenParser()
                .SetShieldingSymbol('\\')
                .AddToken(underscoreToken).That
                    .CanBeNestedIn(doubleUnderscoreToken).And
                    .CantIntersect()
                .AddToken(doubleUnderscoreToken).That
                    .CantBeNestedIn(underscoreToken).And
                    .CantIntersect()
                .AddToken(sharpToken).That
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
                parser.ParseParagraph(paragraph);

                var tokenSegments = parser
                    .FindAllTokens()
                    .Validate()
                    .GroupToDictionariesBy(x => x.Value.Token.ToString())
                    .Select(g => parser.GetTokensSegments(g))
                    .ForEachPairs(parser.ValidatePairSets)
                    .Aggregate((f, s) => f.Union(s));

                parsedText.Append(parser.ReplaceTokens(tokenSegments, translator));
            }
    
            return parsedText.ToString();
        }
    }
}