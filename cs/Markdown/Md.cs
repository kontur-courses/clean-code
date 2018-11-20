﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private LinkedList<string> tokens = new LinkedList<string>();
        private readonly Stack<Tag> openTags = new Stack<Tag>();
        public string Render(string inputText)
        {
            if (inputText == "") return "";
            tokens = SplitInputText(inputText);
            var markdown = new StringBuilder();
            var currentNode = tokens.First;
            do
            {
                if (!MarkupLanguage.IsKeyWords(currentNode.Value)) continue;
                if (MarkupLanguage.LanguageRules.Any(r => !r(currentNode))) continue;

                if (openTags.Count > 0 && Tag.TryCloseTag(currentNode, openTags.Peek()))
                {
                    openTags.Pop();
                }
                else if (Tag.TryOpenTag(currentNode, out var tag))
                {
                    if (tag is IPairTag &&
                        openTags.All(t => t is IPairTag pairTag && pairTag.CanIContainThisTagRule(tag)))
                        openTags.Push(tag);
                }

            } while ((currentNode = currentNode.Next) != null);

            foreach (var token in tokens)
                markdown.Append(token);

            return markdown.ToString();
        }

        public LinkedList<string> SplitInputText(string inputText)
        {
            var textFromTokens = new StringBuilder(inputText);
            var currentToken = new StringBuilder();
            for (var i = 0; i < textFromTokens.Length; i++)
            {
                var possibleKeyWords = MarkupLanguage.GetKeyWordsOnFirstLetter(textFromTokens[i]);
                if (possibleKeyWords.Count == 0)
                {
                    currentToken.Append(textFromTokens[i]);
                    continue;
                }
                foreach (var possibleKeyWord in possibleKeyWords)
                {
                    if (!inputText.Substring(i).StartsWith(possibleKeyWord)) continue;
                    i += possibleKeyWord.Length - 1;
                    if (currentToken.Length > 0) tokens.AddLast(currentToken.ToString());
                    tokens.AddLast(possibleKeyWord);
                    currentToken.Clear();
                    break;
                }
            }
            if (currentToken.Length > 0) tokens.AddLast(currentToken.ToString());
            return tokens;
        }
    }
}
