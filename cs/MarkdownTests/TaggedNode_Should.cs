using System;
using FluentAssertions;
using Markdown;
using Markdown.Nodes;
using NUnit.Framework;

namespace MarkdownTests
{
    //Нормально ли, что тесты в этом классе проверяют логику лишь одного наследника класса TaggedNode?
    public class TaggedNode_Should
    {
        [Test]
        public void NotBeAbleToCloseOnce()
        {
            var marking = new Marking(null, "__", null);
            var node = new EmphasizedTaggedNode();
            Action closingAction = () => node.Close(marking, out marking);
            closingAction.Should().NotThrow();
        }

        [Test]
        public void NotBeAbleToCloseTwice()
        {
            var marking = new Marking(null, "__", null);
            var node = new EmphasizedTaggedNode();
            node.Close(marking, out marking);
            Action secondClosingAction = () => node.Close(marking, out marking);
            secondClosingAction.Should().Throw<Exception>();
        }

        [Test]
        public void ReturnChangedMarkingAfterClosing()
        {
            var initialMarking = new Marking(null, "__", null);
            var node = new EmphasizedTaggedNode();
            node.Close(initialMarking, out var newMarking);

            newMarking.Should().BeEquivalentTo(initialMarking,
                    config => config.Excluding(x => x.StringMarking))
                .And
                .NotBeEquivalentTo(initialMarking,
                    config => config.Including(x => x.StringMarking));
        }
        
        [Test]
        public void BuildValueWithHtmlTag_WhenClosed()
        {
            var marking = new Marking(null, "_", null);
            var node = new EmphasizedTaggedNode();
            var expected = "\\<em>hello world\\</em>";
            node.AddNode(new StringNode("hello world"));
            
            node.Close(marking, out marking);

            var result = node.GetNodeBuilder().ToString();
            result.Should().Be(expected);
        }
        
        [Test]
        public void BuildValueWithMarkdownTag_WhenWasNotClosed()
        {
            var node = new EmphasizedTaggedNode();
            var expected = "_hello world";
            node.AddNode(new StringNode("hello world"));

            var result = node.GetNodeBuilder().ToString();
            result.Should().Be(expected);
        }

        [Test]
        public void CombineNodesWhileBuilding()
        {
            var node = new EmphasizedTaggedNode();
            var otherNodes = new StringNode[]
            {
                new("hello"),
                new(" "),
                new("world")
            };
            var expected = "_hello world";

            foreach (var otherNode in otherNodes)
            {
                node.AddNode(otherNode);
            }

            var builtString = node.GetNodeBuilder().ToString();

            builtString.Should().Be(expected);
        }
    }
}