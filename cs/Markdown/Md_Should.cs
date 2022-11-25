using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        private Md md;
        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [Test]
        public void Render_ShouldTag_Italic()
        {
            var text = "_Одна земля_";
            var res = md.Render(text);

            res.Should().Be("<em>Одна земля</em>");
        }

        [Test]
        public void Render_ShouldTag_Strong()
        {
            var text = "__Две земли__";
            var res = md.Render(text);

            res.Should().Be("<strong>Две земли</strong>");
        }

        [Test]
        public void Render_ShouldShielding_Marks()
        {
            var text = @"\_Одна земля\_ \_\_Две земли\_\_";
            var res = md.Render(text);

            res.Should().Be("_Одна земля_ __Две земли__");
        }

        [Test]
        public void Render_ShouldShielding_ShieldSymbol()
        {
            var text = @"\\Два экранирования\\";
            var res = md.Render(text);

            res.Should().Be(@"\Два экранирования\");
        }

        [Test]
        public void Render_ShouldNotShielding_NotMarks()
        {
            var text = @"Экра\ниерование вн\утри\ те\кста";
            var res = md.Render(text);

            res.Should().Be(@"Экра\ниерование вн\утри\ те\кста");
        }

        [Test] public void Render_ShouldTag_ItalicInsideStrong()
        {
            var text = @"__Внутри двойного _одинарное_ работает.__";
            var res = md.Render(text);

            res.Should().Be(@"<strong>Внутри двойного <em>одинарное</em> работает.</strong>");
        }

        [Test]
        public void Render_ShouldNotTag_StrongInsideItalic()
        {
            var text = @"_Внутри одинарного __двойное__ не работает._";
            var res = md.Render(text);

            res.Should().Be(@"<em>Внутри одинарного __двойное__ не работает.</em>");
        }

        [Test]
        public void Render_ShouldNotTag_WhenOnlyNumbers()
        {
            var text = @"Текст с цифрами_12_3";
            var res = md.Render(text);

            res.Should().Be(@"Текст с цифрами_12_3");
        }

        [Test]
        public void Render_ShouldNotTag_WhenOnlyWhiteSpaces()
        {
            var text = @"__  __";
            var res = md.Render(text);

            res.Should().Be(@"__  __");
        }

        [Test]
        public void Render_ShouldNotCloseTag_WithWhiteSpaceBeforeEnd()
        {
            var text = @"_подчерк _не считается_";
            var res = md.Render(text);

            res.Should().Be(@"_подчерк <em>не считается</em>");
        }

        [Test]
        public void Render_ShouldNotOpenTag_WithWhiteSpaceAfterStart()
        {
            var text = @"подчерк_ _не считается_";
            var res = md.Render(text);

            res.Should().Be(@"подчерк_ <em>не считается</em>");
        }

        [Test]
        public void Render_ShouldNotTag_WhenTheyIntersects()
        {
            var text = @"_пересечение __не должно_ выделяться__";
            var res = md.Render(text);

            res.Should().Be(@"_пересечение __не должно_ выделяться__");
        }

        [Test]
        public void Render_ShouldNotTag_WhenNoPair()
        {
            var text = @"_Непарные символы";
            var res = md.Render(text);

            res.Should().Be(@"_Непарные символы");
        }

        [Test]
        public void Render_ShouldTag_PartOfWord()
        {
            var text = @"и в _нач_але, и в сер_еди_не, и в кон_це_.";
            var res = md.Render(text);

            res.Should().Be(@"и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це</em>.");
        }

        [Test]
        public void Render_ShouldNotTag_PartOfDifferentWords()
        {
            var text = @"в ра_зных сл_овах";
            var res = md.Render(text);

            res.Should().Be(@"в ра_зных сл_овах");
        }

        [Test]
        public void Render_ShouldTag_Heading()
        {
            var text = @"# Заголовок";
            var res = md.Render(text);

            res.Should().Be(@"<h1>Заголовок</h1>");
        }

        [Test]
        public void Render_ShouldTag_HeadingWithNewLine()
        {
            var text = @"# Заголовок
123123";
            var res = md.Render(text);

            res.Should().Be(@"<h1>Заголовок</h1>
123123");
        }

        [Test]
        public void Render_ShouldNotTag_HeadingWithCharsBeforeOpenMark()
        {
            var text = @"1231# Заголовок";
            var res = md.Render(text);

            res.Should().Be(@"1231# Заголовок");
        }

        [Test]
        public void Render_ShouldCorrectTag_WithMixedText()
        {
            var lines = new string[]
            {
                @"# Смешанный _текст_, __вроде _должно_ работать__ но _я __не__ уверен_",
                "возможны # неполадки. _неоконченный тег",
                "тут закончил_ __ __",
                @"\# парочка э\кранирований \_тегов_ \_\_фыв_ _часть\__1_",
            };

            var text = string.Join(Environment.NewLine, lines);
            var res = md.Render(text);

            res.Should().Be(
                string.Join(Environment.NewLine, new[] {
                @"<h1>Смешанный <em>текст</em>, <strong>вроде <em>должно</em> работать</strong> но <em>я __не__ уверен</em></h1>",
                "возможны # неполадки. _неоконченный тег",
                "тут закончил_ __ __",
                @"# парочка э\кранирований _тегов_ __фыв_ <em>часть_</em>1_"}));
        }

        [Test]
        public void Render_ShouldWorkFast()
        {
            var sw = new Stopwatch();
            var res = new List<TimeSpan>();
            var count = 5;
            for (int i = 0; i < count; i++) 
                res.Add(new TimeSpan());
            for (int i = 0; i <= count + 2; i++)
            {
                var temp = GetTestResult(32000, 512000, 2);
                if (i > 1)
                    for (int j = 0; j < res.Count(); j++)
                        res[j] += temp[j];
            }

            for (int i = 0; i < res.Count(); i++)
                res[i] /= count;

            for (int i = 1; i < res.Count; i++)
                (res[i].Ticks / res[i - 1].Ticks).Should().BeLessThan(4);
        }

        private List<TimeSpan> GetTestResult(int startCharCount, int endCharCount, int multiplayer)
        {
            var sw = new Stopwatch();
            var res = new List<TimeSpan>();
            for (int i = startCharCount; i <= endCharCount; i *= multiplayer)
            {
                var text = GetRandomString(i);
                sw.Start();
                md.Render(text);
                sw.Stop();
                res.Add(sw.Elapsed);
                sw.Reset();
            }

            return res;
        }

        private string GetRandomString(int length)
        {
            var variants = new List<string>()
            {
                " ", "_", "__", "\\", "# ", "#", "\n",
            };
            for(int i ='a';i<'z';i++)
                variants.Add(((char) i).ToString());
            var text = new StringBuilder();
            var rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                text.Append(variants[rnd.Next(variants.Count)]);
            }
            return text.ToString();
        }
    }
}
