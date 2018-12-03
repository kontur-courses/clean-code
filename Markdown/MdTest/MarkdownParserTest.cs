using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown;
using Markdown.Ecxeptions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MdTest
{
    [TestFixture]
    public class MarkdownParserTest
    {
        [Test]
        public void Parse_ShouldThrowUnknownTokenException_ThenLexerTokenizeReturnUnknownTokens()
        {
            var parser = new MarkdownParser(new TestLexer());
            Action testingAct = () => parser.Parse();
            testingAct.Should().Throw<UnknownTokenException>();
        }
    }
}
