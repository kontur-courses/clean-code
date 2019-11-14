using System;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    class SeparatorSearchTool_Should
    {
        [Test]
        public void GetSeparators_ArgumentNullException_WhenInputStringIsNull()
        {
            Action act = () => SeparatorSearchTool.GetSeparators(null, new string[] { "_" }.ToList());
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetSeparators_ReturnEmptyDictionary_WhenInputSeparatorsTagsCountIsZero()
        {
            var foundSeparators = SeparatorSearchTool.GetSeparators("_a_", new List<string>());

            foundSeparators.Count.Should().Be(0);
        }

        [TestCase("_a", new string[] { "_" })]
        [TestCase("__a", new string[] { "__" })]
        [TestCase("_a __a", new string[] { "_", "__" })]
        [TestCase("__a _a", new string[] { "_", "__" })]
        public void GetSeparators_AbleToFind_OpeningSeparators(string input, string[] separatorsTags)
        {
            var foundSeparators = SeparatorSearchTool.GetSeparators(input, separatorsTags.ToList());

            foreach (var separatorTag in separatorsTags)
            {
                var separator = foundSeparators[separatorTag].First();
                separator.Type.Should().Be(SeparatorType.Opening);
            }
        }

        [TestCase("a_", new string[] { "_" })]
        [TestCase("a__", new string[] { "__" })]
        [TestCase("a_ a__", new string[] { "_", "__" })]
        [TestCase("a__ a_", new string[] { "_", "__" })]
        public void GetSeparators_AbleToFind_ClosingSeparators(string input, string[] separatorsTags)
        {
            var foundSeparators = SeparatorSearchTool.GetSeparators(input, separatorsTags.ToList());

            foreach (var separatorTag in separatorsTags)
            {
                var separator = foundSeparators[separatorTag].First();
                separator.Type.Should().Be(SeparatorType.Closing);
            }
        }

        [TestCase("_aa_ __aa__", new string[] { "_", "__" })]
        [TestCase("_aa__ __aa_", new string[] { "_", "__" })]
        public void GetSeparators_MustDistinguish_SeparatorsDifferentLengths(string input, string[] separatorsTags)
        {
            var foundSeparators = SeparatorSearchTool.GetSeparators(input, separatorsTags.ToList());

            foreach (var separatorTag in separatorsTags)
                foreach (var separator in foundSeparators[separatorTag])
                    separator.Tag.Should().Be(separatorTag);
        }

        [TestCase("_a", new string[] { "_", "__" }, 1)]
        [TestCase("__a", new string[] { "_", "__" }, 1)]
        [TestCase("_a__", new string[] { "_", "__" }, 2)]
        [TestCase("_a__ __a_", new string[] { "_", "__" }, 4)]
        [TestCase("___a___", new string[] { "_", "__" }, 0)]
        [TestCase("_a _a _a _a", new string[] { "_", "__" }, 4)]
        public void GetSeparators_MustFind_AllSeparatorsInString(string input, string[] separatorsTags, int countAllSeparatorsInString)
        {
            var foundSeparators = SeparatorSearchTool.GetSeparators(input, separatorsTags.ToList());

            foundSeparators.Values.SelectMany(x => x).Count().Should().Be(countAllSeparatorsInString);
        }
    }
}
