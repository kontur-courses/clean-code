using System.Collections.Generic;
using FluentAssertions;
using Markdown.Readers;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class LinkReaderWithTextReaderInside_Should : Reader_Should<LinkReader>
    {
        private ReadingOptions options;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            defaultReader = new LinkReader();
            var textReader = new TextReader("]");
            options = new ReadingOptions(
                new List<AbstractReader> { textReader },
                new Dictionary<AbstractReader, HashSet<AbstractReader>>
                {
                    [defaultReader] = new HashSet<AbstractReader> { textReader }
                }
            );
        }

        [TestCase("desc", "http://domain.do/link.html", TestName = "link is full")]
        [TestCase("desc", "my_short_link", TestName = "link without protocol and domain")]
        [TestCase("desc", "mysite.my/my_short_link", TestName = "link without protocol and domain")]
        [TestCase("desc", "/my_short_link", TestName = "link starts with slash")]
        [TestCase("desc", "mysite.my:8080/my_short_link", TestName = "link contains port")]
        [TestCase("a bb ccc\td", "link", TestName = "description contains whitespaces")]
        public void ReadLinkTokenWithTextDescription_When(string description, string link)
        {
            var expectedToken = new LinkToken(new List<IToken>{new TextToken(description)}, link);

            var (token, _) = defaultReader.ReadToken($"[{description}]({link})", 0, options);

            token.Should().Be(expectedToken);
        }

        [TestCase("[desc](link", TestName = "token without closing symbols")]
        [TestCase("[desc] (link)", TestName = "any symbols between description and link")]
        public void NotReadLinkToken_When(string text)
        {
            var (token, length) = defaultReader.ReadToken(text, 0, options);

            token.Should().BeNull();
            length.Should().Be(0);
        }
    }
}