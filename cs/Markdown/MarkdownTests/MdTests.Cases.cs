namespace MarkdownTests;

public partial class MdTests
{
    private static readonly IEnumerable<TestCaseData> conversionTestCases =
        [
            new TestCaseData(
                "# Заголовок с разными символами\n",
                "<h1>Заголовок с разными символами</h1>")
            .SetName("Simple Headline"),
            new TestCaseData(
                "Текст, _окруженный с двух сторон_ одинарными символами подчерка",
                "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка")
            .SetName("Simple Italic"),
            new TestCaseData(
                "__Выделенный двумя символами текст__ должен становиться полужирным",
                "<strong>Выделенный двумя символами текст</strong> должен становиться полужирным")
            .SetName("Simple Bold"),
            new TestCaseData(
                "Однако выделять часть слова они могут: и в __нач__але",
                "Однако выделять часть слова они могут: и в <strong>нач</strong>але")
            .SetName("HighlightingBeginningWord"),
            new TestCaseData(
                "Однако выделять часть слова они могут: и в сер_еди_не,",
                "Однако выделять часть слова они могут: и в сер<em>еди</em>не,")
            .SetName("Highlighting Middle Word"),
            new TestCaseData(
                "Однако выделять часть слова они могут: и в кон_це._",
                "Однако выделять часть слова они могут: и в кон<em>це.</em>")
            .SetName("Highlighting End Word"),
            new TestCaseData(
                "Внутри __двойного выделения _одинарное_ тоже__ работает.",
                "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.")
            .SetName("Nested Tags Italic In Bold"),
            new TestCaseData(
                "Но не наоборот — внутри _одинарного __двойное__ не_ работает.",
                "Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.")
            .SetName("Nested Tags Bold In Italic"),
            new TestCaseData(
                "# Заголовок __с _разными_ символами__\n",
                "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")
            .SetName("Overlapping Tags"),
                    new TestCaseData(
                "_Разные_ __теги__ и [ссылка](https://google.com)",
                "<em>Разные</em> <strong>теги</strong> и <a href=\"https://google.com\">ссылка</a>")
            .SetName("Text with links"),
    ];
}
