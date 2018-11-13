using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;


namespace Markdown
{
    class Md
    {
        public Md()
        {
        }

        public string Render(string markDownInput)
        {
            //Сейчас написана реализация только для одного нижнего подчеркивания, 
            //Планируется расширить для произвольных разделителей
            var converter = new MdToHTMLConverter();
            var result = converter.Convert(markDownInput);
            return result;
        }
    }

    [TestFixture]
    public class Md_Tests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [Test]
        public void Render_ShouldReturnEmptyString_OnEmptyInput()
        {
            md.Render("").Should().BeEmpty();
        }

        [Test]
        public void Render_ShouldConvert_WhenSingleToken()
        {
            md.Render("_text_").Should().Be("<em>text</em>");
        }

        [Test]
        public void Render_ShouldConvert_WhenTwoTokens()
        {
            md.Render("_tex_ _t_").Should().Be("<em>tex</em> <em>t</em>");
        }

        [Test]
        public void Render_ShouldConvert_When_SingleNonPairDelimiter()
        {
            md.Render("_tex _t_ _abc_").Should().Be("_tex <em>t</em> <em>abc</em>");
        }
        [Test]
        public void Render_ShouldConvert_When_SingleNonPairDelimiter2()
        {
            md.Render("tex _t_ _abc_ _t").Should().Be("tex <em>t</em> <em>abc</em> _t");
        }

        [Test]
        public void Render_ShouldConvert_When_Smth()
        {
            md.Render("_abc_ _def_ xyz").Should().Be("<em>abc</em> <em>def</em> xyz");
        }
    }
}