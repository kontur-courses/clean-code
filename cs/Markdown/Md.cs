using System;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var characterLevelTokenizer = new CharacterLevelTokenizer();
            var tagLevelTokenizer = new TagLevelTokenizer();
            var semanticLeveTokenizer = new SemanticLevelTokenizer();
            var tokensToHtmlConverter = new TokensToHTMLConverter();
            var characterTokenList = characterLevelTokenizer.Tokenize(text);
            var tagTokenList = tagLevelTokenizer.Tokenize(characterTokenList);
            var semanticTokenList = semanticLeveTokenizer.Tokenize(tagTokenList);
            return tokensToHtmlConverter.Convert(semanticTokenList);
        }
    }
}

/* Есть класс Token, у него два поля: значение токена и тип токена. Например Token('abc', тип.String)
1. Определение токенов первого уровня из текста: Пробел, нижнее подчёркивание, решётка, обратный слеш, новая строка, а всё остальное - строки
Формируется список, в котором лежат Token: значение строки и её тип, если склеить все строки по порядку, то получится исходный текст.
2. Создание нового списка с токенами второго уровня: обрабатка токенов первого уровня и замена их на SecondTokenType,
получился новый список с Токенами, в котором лежат токены, которые потенциально могут быть тегами
3. Анализ предыдущего списка: опредление может ли токен быть тегом или нет, если нет, то тогда его тип заменяется на String
4. Формирование html строки по последнему списку токенов. (Наверное стоит написать класс, который будет это делать)
    
    character level tokenizer
    tag level tokenizer
    semantic level tokenizer
    html printer
    */

    
    
