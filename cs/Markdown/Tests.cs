using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parsers.Tokens;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Markdown;
using NUnit.Framework;


namespace Markdown
{

    public class Tests
    {
        [Test]
        public void ConvertMarkdownToHtml()
        {
            var a = new List<IToken>
            {
                new MdItalicTag(TagPosition.Start),
                new MdTextTag("abc"),
                new MdItalicTag(TagPosition.End)
            };
            var b = a.ConvertAll(tag => tag.ToHtml());
            Assert.AreEqual("<em>", b.First().ToString());
            Assert.AreEqual("abc", b.Skip(1).First().ToString());
            Assert.AreEqual("</em>", b.Skip(2).First().ToString());
        }
    }
}
