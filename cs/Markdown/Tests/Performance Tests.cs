using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tests
{
    [TestFixture]
    public class PerfomanceTests
    {
        private string boldMdText;
        private string italicMdText;
        private Md mdRender;

        [SetUp]
        public void SetUp()
        {
            boldMdText = CreateMdText("__");
            italicMdText = CreateMdText("_");
            mdRender = new Md();
        }

        private string CreateMdText(string formattingCharacter, int iteration = 100000)
        {
            var random = new Random();
            var text = new StringBuilder();
            for (int i=0;i<iteration;i++)
            {
                text.Append(formattingCharacter);
                text.Append((char)random.Next(0x0410, 0x44F));
                text.Append(formattingCharacter);
            }
            return text.ToString();
        }

        [Test, MaxTime(1000)]
        public void ItalicParserTest()
        {
            mdRender.Render(italicMdText);
        }

        [Test, MaxTime(1000)]
        public void BoldParserTest()
        {
            mdRender.Render(boldMdText);
        }
    }
}
