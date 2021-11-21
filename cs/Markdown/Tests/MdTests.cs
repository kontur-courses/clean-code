using System;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdTests
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase("#Это тестовая строка", ExpectedResult = "<h1>Это тестовая строка</h1>", TestName = "Render_ShouldParseH1")]
        [TestCase("Это _тестовая_ строка", ExpectedResult = "Это <em>тестовая</em> строка", TestName = "Render_ShouldParseEm")]
        [TestCase("Это __тестовая__ строка", ExpectedResult = "Это <strong>тестовая</strong> строка", TestName = "Render_ShouldParseStrong")]
        [TestCase("#Это _тестовая_ __строка__", ExpectedResult = "<h1>Это <em>тестовая</em> <strong>строка</strong></h1>", TestName = "Render_ShouldParseAll")]
        public string Render_ShouldWorkCorrectly(string input)
        {
            return parser.Render(input);
        }

        [Test]
        public void Demo()
        {
            const string str = "Это *тестовая* *строка*";
            var tokenizer = new Tokenizer(new MdWrapSetting("*", "<em>", MdWrapType.Text));

            //foreach (var token in tokenizer.ParseTextTokens(str))
                //Console.WriteLine(token.Value);
        }
    }
}