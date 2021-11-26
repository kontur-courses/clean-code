using FluentAssertions;
using MarkDown;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MarkDownTests
{
    public class MdTests
    {
        [Test]
        public void Render_TestWithAllTags_ShouldWorkCorrectly()
        {
            var text = "# header __strong__ _em_ \r\n" +
                       "* _двойное __внутри__ одинарного не работает_ \r\n" +
                       "* __одинарное _внутри_ двойного работает__ \r\n" +
                       "* _при__пересечении_игнорируем__всю разметку в параграфе \r\n" +
                       "* #в списках не может быть заголовков \r\n" +
                       "* \\_экранирование\\_ работает \r\n" +
                       "* _ такое _ не _ работает __совсем __ даже ес_ли очень_ хочется";

            var exp = "<h1>header <strong>strong</strong> <em>em</em> </h1>\r\n" +
                       "<li><em>двойное __внутри__ одинарного не работает</em> </li>\r\n" +
                       "<li><strong>одинарное <em>внутри</em> двойного работает</strong> </li>\r\n" +
                       "<li>_при__пересечении_игнорируем__всю разметку в параграфе </li>\r\n" +
                       "<li>#в списках не может быть заголовков </li>\r\n" +
                       "<li>_экранирование_ работает </li>\r\n" +
                       "<li>_ такое _ не _ работает __совсем __ даже ес_ли очень_ хочется</li>";
            Md.Render(text).Should().Be(exp);
        }

        [Test]
        public void Render_Simple()
        {
            var text = "абобус";
            Md.Render(text).Should().Be("абобус");
        }

        [Test]
        public void Render_Header()
        {
            var text = "# абобус";
            Md.Render(text).Should().Be("<h1>абобус</h1>");
        }

        [Test]
        public void Render_MustBeLinear()
        {
            var bigResults = new List<double>();
            Stopwatch sw = new Stopwatch();
            MeasureAndAddInList(bigResults, sw, 10);
            //4 -- потому что 2*2 будет квадратом, а всё, что меньше квадрата -- ещё более-менее линия
            //На деле будет около двух.
            bigResults.Average().Should().BeLessThan(4);
        }

        private void MeasureAndAddInList(List<double> bigResults, Stopwatch sw, int nTimes)
        {
            for (int ii = 0; ii < nTimes; ii++)
            {
                List<string> texts = new();
                var i = 16;
                while (i < 2048 * 128)
                {
                    texts.Add(GenerateRandomString(i * 10));
                    i *= 2;
                }
                List<long> results = new();
                for (int j = 0; j < texts.Count; j++)
                {
                    sw.Start();
                    var result = Md.Render(texts[j]);
                    sw.Stop();
                    results.Add(sw.ElapsedTicks);
                    sw.Reset();
                }
                var counter = 0d;
                for (int j = 0; j < results.Count - 1; j++)
                {
                    counter += (double)results[j + 1] / results[j];
                }
                bigResults.Add(counter / (results.Count - 1));
            }
        }

        private string GenerateRandomString(int length)
        {
            var random = new Random();
            var SB = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                if (random.NextDouble() > 0.9)
                {
                    SB.Append('_');
                }
                if (random.NextDouble() > 0.9)
                {
                    SB.Append('*');
                }
                if (random.NextDouble() > 0.9)
                {
                    SB.Append('#');
                }
                if (random.NextDouble() > 0.9)
                {
                    SB.Append('\\');
                }
                SB.Append((char)random.Next(128));
            }
            return SB.ToString();
        }
    }
 }
