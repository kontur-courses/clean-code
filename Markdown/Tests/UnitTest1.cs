using System.Security.Cryptography;
using NUnit.Framework;
using Markdown;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BasicCursive()
        {
            Md.Render("Text _with_ text")
                .Should().Be(@"Text <em>with<\em> text");
        }
    }
}