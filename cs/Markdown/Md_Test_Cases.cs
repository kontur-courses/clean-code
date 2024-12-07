using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class Md_Test_Cases
    {
        public static IEnumerable<TestCaseData> SimpleTests
        {
            get
            {
                yield return new TestCaseData("_Hello_", "<em>Hello</em>").
                    SetName("Correct conversion of a italic tags");

                yield return new TestCaseData("#Hello", "<h1>Hello</h1>").
                    SetName("Correct conversion of a header tags");

                yield return new TestCaseData("__Hello__", "<strong>Hello</strong>").
                    SetName("Correct conversion of bold tags");
            }
        }

        public static IEnumerable<TestCaseData> TestsWhenSpacesBetweenTagAndWord
        {
            get
            {
                yield return new TestCaseData("_ Hello_", "_ Hello_").
                    SetName("Correct conversion when there is a space after the opening italic tag");

                yield return new TestCaseData("_Hello _", "_Hello _").
                    SetName("Correct conversion if there is a space before the closing italic tag");

                yield return new TestCaseData("__Hello __", "__Hello __").
                    SetName("Correct conversion if there is a space before the closing bold tag");

                yield return new TestCaseData("__ Hello__", "__ Hello__").
                    SetName("Correct conversion when there is a space after the opening bold tag");
            }
        }

        public static IEnumerable<TestCaseData> TestsWhenPairTagNotExists
        {
            get
            {
                yield return new TestCaseData("_Hello", "_Hello").
                    SetName("Correct conversion if there is no closing italic tag");

                yield return new TestCaseData("Hello_", "Hello_").
                    SetName("Correct conversion if there is no opening italic tag");

                yield return new TestCaseData("__Hello", "__Hello").
                    SetName("Correct conversion if there is no closing bold tag");

                yield return new TestCaseData("Hello__", "Hello__").
                    SetName("Correct conversion if there is no opening bold tag");

                yield return new TestCaseData("_Hello_world_", "<em>Hello</em>world_").
                    SetName("Correct conversion if there are several italic tags and there are extra ones");

                yield return new TestCaseData("_Hello_wor_ld_", "<em>Hello</em>wor<em>ld</em>").
                    SetName("Correct conversion if there are several tags");

                yield return new TestCaseData("Hel_lo w_orld", "Hel_lo w_orld").
                    SetName("No highlighting if the italic tags are in different words");

                yield return new TestCaseData("Hel__lo w__orld", "Hel__lo w__orld").
                    SetName("No highlighting if the bold tags are in different words");

                yield return new TestCaseData("__Hello_ world", "__Hello_ world").
                    SetName("Correct conversion of unpaired tags");
            }
        }

        public static IEnumerable<TestCaseData> TestsWhenPairTagsInDifferentWords
        {
            get
            {
                yield return new TestCaseData("Hel_lo w_orld", "Hel_lo w_orld").
                    SetName("No highlighting if the italic tags are in different words");

                yield return new TestCaseData("Hel__lo w__orld", "Hel__lo w__orld").
                    SetName("No highlighting if the bold tags are in different words");
            }
        }

        public static IEnumerable<TestCaseData> TestsWhenTagsHighlightPartOfWord
        {
            get
            {
                yield return new TestCaseData("_Hell_o _w_orld", "<em>Hell</em>o <em>w</em>orld").
                    SetName("Correct highlighting if italic tags highlight part of a word");

                yield return new TestCaseData("__Hell__o __w__orld", "<strong>Hell</strong>o <strong>w</strong>orld").
                    SetName("Correct highlighting if bold tags highlight part of a word");
            }
        }

        public static IEnumerable<TestCaseData> TestsWithoutWords
        {
            get
            {
                yield return new TestCaseData("__", "__").
                    SetName("Don't convert tags if there is nothing inside them");

                yield return new TestCaseData("____", "____").
                    SetName("Don't convert tags if there is nothing inside them");

                yield return new TestCaseData("__ __", "__ __").
                    SetName("Don't convert tags if there is only a space inside them");
            }
        }

        public static IEnumerable<TestCaseData> TestsWithIntersectionTags
        {
            get
            {
                yield return new TestCaseData("H __e _l_ l__ o", "H <strong>e <em>l</em> l</strong> o").
                    SetName("Correct converting if there are italic tags inside bold tags");

                yield return new TestCaseData("H _e __l__ l_ o", "H <em>e __l__ l</em> o").
                    SetName("Correct converting if there are bold tags inside italic tags");

                yield return new TestCaseData("H _e __l_ l__ o", "H _e __l_ l__ o").
                    SetName("Correct converting if there are intersection of tags");
            }
        }

        public static IEnumerable<TestCaseData> TestsWithEscape
        {
            get
            {
                yield return new TestCaseData(@"\Hello\", @"\Hello\").
                    SetName("Correct conversion if the escape symbol does not escape anything");

                yield return new TestCaseData(@"\\\_Hello_", @"\_Hello_").
                    SetName("Correct conversion with multiply escaping");

                yield return new TestCaseData(@"\_Hello_", @"_Hello_").
                    SetName("Correct conversion if the escaping character escapes the italic tag");

                yield return new TestCaseData(@"\__Hello__", @"__Hello__").
                    SetName("Correct conversion if the escaping character escapes the bold tag");

                yield return new TestCaseData(@"\\_Hello_", @"\<em>Hello</em>").
                    SetName("Correct conversion if the escaping character escapes another escaping character");

                yield return new TestCaseData(@"\# Hello", @"# Hello").
                    SetName("Correct conversion if the escaping character escapes the header tag");
            }
        }

        public static IEnumerable<TestCaseData> BigTests
        {
            get
            {
                yield return new TestCaseData(
                    "#заголовок __с жирным текстом__ \n" +
                    "Просто текст, в котором _курсивные_ выделения\n" +
                    "__Есть жирный текст__\n" +
                    "__А вот жирный текст _с курсивом_ внутри _и ещё курсив_ в жирном__\n" +
                    "_Вот __это_ не__ сработает\n" +
                    "_И вот так __тоже__ нет_\n" +
                    "Это - _ - просто подчёркивание\n" +
                    "Так_ не работает_\n" +
                    "И _ вот так _ тоже\n" +
                    "В с_лов_е можно выделять, а в цифрах 1_23_ нет\n",

                    "<h1>заголовок <strong>с жирным текстом</strong> </h1>\n" +
                    "Просто текст, в котором <em>курсивные</em> выделения\n" +
                    "<strong>Есть жирный текст</strong>\n" +
                    "<strong>А вот жирный текст <em>с курсивом</em> внутри <em>и ещё курсив</em> в жирном</strong>\n" +
                    "_Вот __это_ не__ сработает\n" +
                    "<em>И вот так __тоже__ нет</em>\n" +
                    "Это - _ - просто подчёркивание\n" +
                    "Так_ не работает_\n" +
                    "И _ вот так _ тоже\n" +
                    "В с<em>лов</em>е можно выделять, а в цифрах 1_23_ нет\n"
                    ).
                    SetName("Correct conversion with multiply lines");
            }
        }
    }
}
