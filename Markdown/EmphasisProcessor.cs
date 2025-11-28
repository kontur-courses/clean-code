using System.Collections.Generic;

namespace Markdown
{
    internal sealed class EmphasisProcessor : ITokenProcessor
    {
        public List<Token> Process(List<Token> tokens)
        {

            int n = tokens.Count;
            var pair = new int[n];
            for (int i = 0; i < n; i++) pair[i] = -1;

            int lastOpenEm = -1;
            int lastOpenStrong = -1; 

            bool isEmOpenInsideWord = false;
            bool isStrongOpenInsideWord = false;
            bool isInsideWord = false;

            int lastDigit = -1;
            
            for(int i = 0; i < n; i++) // первый проход по массиву токенов, делаем первоначальный матчинг пар
            {
                if (tokens[i].Type == TokenType.Text ||
                    tokens[i].Type == TokenType.Digit)
                {
                    isInsideWord = true;
                }
                if (tokens[i].Type == TokenType.Digit)
                {
                    lastDigit = i;
                    lastOpenEm = -1;
                    lastOpenStrong = -1;
                    isEmOpenInsideWord = false;
                    isStrongOpenInsideWord = false;
                }
                if (tokens[i].Type == TokenType.Whitespace)
                {
                    isInsideWord = false;
                    if (isEmOpenInsideWord)
                    {
                        isEmOpenInsideWord = false;
                        lastOpenEm = -1;
                    }
                    if (isStrongOpenInsideWord)
                    {
                        isStrongOpenInsideWord = false;
                        lastOpenStrong = -1;
                    }
                    continue;
                }
                if (tokens[i].Type == TokenType.Digit)
                {
                    lastOpenEm = -1;
                    lastOpenStrong = -1;
                }
                if (tokens[i].Type == TokenType.Underscore)
                {
                    if (lastOpenEm != -1 && tokens[i].CanClose)
                    {
                        pair[lastOpenEm] = i;
                        pair[i] = lastOpenEm;
                        lastOpenEm = -1;
                        continue;
                    }
                    if(tokens[i].CanOpen)
                    {
                        lastOpenEm = i;
                        isEmOpenInsideWord = isInsideWord;
                        continue;
                    }
                }
                if (tokens[i].Type == TokenType.DoubleUnderscore)
                {
                    if (lastOpenStrong != -1 && tokens[i].CanClose)
                    {
                        pair[lastOpenStrong] = i;
                        pair[i] = lastOpenStrong;
                        lastOpenStrong = -1;
                        continue;
                    }
                    if (tokens[i].CanOpen)
                    {
                        lastOpenStrong = i;
                        isStrongOpenInsideWord = isInsideWord;
                        continue;
                    }
                }
            }
            // обрабатывает различные неучтённые при первом проходе ситуации
            bool isEmOpen = false;
            bool isEmInInterval = false;
            bool isStrongInInterval = false;
            bool isStrongOpen = false;
            int lastEmPair = -1;
            int lastStrongPair = -1;
            for (int i = 0; i < n; i++)
            {
                if (pair[i] == -1)
                {
                    isEmInInterval |= tokens[i].Type == TokenType.Underscore && 
                        isStrongOpen;
                    isStrongInInterval |= tokens[i].Type == TokenType.DoubleUnderscore &&
                        isEmOpen;
                    continue;
                }
                if (tokens[i].Type == TokenType.Underscore)
                {
                    bool isOpening = pair[i] > i;
                    isEmOpen = isOpening;
                    if (isOpening)
                    {
                        lastEmPair = i;
                        isStrongInInterval = false;
                    }
                    else if (isStrongOpen && lastEmPair < lastStrongPair )
                    {
                        
                        pair[pair[lastStrongPair]] = -1;
                        pair[lastStrongPair] = -1;
                        pair[pair[i]] = -1;
                        pair[i] = -1;
                        isEmOpen = false;
                        isStrongOpen = false;
                    }
                    else if (isStrongInInterval || Math.Abs(i - pair[i]) == 1) 
                    {
                        pair[pair[i]] = -1;
                        pair[i] = -1;
                        isEmOpen = false;
                    }
                }
                else
                {
                    bool isOpening = pair[i] > i;
                    isStrongOpen = isOpening;
                    if (isOpening)
                    {
                        lastStrongPair = i;
                        isEmInInterval = false;
                    }
                    else if (isEmOpen && lastEmPair > lastStrongPair)
                    {
                        pair[pair[lastEmPair]] = -1;
                        pair[lastEmPair] = -1;
                        pair[pair[i]] = -1;
                        pair[i] = -1;
                        isEmOpen = false;
                        isStrongOpen = false;
                    }
                    else if (isEmInInterval || Math.Abs(i - pair[i]) == 1)
                    {
                        pair[pair[i]] = -1;
                        pair[i] = -1;
                        isStrongOpen = false;
                    }
                }
            }

            // обрабатывает ситуацию "_пример __теста__ пример_" -> "<em>пример __теста__ пример</em>"
            isEmOpen = false;
            for(int i = 0; i < n; i++)
            {
                if (pair[i] == -1)
                {
                    continue;
                }
                if (tokens[i].Type == TokenType.Underscore)
                {
                    isEmOpen = pair[i] > i;
                }
                else
                {
                    if (isEmOpen)
                    {
                        pair[pair[i]] = -1;
                        pair[i] = -1;
                    }
                }
            }

            
            var result = new List<Token>(n);
            for (int i = 0; i < n; i++)
            {
                var t = tokens[i];

                if (t.Type == TokenType.Underscore || t.Type == TokenType.DoubleUnderscore)
                {

                    if (pair[i] != -1)
                    {
                        bool isOpening = pair[i] > i;
                        if (t.Type == TokenType.DoubleUnderscore)
                            result.Add(new Token(TokenType.Tag, false, false, isOpening ? "<strong>" : "</strong>"));
                        else
                            result.Add(new Token(TokenType.Tag, false, false, isOpening ? "<em>" : "</em>"));
                        continue;
                    }
                    else
                    {
                        result.Add(new Token(TokenType.Text, false, false, t.Text));
                        continue;
                    }

                }

                result.Add(t);
            }

            return result;
        }
    }
}
