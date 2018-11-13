using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkupFinderTests
    {
        [TestCase("__f _d_ f__", new[] { 4, 6 }, new[] { 0, 9 }, TestName = "Should find simple and double markup")]
        public void FindDoubleAndSimple(string paragraph, int[] simpleUnderscorePositions,
            int[] doubleUnderscorePositions)
        {
            var expectedDoubleUnderscorePositions = new List<MarkupPosition>();
            var expectedSimpleUnderscorePositions = new List<MarkupPosition>();
            for (var i = 0; i < simpleUnderscorePositions.Length; i += 2)
                expectedSimpleUnderscorePositions.Add(new MarkupPosition(simpleUnderscorePositions[i],
                    simpleUnderscorePositions[i + 1]));
            for (var i = 0; i < doubleUnderscorePositions.Length; i += 2)
                expectedDoubleUnderscorePositions.Add(new MarkupPosition(doubleUnderscorePositions[i],
                    doubleUnderscorePositions[i + 1]));

            var doubleUnderscore = new Markup("doubleUnderscore", "__", "strong");
            var simpleUnderscore = new Markup("simpleUnderscore", "_", "em");
            var markups = new List<Markup> { doubleUnderscore, simpleUnderscore };

            var markupFinder = new MarkupFinder(markups);
            var markupsWithPositions = markupFinder.GetMarkupsWithPositions(paragraph);

            markupsWithPositions[simpleUnderscore].ShouldBeEquivalentTo(expectedSimpleUnderscorePositions);
            markupsWithPositions[doubleUnderscore].ShouldBeEquivalentTo(expectedDoubleUnderscorePositions);
        }

        [Test]
        public void ShouldFindDoubleUnderscore()
        {
            var doubleUnderscore = new Markup("doubleUnderscore", "__", "strong");
            var markups = new List<Markup> { doubleUnderscore };

            var markupFinder = new MarkupFinder(markups);

            var paragraph = "__f__";
            var markupsWithPositions = markupFinder.GetMarkupsWithPositions(paragraph);

            markupsWithPositions[doubleUnderscore].First().ShouldBeEquivalentTo(new MarkupPosition(0, 3));
        }

        [TestCase("_ff\\_", TestName = "Should not find finishing markup with screening")]
        [TestCase("\\_ff_", TestName = "Should not find starting markup with screening")]
        [TestCase("f_", TestName = "Should not find markup without starting markup")]
        [TestCase("_f", TestName = "Should not find markup without finishing markup")]
        public void FindSimpleUnderscore(string paragraph)
        {
            var simpleUnderscore = new Markup("simpleUnderscore", "_", "em");
            var markups = new List<Markup> { simpleUnderscore };

            var markupFinder = new MarkupFinder(markups);
            var markupsWithPositions = markupFinder.GetMarkupsWithPositions(paragraph);

            markupsWithPositions.Should().NotContainKey(simpleUnderscore);
        }

        [TestCase("_ff_", new[] { 0, 3 }, TestName = "Should find simple markup")]
        [TestCase("_f _f_ _f_ f_", new[] { 0, 12, 3, 5, 7, 9 }, TestName = "Should find multiple markup on one nesting level")]
        [TestCase("_f _f _f_ f_ f_", new[] { 0, 14, 3, 11, 6, 8 }, TestName = "Should find multiple nesting")]
        public void FindSimpleUnderscore(string paragraph, int[] positions)
        {
            var listOfExpectedPositions = new List<MarkupPosition>();
            for (var i = 0; i < positions.Length; i += 2)
                listOfExpectedPositions.Add(new MarkupPosition(positions[i], positions[i + 1]));

            var simpleUnderscore = new Markup("simpleUnderscore", "_", "em");
            var markups = new List<Markup> { simpleUnderscore };

            var markupFinder = new MarkupFinder(markups);
            var markupsWithPositions = markupFinder.GetMarkupsWithPositions(paragraph);

            markupsWithPositions[simpleUnderscore].ShouldBeEquivalentTo(listOfExpectedPositions);
        }
    }
}