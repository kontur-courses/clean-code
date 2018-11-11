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
            return string.Empty;
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
    }
}