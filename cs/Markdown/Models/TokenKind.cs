namespace Markdown.Models;

public enum TokenKind
{
    Text, // Обычный текст
    EmphasisStart, // Курсив начало
    EmphasisEnd, // Курсив конец
    StrongStart, // Полужирный начало
    StrongEnd, // Полужирный конец
    HeadingStart, // Заголовок начало
    HeadingEnd // Заголовок конец
}