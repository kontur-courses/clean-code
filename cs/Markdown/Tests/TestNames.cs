namespace Markdown.Tests;

public static class TestNames
{
    public const string ReplaceItalicTag = @"The text surrounded on both sides by single handwriting characters should be placed in the HTML tag \<em>";
    public const string ReplaceBoldTag = @"___The __ text separated by two characters should become bold using the \<strong> tag.";
    public const string ReplaceItalicTagInsideBoldTag = @"Inside the __double selection _ single_ is__ works";
    public const string ReplaceBoldTagInsideItalicTag = @"Inside the __single selection _ double is__ not works";
    public const string ReplaceTagInsideWordWithNumbersIsNotWorks = @"Handwriting inside the text with numerals_12_3 is not considered highlighting and must remain handwriting characters.";
    public const string ReplaceTagInsideWordIsWorks = @"However, they can highlight a part of the word: both in the beginning, and in the middle, and in the end.";
    public const string ReplaceTagInDifferentWordsIsNotWorks = @"At the same time, the selection in different syllables does not work.";
    public const string ReplaceOpenTagSymbolMustBeNotFree = @"The handwriting that begins the selection should be followed by a non-whitespace character. Otherwise, these underscores are not considered highlighting and remain just handwriting symbols.";
    public const string ReplaceCloseTagSymbolMustBeNotFree = @"The handwriting ending the selection should follow the non-white character. Otherwise, these _districts _ are not considered the end of the selection and remain just handwriting symbols.";
    public const string ReplaceIntersectsSymbolsIsNotWorks = @"In the case of __intersection of _double__ and single_ handwriting, none of them is considered a selection.";
    public const string ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks = @"If there is an empty string ____ inside the handwriting, then they remain handwriting characters.";
}