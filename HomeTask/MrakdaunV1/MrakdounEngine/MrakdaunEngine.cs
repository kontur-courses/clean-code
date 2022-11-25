using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrakdaunV1.Enums;
using MrakdaunV1.Interfaces;
using MrakdaunV1.MrakdounEngine;

namespace MrakdaunV1
{
    public class MrakdaunEngine : IMrakdaunEngine
    {
        private IStateParser[] _stateParsers =
        {
            new ItalicStateParser(),
            new BoldStateParser()
        };

        private readonly char[] _specialChars = @"#_\".ToCharArray();

        private void FillStatesData(string text, CharState[] _charStates, List<TokenPart> _tokenParts)
        {
            // парсим заголовок
            if (text[0] == '#' && text[1] == ' ')
            {
                _charStates[0] |= CharState.Special;
                _charStates[1] |= CharState.Special;

                for (int i = 0; i < text.Length; i++)
                    _charStates[i] |= CharState.Header1;
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
                        _charStates[i] |= CharState.Special;
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

                // создаем кусочек токена, он хранит инфу о том, какой это тег + добавочная инфа
                var tokenPart = new TokenPart(text, i);
                //Console.WriteLine(tokenPart);

                // у жира две земли
                if (tokenPart.Type == TokenPartType.Bold)
                    i++;

                _tokenParts.Add(tokenPart);
            }
        }

        public CharState[] GetParsedTextStates(string text)
        {
            List<CharState> allStates = new();

            foreach (var line in text.SplitWithDelimeter('\n'))
                allStates.AddRange(GetParsedStatesFromLine(line));

            return allStates.ToArray();
        }

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

        private CharState[] GetParsedStatesFromLine(string text)
        {
            text += " ";

            CharState[] charStates = new CharState[text.Length];
            List<TokenPart> tokenParts = new();
            FillStatesData(text, charStates, tokenParts);

            // запускаем каждый из обработчиков
            foreach (var stateParser in _stateParsers)
                charStates = stateParser.GetParsedStates(tokenParts, charStates, text);

            charStates = charStates.Take(charStates.Length - 1).ToArray();

            return charStates;
        }

        public string GetParsedText(string text)
        {
            return new HtmlRednerer().Render(text, GetParsedTextStates(text));
        }
    }
}