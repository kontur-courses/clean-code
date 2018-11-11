using System;
using System.Collections.Generic;
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
            var simpleUnderscore = new Markup();
            var markups = new List<Markup>();

            var markupFinder = new MarkupFinder(markups);

            var paragraph = "_ff_";
            var markupsWithPositions = markupFinder.GetMarkupsWithPositions(paragraph);

            markupsWithPositions[simpleUnderscore][0].ShouldBeEquivalentTo(new MarkupPosition(0, 4));
        }
    }
}
