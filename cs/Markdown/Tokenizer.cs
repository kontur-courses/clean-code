using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        public List<Token> GetTokens(string text)
        {                                  
            var states = FindTokenLocations(text);
            var tokenList = new List<Token>();
            Tuple<TextType, int> currentStrong = null;
            Tuple<TextType, int> currentItalic = null;
            var italicCounter = 0;
            var boldCounter = 0;
            for (var i = 0; i < states.Count(); i++)
            {
                if (states[i].Item1 == TextType.Strong)
                {
                    if (boldCounter % 2 == 0)
                        currentStrong = states[i];
                    else
                    {
                        tokenList.Add(new Token(currentStrong.Item2, states[i].Item2, text, TextType.Strong, currentItalic));
                        currentStrong = null;
                    }
                    boldCounter++;
                }
                if (states[i].Item1 == TextType.Italic)
                {
                    if (italicCounter % 2 == 0)
                        currentItalic = states[i];
                    else
                    {
                        tokenList.Add(new Token(currentItalic.Item2, states[i].Item2, text, TextType.Italic, currentStrong));
                        currentItalic = null;
                    }
                    italicCounter++;
                }
            }
            return tokenList;                                  
        }

        private List<Tuple<TextType, int>> FindTokenLocations(string text)
        {
            var states = new List<Tuple<TextType, int>>();
            var underscoreCount = 0;
            var typeTagsLengthDict = new Dictionary<int, TextType>();
            typeTagsLengthDict.Add(1, TextType.Italic);
            typeTagsLengthDict.Add(2, TextType.Strong);
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '_')
                {
                    underscoreCount++;
                    if (i > 0)
                    {
                        if (text[i - 1].ToString() == @"\")
                            underscoreCount--;
                    }
                }
                if (underscoreCount > 0 && (i == text.Length - 1 || text[i] != '_'))
                {
                    if (AnalizeCorrectness(text, underscoreCount, i, states))
                    {
                        var shift = 0;
                        if (i == text.Length - 1 && text[i] == '_')
                            shift = 1;
                        states.Add(new Tuple<TextType, int>(typeTagsLengthDict[underscoreCount], i - underscoreCount + shift));
                        underscoreCount = 0;
                        continue;
                    }
                    else
                        underscoreCount = 0;
                }
            }
            return states;
        }
                
        private bool AnalizeCorrectness(string original, int underscore, int position, List<Tuple<TextType, int>> currentState)
        {
            var italicCount = currentState.Count(s => s.Item1 == TextType.Italic);
            var strongCount = currentState.Count(s => s.Item1 == TextType.Strong);
            var shift = 2;
            if (underscore == 2)
                shift = 3;
            if (position == original.Length - 1)
                shift--;
            var italicCondition= underscore == 1 && italicCount % 2 == 0;
            var strongCondition = underscore == 2 && strongCount % 2 == 0;
            if (italicCondition || strongCondition)
            {
                if (position == original.Length - 1)
                    return false;
                return original[position] != ' ';
            }
            else
                return original[position - shift] != ' ';            
        }
    }
}
