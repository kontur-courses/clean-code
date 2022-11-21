using System;
using System.Collections.Generic;
using System.Text;
using MrakdaunV1.Enums;
using MrakdaunV1.Interfaces;

namespace MrakdaunV1
{
    public class MrakdaunEngine : IMrakdaunEngine
    {
        private List<IStateParser> _stateParsers = new()
        {
            new ItalicStateParser(),
            new BoldStateParser()
        };
        private CharState[] _charStates;
        private List<TokenPart> tokenParts = new();

        private void ParseTokens(string text)
        {
            tokenParts = new();
            
            for (var i = 0; i < text.Length; i++)
            {
                // ищем землю
                if (text[i] != '_')
                    continue;

                // тройные земли пока игнорируются
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
                    
                // у жира две земли
                if (tokenPart.Type == TokenPartType.Bold)
                    i++;
                    
                tokenParts.Add(tokenPart);
            }
        }
        
        public string GetParsedText(string text)
        {
            _charStates = new CharState[text.Length];
            ParseTokens(text);
            
            foreach (var stateParser in _stateParsers)
            {
                _charStates = stateParser.GetParsedStates(tokenParts, _charStates, text);
            }

            //Console.WriteLine("Char states:");
            var sb = new StringBuilder();
            
            foreach (var state in _charStates)
            {
                var name = "";
                if ((state & CharState.Bold) == CharState.Bold)
                    name += "B";
                if ((state & CharState.Italic) == CharState.Italic)
                    name += "I";
                if ((state & CharState.Special) == CharState.Special)
                    name += "S";
                if (name == "")
                    name = "_";
                sb.Append(name + " ");
            }

            return sb.ToString().Trim();
        }
    }
}