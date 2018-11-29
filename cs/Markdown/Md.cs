using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private List<string> tokens = new List<string>();

        private readonly Stack<Tag> openTags = new Stack<Tag>();

        public string Render(string inputText)
        {
            if (inputText == "") return "";
            tokens = SplitInputText(inputText);
            var markdown = new StringBuilder();
            for (var i = 0; i < tokens.Count; i++)
            {
                CreateTokensContext(i, out var previousToken, out var currentToken, out var nextToken);

                if (TryFeedTag(currentToken, i) || !MarkupLanguage.IsKeyWords(currentToken) ||
                    MarkupLanguage.IsSelectionOfCharacters(previousToken, currentToken, nextToken) ||
                    IsEscapeCharacter(currentToken, ref i)) continue;

                if (openTags.Count > 0 && Tag.TryCloseTag(previousToken, currentToken, nextToken, openTags.Peek()))
                    RenderTag(i);
                else if (Tag.TryOpenTag(previousToken, currentToken, nextToken, out var tag))
                    PutInOpenTags(tag, i);
            }

            foreach (var token in tokens)
                markdown.Append(token);

            return markdown.ToString();
        }

        private void RenderTag(int i)
        {
            var closedTag = openTags.Pop();
            tokens[closedTag.InitialIndex] = ((IPairTag)closedTag).StartTag;
            tokens[i] = ((IPairTag)closedTag).EndTag;
        }

        private void PutInOpenTags(Tag tag, int i)
        {
            if (tag is IPairTag && openTags.All(t => t is IPairTag pairTag && pairTag.CanContainTag(tag)))
            {
                openTags.Push(tag);
                tag.InitialIndex = i;
            }
        }

        private bool TryFeedTag(string currentToken, int i)
        {
            if (openTags.Count > 0 && (openTags.Peek() as IPairTag).TryEat(currentToken))
            {
                tokens[i] = "";
                return true;
            }

            return false;
        }

        private void CreateTokensContext(int i, out string previousToken, out string currentToken, out string nextToken)
        {
            previousToken = i == 0 ? "" : tokens[i - 1];
            currentToken = tokens[i];
            nextToken = i + 1 == tokens.Count ? "" : tokens[i + 1];
        }

        private bool IsEscapeCharacter(string currentToken, ref int i)
        {
            if (MarkupLanguage.EscapeCharacter == currentToken)
            {
                tokens[i] = "";
                i += 2;
                return true;
            }

            return false;
        }

        public List<string> SplitInputText(string inputText)
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
                    if (currentToken.Length > 0) tokens.Add(currentToken.ToString());
                    tokens.Add(possibleKeyWord);
                    currentToken.Clear();
                    break;
                }
            }
            if (currentToken.Length > 0) tokens.Add(currentToken.ToString());
            return tokens;
        }
    }
}
