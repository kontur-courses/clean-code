﻿using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Extensions;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class ParserShould
    {
        private ITokenParser parser;
        private ITagTranslator translator;
        
        [SetUp]
        public void SetUp()
        {
            Tag.ClearTagBase();
            
            var fromTag1 = Tag.GetOrAddSymmetricTag("-");
            var fromTag2 = Tag.GetOrAddSymmetricTag("--");
            var fromTag3 = Tag.GetOrAddSymmetricTag("__");
            var fromTag4 = Tag.GetOrAddSymmetricTag("_<");
            
            var toTag1 = Tag.GetOrAddPairTag("<", "/>");
            var toTag2 = Tag.GetOrAddPairTag("<*", "/*>");
            var toTag3 = Tag.GetOrAddPairTag("(<*", "/*>)");
            var toTag4 = Tag.GetOrAddPairTag("<H1>", "</H1>");
            
            parser = TokenParserConfigurator.CreateTokenParser()
                .AddToken(fromTag1).That
                    .CanBeNestedIn(fromTag2).And
                    .CanBeNestedIn(fromTag3).And
                    .CanBeNestedIn(fromTag4)
                .AddToken(fromTag2).That
                    .CanBeNestedIn(fromTag3).And
                    .CanBeNestedIn(fromTag4)
                .AddToken(fromTag3).That
                    .CanBeNestedIn(fromTag4)
                .AddToken(fromTag4)
                .Configure();

            translator = TagTranslatorConfigurator
                .CreateTokenTranslator()
                .SetReference().From(fromTag1).To(toTag1)
                .SetReference().From(fromTag2).To(toTag2)
                .SetReference().From(fromTag3).To(toTag3)
                .SetReference().From(fromTag4).To(toTag4)
                .Configure();
        }

        [TestCase("a__b-c__d", "a(<*b-c/*>)d")]
        
        [TestCase("a__b-d-c__d", "a(<*b<d/>c/*>)d")]
        [TestCase("a__-d-__d", "a(<*<d/>/*>)d")]
        [TestCase("a-__d__-d", "a<__d__/>d")]//
        [TestCase("a_<__-d-___<d", "a<H1>(<*<d/>/*>)</H1>d")]
        
        [TestCase("a_<__d-_<d", "a<H1>__d-</H1>d")]
        [TestCase("a---bb---a", "a<*<bb/>/*>a")]
        [TestCase("a---bb--a", "a--<bb/>-a")]
        
        [TestCase("a-bc-d", "a<bc/>d")]
        [TestCase("a-bc-da-bc-d", "a<bc/>da<bc/>d")]
        [TestCase("a--bc--d", "a<*bc/*>d")]
        [TestCase("a--bc--da-bc-d", "a<*bc/*>da<bc/>d")]
        public void ReplaceTokens_Test(string text, string expected)
        {
            var segments = parser
                .FindAllTokens(text)
                .SelectValid()
                .GroupBy(x => x.Token.ToString())
                .Select(x => x.ToSegmentsCollection())
                .ToList()
                .ForEachPairs((f, s) => parser.IgnoreSegmentsThatDoNotMatchRules(f, s));
            
            var actualText = parser.ReplaceTokens(text, SegmentsCollection.Union(segments), translator);

            actualText.Should().Be(expected);
        }

        [TestCase("-a", true, false)]
        [TestCase("a-", false, true)]
        [TestCase("-", false, false)]
        [TestCase("a-a", true, true)]
        [TestCase("a -a", true, false)]
        [TestCase("a- a", false, true)]
        [TestCase("a-- a", false, true)]
        [TestCase("a --a", true, false)]
        public void FindAllTokens_Validation_Test(string text, bool expectedValidToStart, bool expectedValidToClose)
        {
            var actualToken = parser
                .FindAllTokens(text)
                .GetTokenInfos()
                .First();

            var actualValidToStart = actualToken.OpenValid;
            var actualValidToClose = actualToken.CloseValid;

            actualValidToStart.Should().Be(expectedValidToStart);
            actualValidToClose.Should().Be(expectedValidToClose);
        }
        
        [TestCase("abc")]
        [TestCase("a-a", 1)]
        [TestCase("a-a-a", 1, 3)]
        [TestCase("-a", 0)]
        [TestCase("a-", 1)]
        [TestCase("a--a", 1)]
        [TestCase("a---a", 1, 3)]
        [TestCase("a----a", 1, 3)]
        [TestCase("a--a--a", 1, 4)]
        [TestCase("a__a-_<a", 1, 4, 5)]
        [TestCase("a__", 1)]
        [TestCase("__a", 0)]
        [TestCase("a_<", 1)]
        [TestCase("_<a", 0)]
        
        [TestCase("a---bb---a", 1, 3, 6, 7)]
        [TestCase("a---bb--a", 1, 3, 6, 7)]
        [TestCase("a---bb-a", 1, 3, 6)]
        public void FindAllTokens_Location_Test(string text, params int[] expectedIndexes)
        {
            var actualIndexes = parser
                .FindAllTokens(text)
                .GetTokenInfos()
                .Select(x => x.Position)
                .OrderBy(x => x)
                .ToArray();

            actualIndexes
                .Should()
                .Equal(expectedIndexes
                    .OrderBy(x => x)
                    .ToArray()
                );
        }
    }
}