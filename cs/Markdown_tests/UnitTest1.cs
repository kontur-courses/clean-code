using NUnit.Framework;
using Markdown;
using Microsoft.VisualBasic;
using FluentAssertions;

namespace Markdown_tests
{
    [TestFixture]
    public class Tests
    {
        Md markdown = new Md();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var parser = new Parser("Текст, _окруженный с двух сторон_ одинарными символами подчерка");
            var concs = parser.Parse();

            var expected = @"Текст, \<em>окруженный с двух сторон\</em> одинарными символами подчерка,
должен помещаться в HTML-тег \<em>.";
            
        }

        [Test]
        public void Test2()
        {
            var parser = new Parser(@"__Непарные_ символы в рамках одного абзаца не считаются выделением.");
            var concs = parser.Parse();

            

        }
    }
}