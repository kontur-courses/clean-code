using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Markdown.Handlers;
using Markdown.Parsers;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TokenTreeCreator_Test
    {
        private TokenTreeCreator sut;

        [OneTimeSetUp]
        public void StartTests()
        {
            sut = new TokenTreeCreator(
                new BoldTagMdParser(),
                new ItalicTagMdParser(),
                new HeadingMdParser());
        }
        
        [Test]
        public void GetRootToken_ShouldReturnOneTokenWithoutChildren_WhenNoTags()
        {
            const string text = "text without tags";
            var rootToken = sut.GetRootToken(text);
            rootToken.Should().NotBeNull();
            rootToken.Length.Should().Be(text.Length);
            rootToken.Position.Should().Be(0);
            rootToken.Type.Should().Be(TextType.Default);
            rootToken.GetInternalTokens().Count.Should().Be(0);
        }

        [TestCase("#text")]
        [TestCase("\\_text\\_")]
        [TestCase("_text\\_")]
        [TestCase("1_23_4")]
        [TestCase("*text*")]
        [TestCase("te_xt")]
        [TestCase("text text")]
        public void GetRootToken_ShouldReturnOneTokenWithoutChildren_WhenIncorrectTagUsing(string text)
        {
            var rootToken = sut.GetRootToken(text);
            rootToken.Should().NotBeNull();
            rootToken.Length.Should().Be(text.Length);
            rootToken.Position.Should().Be(0);
            rootToken.Type.Should().Be(TextType.Default);
            rootToken.GetInternalTokens().Count.Should().Be(0);
        }

        [Test]
        public void GetRootToken_ShouldReturnTokenWithCorrectChildren_WhenTextHasTags()
        {
            const string text = "#This is text with __bold _italic_ text__\n" +
                                "Default text and __bold__ highlighting.\n" +
                                "And some it_ali_c :)";
            var rootToken = sut.GetRootToken(text);
            var rootInternalTokens = rootToken.GetInternalTokens();
            rootInternalTokens.Count.Should().Be(3);
            rootInternalTokens
                .Select(token => token.GetInternalTokens().Count)
                .Max()
                .Should().Be(1);
        }
    }

    public static class TokenExtensions
    {
        public static List<Token> GetInternalTokens(this Token token)
        {
            var fieldInfo = typeof (Token).GetField("internalTokens",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null) return null;
            return (List<Token>)fieldInfo.GetValue(token);
        }
    }
}