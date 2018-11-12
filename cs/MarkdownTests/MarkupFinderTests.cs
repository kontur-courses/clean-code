using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    [TestClass]
    public class MarkupFinderTests
    {
        [TestMethod]
        public void FindSimpleUnderscore()
        {
            var simpleUnderscore = new Markup("simpleUnderscore", "_", "em");
            var markups = new List<Markup> { simpleUnderscore };

            var markupFinder = new MarkupFinder(markups);

            var paragraph = "_ff_";
            var markupsWithPositions = markupFinder.GetMarkupsWithPositions(paragraph);

            markupsWithPositions[simpleUnderscore].First().ShouldBeEquivalentTo(new MarkupPosition(0, 3));
        }
        [TestMethod]
        public void FindDoubleUnderscore()
        {
            var doubleUnderscore = new Markup("doubleUnderscore", "__", "strong");
            var markups = new List<Markup> { doubleUnderscore };

            var markupFinder = new MarkupFinder(markups);

            var paragraph = "__f__";
            var markupsWithPositions = markupFinder.GetMarkupsWithPositions(paragraph);

            markupsWithPositions[doubleUnderscore].First().ShouldBeEquivalentTo(new MarkupPosition(0, 3));
        }

        [TestMethod]
        public void FindMultipleSimpleUnderscore()
        {
            var simpleUnderscore = new Markup("simpleUnderscore", "_", "em");
            var markups = new List<Markup> { simpleUnderscore };

            var markupFinder = new MarkupFinder(markups);

            var paragraph = "_f _f _f_ f_ f_";
            var markupsWithPositions = markupFinder.GetMarkupsWithPositions(paragraph);

            markupsWithPositions[simpleUnderscore].ShouldBeEquivalentTo(
            new List<MarkupPosition>(){
                new MarkupPosition(0, 14),
                new MarkupPosition(3, 11),
                new MarkupPosition(6, 8)
            });
        }

        [TestMethod]
        public void FindMultipleSimpleUnderscore2()
        {
            var simpleUnderscore = new Markup("simpleUnderscore", "_", "em");
            var markups = new List<Markup> { simpleUnderscore };

            var markupFinder = new MarkupFinder(markups);

            var paragraph = "_f _f_ _f_ f_";
            var markupsWithPositions = markupFinder.GetMarkupsWithPositions(paragraph);

            markupsWithPositions[simpleUnderscore].ShouldBeEquivalentTo(
                new List<MarkupPosition>(){
                    new MarkupPosition(0, 12),
                    new MarkupPosition(3, 5),
                    new MarkupPosition(7, 9)
                });
        }
    }
}
