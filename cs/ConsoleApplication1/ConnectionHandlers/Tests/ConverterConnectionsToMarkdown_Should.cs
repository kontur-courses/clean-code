using System;
using System.Linq;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.ConnectionHandlers.DataHandlers;
using ConsoleApplication1.UsefulEnums;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.ConnectionHandlers.Tests
{
    [TestFixture]
    public class ConverterConnectionsToMarkdown_Should
    {
        private readonly ConverterConnectionsToMarkdown converter = new ConverterConnectionsToMarkdown();

        [TestCase(1, MarkdownConnectionType.Single, TestName = "Strength is one")]
        [TestCase(2, MarkdownConnectionType.Double, TestName = "Strength is two")]
        [TestCase(3, MarkdownConnectionType.SingleAndDouble, TestName = "Strength is three")]
        [TestCase(1000000, MarkdownConnectionType.SingleAndDouble, TestName = "Strength is quiet big")]
        public void Convert_ReturnsCorrectConnectionType_WhenGetsCurrectStrengthConnection(int connectionStrength,
            MarkdownConnectionType exptectedType)
        {
            var givenConnection = new Connection(0, 1, connectionStrength);
            converter.Convert(givenConnection);
        }

        [Test, Timeout(1000)]
        public void Convert_WorksFast_WhenExecutesManyTimes()
        {
            var iterationCount = 100000;

            for (var index = 0; index < iterationCount; index++)
                converter.Convert(new Connection(index, index + 1, index + 1));
        }

        [Test]
        public void Convert_ReturnsConnectionsWithSameTypes_WhenConnectionsHaveOnlyDifferentIndexes()
        {
            var strength = 100;
            var maxIndex = 1000;
            var mdConnections = Enumerable
                .Range(0, maxIndex)
                .SelectMany(x => Enumerable.Range(0, maxIndex).Select(y => Tuple.Create(x, y)))
                .Where(x => x.Item1 != x.Item2)
                .Select(x => new Connection(x.Item1, x.Item2, strength))
                .Select(x => converter.Convert(x));

            mdConnections.Should().OnlyContain(x => x.ConnectionType == MarkdownConnectionType.SingleAndDouble);
        }
    }
}
