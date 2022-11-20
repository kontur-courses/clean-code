using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal class MarkdownToHtmlParser
    {
        private static StateGraph stateGraph = new StateGraph();

        public string Render(string markdownText)
        {
            if (string.IsNullOrEmpty(markdownText))
                throw new ArgumentException(nameof(markdownText));

            var tokens = ParseStringToTokens(markdownText);
            ApplySymbolShielding(tokens);

            return ParseTokensToHtml(tokens);
        }

        private static List<Token> ParseStringToTokens(string text)
        {
            var tokens = new List<Token>();

            int index = 0;
            bool pathFound = false;
            StringBuilder bufferSb = new StringBuilder();
            char currentChar = ' ';
            while (index < text.Length)
            {
                currentChar = text[index];
                foreach (var path in stateGraph.CurrentState.Pathes)
                {
                    if (!path.Match(currentChar))
                        continue;

                    pathFound = true;
                    if (path.Destination == stateGraph.CurrentState)
                    {
                        bufferSb.Append(currentChar);
                    }
                    else
                    {
                        tokens.Add(stateGraph.CurrentState.GetResult(bufferSb.ToString()));
                        bufferSb.Clear();

                        stateGraph.CurrentState = path.Destination;
                        bufferSb.Append(currentChar);
                    }
                    
                    index++;
                    break;
                }

                if (!pathFound)
                {
                    //throw new ArgumentException($"Unexpected symbol '{text[index]}' at {index}");
                    tokens.Add(new Token()
                    {
                        Type = TokenType.Word,
                        HasBody = false,
                        Text = bufferSb.ToString()
                    });
                    bufferSb.Clear();
                }

                pathFound = false;
            }

            if(bufferSb.Length > 0)
            {
                tokens.Add(stateGraph.CurrentState.GetResult(bufferSb.ToString()));
            }

            return tokens;
        }

        private static void ApplySymbolShielding(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count - 1; i++)
            {
                var currentToken = tokens[i];
                if (currentToken.Type != TokenType.Backslash)
                    continue;

                var nextToken = tokens[i + 1];
                if (nextToken.Type == TokenType.Word)
                    continue;

                if (nextToken.Type == TokenType.Newline)
                    nextToken.Text = currentToken.Text + nextToken.Text;
                nextToken.Type = TokenType.Word;
                tokens.RemoveAt(i);
            }
        }

        private static string ParseTokensToHtml(List<Token> tokens)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var token in tokens)
                sb.Append(token.Text);
            return sb.ToString();
        }
    }
}