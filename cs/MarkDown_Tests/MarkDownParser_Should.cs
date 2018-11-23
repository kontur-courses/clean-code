using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkDown;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests
{
    [TestFixture]
    public class MarkDownParser_Should
    {
        private List<TagType> availableTagTypes;
        private List<string> specCharacters;

        [SetUp]
        public void SetUp()
        {
            specCharacters = new List<string>(){"_", "__", "\\", "[", "]", "(", ")"}; 
            availableTagTypes = new List<TagType>(){new EmTag(), new StrongTag(), new ATag()};
        }

        [Test]
        public void GetTokens_FromFirstLayCorrectly_WhenMultipleLayers()
        {
            var parser = new MarkDownParser("__just _some_ text__".GetCharStates(specCharacters), availableTagTypes);
            var token = parser.GetTokens().First();
            token.Position.Should().Be(0);
            token.Length.Should().Be(20);
            token.TagType.GetType().Should().Be(typeof(StrongTag));
            token.TokenType.Should().Be(TokenType.Tag);
            string.Concat(token.Content.Select(s => s.Char)).Should().Be("just _some_ text");
        }

        [Test]
        public void GetTokens_FromSecondLayerCorrectly_WhenMultipleLayersAndTextTokenInStart()
        {
            var parser = new MarkDownParser("__just _some_ text__".GetCharStates(specCharacters), availableTagTypes);
            var tokens = parser.GetTokens().First().InnerTokens.ToList();
            tokens[0].Position.Should().Be(0);
            tokens[0].Length.Should().Be(5);
            tokens[0].TagType.Should().BeNull();
            tokens[0].TokenType.Should().Be(TokenType.Text);
            string.Concat(tokens[0].Content.Select(s => s.Char)).Should().Be("just ");
        }        
        
        [Test]
        public void GetTokens_FromSecondLayerCorrectly_WhenMultipleLayersAndTextTokenAtTheEnd()
        {
            var parser = new MarkDownParser("__just _some_ text__".GetCharStates(specCharacters), availableTagTypes);
            var tokens = parser.GetTokens().First().InnerTokens.ToList();
            tokens[2].Position.Should().Be(11);
            tokens[2].Length.Should().Be(5);
            tokens[2].TagType.Should().BeNull();
            tokens[2].TokenType.Should().Be(TokenType.Text);
            string.Concat(tokens[2].Content.Select(s => s.Char)).Should().Be(" text");
        }       
        
        [Test]
        public void GetTokens_FromSecondLayerCorrectly_WhenMultipleLayersAndInnerTagToken()
        {
            var parser = new MarkDownParser("__just _some_ text__".GetCharStates(specCharacters), availableTagTypes);
            var tokens = parser.GetTokens().First().InnerTokens.ToList();
            tokens[1].Position.Should().Be(5);
            tokens[1].Length.Should().Be(6);
            tokens[1].TagType.GetType().Should().Be(typeof(EmTag));
            tokens[1].TokenType.Should().Be(TokenType.Tag);
            string.Concat(tokens[1].Content.Select(s => s.Char)).Should().Be("some");
        }
    }
}
