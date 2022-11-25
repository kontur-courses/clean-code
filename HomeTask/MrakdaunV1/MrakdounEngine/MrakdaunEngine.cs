using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrakdaunV1.Enums;
using MrakdaunV1.Interfaces;

namespace MrakdaunV1.MrakdounEngine
{
    public class MrakdaunEngine : IMrakdaunEngine
    {
        private readonly IStateParser[] _stateParsers =
        {
            new ItalicStateParser(),
            new BoldStateParser()
        };

        private readonly char[] _specialChars = @"#_\".ToCharArray();
        
        
        /// <summary>
        /// Метод вернет HTML строку, принимая на вход MD строку
        /// </summary>
        public string GetParsedText(string text)
        {
            return new HtmlRednerer().Render(text, GetParsedTextStates(text));
        }

        /// <summary>
        /// Метод вернет стейты для исходного текста
        /// </summary>
        public CharState[] GetParsedTextStates(string text)
        {
            List<CharState> allStates = new();

            // для строк, независимо друг от друга, запускаем обработку
            // и формируем общий массив стейтов для всего текста
            foreach (var line in text.SplitWithDelimiter('\n'))
                allStates.AddRange(GetParsedStatesFromLine(line));

            return allStates.ToArray();
        }

        /// <summary>
        /// Метод сформирует наглядное предстваление массива стейтов
        /// </summary>
        public string GetCharStatesString(CharState[] states)
        {
            var result = new StringBuilder();

            foreach (var state in states)
            {
                var name = "";
                if (state.HasFlag(CharState.Bold))
                    name += "B";
                if (state.HasFlag(CharState.Italic))
                    name += "I";
                if (state.HasFlag(CharState.Special))
                    name += "S";
                if (state.HasFlag(CharState.Header1))
                    name += "H1";
                if (name == "")
                    name = "_";
                result.Append(name + " ");
            }

            return result.ToString().Trim();
        }
        
        /// <summary>
        /// Метод заполнит токены (а для некоторых тегов и стейты)
        /// для дальнейшей работы 
        /// </summary>
        private void FillStatesData(string text, CharState[] charStates, List<TokenPart> tokenParts)
        {
            // парсим заголовок
            if (text[0] == '#' && text[1] == ' ')
            {
                charStates[0] |= CharState.Special;
                charStates[1] |= CharState.Special;

                for (int i = 0; i < text.Length; i++)
                    charStates[i] |= CharState.Header1;
            }

            for (var i = 0; i < text.Length; i++)
            {
                if (!_specialChars.Contains(text[i]))
                    continue;

                // парсим экранирование, если после слеша идет
                // управляющий символ, то мы его пропустим из обработки
                if (i <= text.Length - 1 && text[i] == '\\')
                {
                    if (_specialChars.Contains(text[i + 1]))
                        charStates[i] |= CharState.Special;
                    i++;
                    continue;
                }

                // тройные земли игнорируются
                if (i <= text.Length - 3
                    && text[i] == '_'
                    && text[i + 1] == '_'
                    && text[i + 2] == '_')
                {
                    i += 2;
                    continue;
                }

                // создаем кусочек токена, он хранит инфу о теге (не обязательно о всем сразу)
                var tokenPart = new TokenPart(text, i);
                
                // у жирного выделения две земли, поэтому также сдвинем индекс
                if (tokenPart.Type == TokenPartType.Bold)
                    i++;
                
                tokenParts.Add(tokenPart);
            }
        }


        /// <summary>
        /// Метод вернет массив стейтов для строки
        /// </summary>
        private CharState[] GetParsedStatesFromLine(string text)
        {
            text += " ";

            CharState[] charStates = new CharState[text.Length];
            List<TokenPart> tokenParts = new();
            
            // заполняем токены, а также частично стейты
            FillStatesData(text, charStates, tokenParts);

            // запускаем каждый из обработчиков
            // и постепенно формируем итоговый вариант стейтов
            foreach (var stateParser in _stateParsers)
                charStates = stateParser.GetParsedStates(tokenParts, charStates, text);

            charStates = charStates.Take(charStates.Length - 1).ToArray();

            return charStates;
        }
    }
}