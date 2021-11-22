using System.Linq;
using System.Text;
using FluentAssertions;
using Markdown.Extensions;
using NUnit.Framework;

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
                .AddToken(underscoreTag).That
                    .CanBeNestedIn(doubleUnderscoreTag).And
                    .CanBeNestedIn(sharpTag)
                .AddToken(doubleUnderscoreTag).That
                    .CanBeNestedIn(sharpTag)
                .AddToken(sharpTag)
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
                    .ForEachPairs(parser.IgnoreSegmentsThatDoNotMatchRules);

                parsedText.Append(parser.ReplaceTokens(paragraph, SegmentsCollection.Union(tokenSegments), translator));
            }
    
            return parsedText.ToString();
        }
    }

    [TestFixture]
    public class MdRenderShould
    {
        [TestCase("Hello _World_!! __Th_is__ i_s __a__ __markdown _test_ sentence__", 
            "Hello <em>World</em>!! __Th_is__ i_s <strong>a</strong> <strong>markdown <em>test</em> sentence</strong>")]
        public void RenderTest(string text, string expectedResult)
        {
            var actual = new Md().Render(text);

            actual.Should().Be(expectedResult);
        }
    }
}