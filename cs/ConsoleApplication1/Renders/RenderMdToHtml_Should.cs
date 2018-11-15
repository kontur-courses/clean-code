using System;
using ConsoleApplication1.Directions;
using ConsoleApplication1.TextsConverter;
using ConsoleApplication1.UsefulEnums;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.Renders
{
    [TestFixture]
    public class RenderMdToHtml_Should
    {
        private RenderMdToHtml render;


        [SetUp]
        public void SetUp()
        {
            render = new RenderMdToHtml(new DetectorTextDirection(),
                 new ConverterMdSelectionsToHtmlData());
        }

        [Test]
        public void AddNextPart_ThrowsArgumentException_WhenAddedNewPartAfterLast()
        {
            render.AddNextPart(new TextPart("", TextType.End));
            Assert.Throws<ArgumentException>(() => render.AddNextPart(new TextPart("aa", TextType.SimpleText)));
        }

        [Test]
        public void GetTranslatedText_ThrowsArgumentException_WhenNotAddedLastPart()
        {
            render.AddNextPart(new TextPart("aaa", TextType.SimpleText));
            Assert.Throws<ArgumentException>(() => render.GetTranslatedText());
        }

        [Test]
        public void GetTranslatedText_WorksCorrectly_WhenThereAreNoSelections()
        {
            render.AddNextPart(new TextPart("aaaa", TextType.SimpleText));
            render.AddNextPart(new TextPart("__", TextType.SpecialSymbols));
            render.AddNextPart(new TextPart("", TextType.End));
            var expectedString = "aaaa__";
            render.GetTranslatedText()
                .Should()
                .Be(expectedString);
        }

        [Test]
        public void GetTranslatedText_ReturnsHtmlCode_WhenThereAreMarkdownSelections()
        {
            render.AddNextPart(new TextPart("__", TextType.SpecialSymbols));
            render.AddNextPart(new TextPart("aa", TextType.SimpleText));
            render.AddNextPart(new TextPart("__", TextType.SpecialSymbols));
            render.AddNextPart(new TextPart("", TextType.End));

            var expectedString = "<strong>aa</strong>";
            render.GetTranslatedText()
                .Should()
                .Be(expectedString);
        }

        [Test]
        public void CreationFails_WhenGetsNullInsteadOfDirectionChooser()
        {
            Assert.Throws<ArgumentException>(() => new RenderMdToHtml(null, new ConverterMdSelectionsToHtmlData()));
        }

        [Test]
        public void CreationFails_WhenGetsNullInsteadOfConverter()
        {
            Assert.Throws<ArgumentException>(() => new RenderMdToHtml(new DetectorTextDirection(), null));
        }
    }
}