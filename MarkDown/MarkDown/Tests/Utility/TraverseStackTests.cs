using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MarkDown.Tests.Utility
{
    [TestFixture]
    public class TraverseStackTests
    {
        [TestCase(1, TestName = "On one element")]
        [TestCase(1, 2, 3, TestName = "On several elements")]
        [TestCase(616, 2134, 6126, 13)]
        public void PushAndPop_ReturnsCorrectElementsInCorrectOrder(params int[] elements)
        {
            var stack = new TraverseStack<int>();
            foreach (var element in elements)
                stack.Push(element);
            foreach (var element in elements.Reverse())
                stack.Pop().Should().Be(element);
        }

        
        [TestCase(1, 2, 3, 4, 5, TestName = "Regular test")]
        [TestCase(1, TestName = "On one element")]
        [TestCase(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        public void TraverseToElementAndReturnBack_ReturnsCorrectSequence(params int[] elements)
        {
            var stack = new TraverseStack<int>();
            foreach (var element in elements)
                stack.Push(element);
            var traverseResults = elements
                .Select(element => stack.TraverseToElementAndReturnBack(element).ToList())
                .ToList();
            for (var i = 0; i < traverseResults.Count; i++)
            {
                var explicitTraverse = new List<int>();
                var index = elements.Length - 1;
                while (index >= 0)
                {
                    explicitTraverse.Add(elements[index]);
                    if (elements[index] == elements[i])
                        break;
                    index--;
                }
                for(index = index + 1;index < elements.Length; index++)
                    explicitTraverse.Add(elements[index]);
                traverseResults[i].Should().BeEquivalentTo(explicitTraverse);
            }
        }

        [Test]
        public void Push_ThrowsArgumentException_OnEqualElements()
        {
            var stack = new TraverseStack<int>();
            stack.Push(5);
            Action action = () => stack.Push(5);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Pop_ThrowsIndexOutOfRangeException_OnZeroSize()
        {
            var stack = new TraverseStack<int>();
            Action action = () => stack.Pop();
            action.Should().Throw<IndexOutOfRangeException>();
        }


        [TestCase(1, true, 1, 2, 3, 4, 5)]
        [TestCase(15, false, 1, 7, 14, 16)]
        public void Contains_ReturnsCorrectValues(int numberToCheck, bool expectedValue, params int[] numbers)
        {
            var stack = new TraverseStack<int>();
            foreach (var number in numbers)
                stack.Push(number);
            stack.Contains(numberToCheck).Should().Be(expectedValue);
        }

        [Test]
        public void Remove_ThrowsArgumentException_OnInvalidElement()
        {
            var stack = new TraverseStack<int>();
            stack.Push(5);
            stack.Push(124);
            Action action = () => stack.Remove(1);
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void TraverseToElementAndReturnBack_ThrowsArgumentException_OnInvalidElement()
        {
            var stack = new TraverseStack<int>();
            stack.Push(5);
            stack.Push(124);
            Action action = () => stack.TraverseToElementAndReturnBack(1).ToList();
            action.Should().Throw<ArgumentException>();
        }

        [TestCase(5, 1, 2, 3, 4, 5, 6, 7, 8)]
        public void Remove_DeletesElementsCorrectly(int amountOfDeletions, params int[] elements)
        {
            var random = new Random();
            var stack = new TraverseStack<int>();
            foreach (var t in elements)
                stack.Push(t);
            var reversedElements = elements.Reverse().ToList();
            for (var i = 0; i < amountOfDeletions; i++)
            {
                var randomValue = reversedElements[random.Next(reversedElements.Count)];
                reversedElements.Remove(randomValue);
                stack.Remove(randomValue);
                var stackAsList = stack.ToList();
                stackAsList.Should().BeEquivalentTo(reversedElements);
            }
        }
    }
}