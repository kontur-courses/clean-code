namespace Markdown.Core.Lexing;

public enum TokenKind
{
    /// <summary>Обычный текст</summary>
    Text,
    /// <summary>Курсивный шрифт</summary>
    Underscore,
    /// <summary>Полужирный шрифт</summary>
    DoubleUnderscore,
    /// <summary>Заголовок</summary>
    Hash,
    /// <summary>Одиночный пробел</summary>
    Space,
    /// <summary>Перевод строки</summary>
    NewLine,
    /// <summary>Конец входа</summary>
    Eof,
    /// <summary>Квадратная скобка '[' открывает текст ссылки</summary>
    LeftBracket,
    /// <summary>Квадратная скобка ']' закрывает текст ссылки</summary>
    RightBracket,
    /// <summary>Круглая скобка '(' открывает адрес ссылки</summary>
    LeftParen,
    /// <summary>Круглая скобка ')' закрывает адрес ссылки</summary>
    RightParen,
}
