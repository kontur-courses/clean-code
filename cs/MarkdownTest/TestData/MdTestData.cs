namespace MarkdownTest.TestData;

public class MdTestData
{
    public static TestCaseData[] SpecExamples =
    {
        new TestCaseData(
            "Текст, _окруженный с двух сторон_ одинарными символами подчерка,\nдолжен помещаться в HTML-тег <em>.",
            "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка,\nдолжен помещаться в HTML-тег <em>."
        ).SetName("SimpleEmTagging"),
        new TestCaseData(
            "__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега <strong>.",
            "<strong>Выделенный двумя символами текст</strong> должен становиться полужирным с помощью тега <strong>."
        ).SetName("SimpleStrongTagging"),
        new TestCaseData(
            "Любой символ можно экранировать, чтобы он не считался частью разметки.\n \\_Вот это\\_, не должно выделиться тегом <em>.",
            "Любой символ можно экранировать, чтобы он не считался частью разметки.\n _Вот это_, не должно выделиться тегом <em>."
        ).SetName("TagShielding"),
        new TestCaseData(
            "Символ экранирования исчезает из результата, только если экранирует что-то.\nЗдесь сим\\волы экранирования\\ \\должны остаться.\\",
            "Символ экранирования исчезает из результата, только если экранирует что-то.\nЗдесь сим\\волы экранирования\\ \\должны остаться.\\"
        ).SetName("ReverseSlashes_RemainItselfIfDoesNotShielding"),
        new TestCaseData(
            @"Символ экранирования тоже можно экранировать: \\_вот_ это будет выделено тегом <em>",
            @"Символ экранирования тоже можно экранировать: \<em>вот</em> это будет выделено тегом <em>"
        ).SetName("ShieldingReverseSlash"),
        new TestCaseData(
            "Внутри __двойного выделения _одинарное_ тоже__ работает.",
            "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает."
        ).SetName("EmInStrongWork"),
        new TestCaseData(
            "Но не наоборот — внутри _одинарного __двойное__ не_ работает.",
            "Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает."
        ).SetName("StrongInEmDoesNotWork"),
        new TestCaseData(
            "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.",
            "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка."
        ).SetName("TaggingNumber_RemainUnderscores"),
        new TestCaseData(
            "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._",
            "Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>"
        ).SetName("TaggingInDifferentPartsOfWord"),
        new TestCaseData(
            "В то же время выделение в ра_зных сл_овах не работает.",
            "В то же время выделение в ра_зных сл_овах не работает."
        ).SetName("TaggingInDifferentWordsDoesNotWork"),
        new TestCaseData(
            "__Непарные_ символы в рамках одного абзаца не считаются выделением.",
            "__Непарные_ символы в рамках одного абзаца не считаются выделением."
        ).SetName("NotPairedSymbols_RemainIteself"),
        new TestCaseData(
            "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением \nи остаются просто символами подчерка.",
            "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением \nи остаются просто символами подчерка."
        ).SetName("SingleUnderscoreWithWhitespaceAhead_RemainUnderscore"),
        new TestCaseData(
            "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения \nи остаются просто символами подчерка.",
            "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти <em>подчерки _не считаются</em> окончанием выделения \nи остаются просто символами подчерка."
        ).SetName("SingleInderscore_RemainUnderscoreInEmTag"),
        new TestCaseData(
            "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.",
            "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением."
        ).SetName("TagsIntersection_RemainUnderscores"),
        new TestCaseData(
            "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.",
            "Если внутри подчерков пустая строка ____, то они остаются символами подчерка."
        ).SetName("TwoDoubleUnderscoresInRow_RemainUnderscores"),
        new TestCaseData(
            "# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"
        ).SetName("HeaderWithInnerTags"),
        new TestCaseData(
            "[Текст помещённый в тэг link, ведущий на главную страницу google](https://www.google.com/)",
            "<a href=\"https://www.google.com/\">Текст помещённый в тэг link, ведущий на главную страницу google</a>"
        ).SetName("LinkTag"),
    };

    public static TestCaseData[] Examples =
    {
        new TestCaseData(
            "_abc def_",
            "<em>abc def</em>"
        ).SetName("SimpleEmTagging"),
        new TestCaseData(
            "__abc def__",
            "<strong>abc def</strong>"
        ).SetName("SimpleStrongTagging"),
        new TestCaseData(
            "\\_abc\\_",
            "_abc_"
        ).SetName("TagShielding"),
        new TestCaseData(
            "Back\\slash \\that not shielding\\",
            "Back\\slash \\that not shielding\\"
        ).SetName("ReverseSlashes_RemainItselfIfDoesNotShielding"),
        new TestCaseData(
            @"\\_abc_",
            @"\<em>abc</em>"
        ).SetName("ShieldingReverseSlash"),
        new TestCaseData(
            "__abc def _ghi_ jkl mno__",
            "<strong>abc def <em>ghi</em> jkl mno</strong>"
        ).SetName("EmInStrongWork"),
        new TestCaseData(
            "_abc def __ghi__ jkl mno_",
            "<em>abc def __ghi__ jkl mno</em>"
        ).SetName("StrongInEmDoesNotWork"),
        new TestCaseData(
            "_123_",
            "_123_"
        ).SetName("EmTaggingNumber_RemainUnderscores"),
        new TestCaseData(
            "__123__",
            "__123__"
        ).SetName("StrongTaggingNumber_RemainUnderscores"),
        new TestCaseData(
            "_abc_d e_fgh_i j_klm_",
            "<em>abc</em>d e<em>fgh</em>i j<em>klm</em>"
        ).SetName("TaggingInDifferentPartsOfWord"),
        new TestCaseData(
            "a_bc de_f",
            "a_bc de_f"
        ).SetName("TaggingInDifferentWordsDoesNotWork"),
        new TestCaseData(
            "__abc_",
            "__abc_"
        ).SetName("NotPairedSymbols_RemainIteself"),
        new TestCaseData(
            "ab_ cd_",
            "ab_ cd_"
        ).SetName("SingleUnderscoreWithWhitespaceAhead_RemainUnderscore"),
        new TestCaseData(
            "__ b__c",
            "__ b__c"
        ).SetName("DoubleUnderscoreWithWhitespaceAhead_RemainUnderscore"),
        new TestCaseData(
            "_ b_c",
            "_ b_c"
        ).SetName("DoubleUnderscoreWithWhitespaceAhead_RemainUnderscore"),
        new TestCaseData(
            "_abc _def ghi_",
            "<em>abc _def ghi</em>"
        ).SetName("SingleInderscore_RemainUnderscoreInEmTag"),
        new TestCaseData(
            "__abc _def__ ghi_",
            "__abc _def__ ghi_"
        ).SetName("TagsIntersection_RemainUnderscores"),
        new TestCaseData(
            "____",
            "____"
        ).SetName("TwoDoubleUnderscoresInRow_RemainUnderscores"),
        new TestCaseData(
            "# abc __def _ghi_ kjl__",
            "<h1>abc <strong>def <em>ghi</em> kjl</strong></h1>"
        ).SetName("HeaderWithInnerTags"),
        new TestCaseData(
            "[abc](https://www.example.com/)",
            "<a href=\"https://www.example.com/\">abc</a>"
        ).SetName("LinkTag"),
        new TestCaseData(
            "[_abc_ __def__](https://www.example.com/)",
            "<a href=\"https://www.example.com/\"><em>abc</em> <strong>def</strong></a>"
        ).SetName("StyledLinkTagText"),
        new TestCaseData(
            "[a](_bc_)",
            "<a href=\"_bc_\">a</a>"
        ).SetName("StyleInLinkSourceNotAllow"),
        new TestCaseData(
            "a# bc\r\ncd# e",
            "a# bc\r\ncd# e"
        ).SetName("HeaderMustStartAfterNewLine"),
        new TestCaseData(
            "# ab# c",
            "<h1>ab# c</h1>"
        ).SetName("HashSymbolAfterHeaderStart"),
    };
}