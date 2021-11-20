using System;
using Markdown.Models;
using NUnit.Framework;

namespace MarkdownTests
{
    public class ContextTests
    {
        [Test]
        public void Constructor_ThrowsException_TextNull()
        {
            Assert.Throws<ArgumentException>(() =>
                new Context(null));
        }
    }
}