using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Markdown;
using Markdown.Nodes;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkingToNodeParser_Should
    {
        private MarkingToNodeParser parser = new MarkingToNodeParser();
        
        [TestCase(' ', "", ' ')]
        [TestCase(' ', "#", ' ', 
            TestName = "Some symbol before new paragraph tag")]
        [TestCase(null, "#", 'a', 
            TestName = "Not space after new paragraph tag")]
        [TestCase('a', "_", ' ', 
            TestName = "Space after emphasized tag")]
        [TestCase('a', "__", ' ', 
            TestName = "Space after strong tag")]
        public void ReturnNothingWhen(char? symbolBeforeMarking, string markingText, char? symbolAfterMarking)
        {
            var initialMarking = new Marking(symbolBeforeMarking, markingText, symbolAfterMarking);
            var result = parser.TryGetOpenedNode(initialMarking, out var node, out var trimmed);

            using (new AssertionScope())
            {
                result.Should().BeFalse();
                node.Should().BeNull();
                trimmed.Should().BeNull();
            }
        }
        
        [TestCase(null, "_", 'a', typeof(EmphasizedTaggedNode),
            TestName = "When <em> given")]
        [TestCase(null, "__", 'a', typeof(StrongTaggedNode),
            TestName = "When <strong> given")]
        [TestCase(null, "#", ' ', typeof(FirstHeaderTaggedNode),
            TestName = "When <h1> given")]
        public void ReturnNotNullWhen(char? symbolBeforeMarking, string markingText, char? symbolAfterMarking, 
            Type expectedType)
        {
            var initialMarking = new Marking(symbolBeforeMarking, markingText, symbolAfterMarking);
            var result = parser.TryGetOpenedNode(initialMarking, out var node, out var trimmed);

            using (new AssertionScope())
            {
                result.Should().BeTrue();
                node.Should().NotBeNull();
                node.Should().BeOfType(expectedType);
                trimmed.Should().NotBeNull();
            }
        }
    }
}