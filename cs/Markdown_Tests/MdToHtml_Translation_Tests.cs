using System;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    public class MdToHtml_Translation_Tests
    {
        [Test]
        public void Test()
        {
            var text = "Текст, _окруженный с двух сторон_ одинарными символами подчерка, должен помещаться в HTML-тег <em>";
            Console.WriteLine(Md.TranslateToHtml(text));
        }
        
        [TestCase("Текст, _окруженный с двух сторон_ одинарными символами подчерка, должен помещаться в HTML-тег <em>",
            "<p>Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка, должен помещаться в HTML-тег <em></p>")]
        [TestCase("__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега <strong>",
            "<p><strong>Выделенный двумя символами текст</strong> должен становиться полужирным с помощью тега <strong></p>")]
        [TestCase("# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
        public void Render_Test(string input, string expectedRender)
        {
            var actualRender = Md.TranslateToHtml(input);
            Assert.AreEqual(expectedRender, actualRender);
        }
    }
}