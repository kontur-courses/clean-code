using NUnit;
using Markdown;
using Xunit;
using FluentAssertions;
using Markdown.Parsing;

namespace MdTests
{
    public class MDTests
    {
        private readonly Md _md;

        public MDTests()
        {
            _md = new Md();
        }

        [Test]
        public void Parse_EmptyInput_ReturnsEmptyString()
        {
            _md.Parse("").Should().BeEmpty();
        }


        [Test]
        public void Parse_PlainText_ReturnsAsIs()
        {
            _md.Parse("Просто текст").Should().Be("Просто текст");
        }


        [Test]
        public void Parse_BoldText_ReturnsStrongTag()
        {
            _md.Parse("__жирный__").Should().Be("<strong>жирный</strong>");
        }

        [Test]
        public void Parse_ItalicText_ReturnsEmTag()
        {
            _md.Parse("_курсив_").Should().Be("<em>курсив</em>");
        }

        [Test]
        public void Parse_CombinedBoldAndItalic_ReturnsNestedTags()
        {
            _md.Parse("__жирный и _курсив_ и еще текст__").Should().Be("<strong>жирный и <em>курсив</em> и еще текст</strong>");
        }

        [Test]
        public void Parse_TripleUnderscore_ReturnsStrongTag()
        {
            _md.Parse("___текст___").Should().Be("<strong><em>текст</em></strong>");
        }

        [Test]
        public void Parse_Header_ReturnsH1Tag()
        {
            _md.Parse("# Заголовок").Should().Be("<h1>Заголовок</h1>");
        }


    }
}