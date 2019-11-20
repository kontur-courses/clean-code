using FluentAssertions;
using Markdown.Languages;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MarkdownSpec
    {
        [TestCase("Текст _окруженный с двух сторон_  одинарными символами", ExpectedResult = "Текст <em>окруженный с двух сторон</em>  одинарными символами")]
        [TestCase("__Двумя символами__", ExpectedResult = "<strong>Двумя символами</strong>")]
        [TestCase("_12_3", ExpectedResult = "_12_3")]
        [TestCase("__непарные _символы не считаются выделением.", ExpectedResult = "__непарные _символы не считаются выделением.")]
        [TestCase("эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.", ExpectedResult = "эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.")]
        [TestCase("эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка.", ExpectedResult = "эти <em>подчерки _не считаются</em> окончанием выделения и остаются просто символами подчерка.")]
        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает.", ExpectedResult = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.")]
        [TestCase("Но не наоборот — внутри _одинарного __двойное__ не работает_.", ExpectedResult = "Но не наоборот — внутри <em>одинарного __двойное__ не работает</em>.")]
        [TestCase(@"\_Вот это\_, не должно выделиться тегом \<em\>.", ExpectedResult = @"\_Вот это\_, не должно выделиться тегом \<em\>.")]
        [TestCase(@"_Вот это_, должно выделиться тегом \<em\>.", ExpectedResult = @"<em>Вот это</em>, должно выделиться тегом \<em\>.")]
        public string DoSomething_WhenSomething(string str)
        {
            return TreeBuilder.RenderTree<MarkDown>(str).ConvertTo<Html>();
        }
    }
}