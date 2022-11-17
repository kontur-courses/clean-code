using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parsers.Tags;
using Markdown.Parsers.Tags.Enum;
using Markdown.Parsers.Tags.Html;
using Markdown.Parsers.Tags.Markdown;
using NUnit.Framework;


namespace Markdown
{

    public class Tests
    {
        [Test]
        public void ConvertMarkdownToHtml()
        {
            var a = new List<ITag>
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
