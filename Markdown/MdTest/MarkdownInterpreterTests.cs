using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown;
using Markdown.Ecxeptions;
using NUnit.Framework;

namespace MdTest
{
    internal class UnknownNode : IElement
    {
        public IElement Child { get; set; }
    }

    [TestFixture]
    public class MarkdownInterpreterTests
    {
        [Test]
        public void Execute_ShouldThrowUnknownElementException_ThenParseReturnUnknownElement()
        {
            var unknownNode = new UnknownNode();
            var inter = new MarkdownInterpreter(unknownNode);
            Action testingAct = () => inter.GetHtmlCode();
            testingAct.Should().Throw<UnknownElementException>();
        }
    }
}
