using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    [TestFixture]
    class SeparatorCleanTool_Should
    {
        [Test]
        public void GetCorrectSeparators_Should_RemoveShieldedDelimiters()
        {
            var inputString = @"\_a\_";
            var separators = new Dictionary<string, List<Separator>>
            {
                ["_"] = new List<Separator>
                {
                    new Separator("_", 1, SeparatorType.Opening),
                    new Separator("_", 3, SeparatorType.Closing),
                }
            };

            var correctSeparators = SeparatorCleanTool.GetCorrectSeparators(inputString, separators);

            correctSeparators.Count.Should().Be(0);
        }

        [Test]
        public void GetCorrectSeparators_Should_RemoveAllUnclosedSeparators()
        {
            var inputString = @"_a_ _a";
            var separators = new Dictionary<string, List<Separator>>
            {
                ["_"] = new List<Separator>
                {
                    new Separator("_", 0, SeparatorType.Opening),
                    new Separator("_", 2, SeparatorType.Closing),
                    new Separator("_", 4, SeparatorType.Opening),
                }
            };

            var correctSeparators = SeparatorCleanTool.GetCorrectSeparators(inputString, separators);

            correctSeparators.Count.Should().Be(2);
            correctSeparators.First().Should().BeEquivalentTo(new Separator("_", 0, SeparatorType.Opening));
            correctSeparators.Last().Should().BeEquivalentTo(new Separator("_", 2, SeparatorType.Closing));
        }
    }
}
