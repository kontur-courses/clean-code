using System.Collections.Generic;
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
            var result = new StringBuilder();
            var currentNode = tokens.First;
            do
            {
                if (!MarkupLanguage.IsKeyWords(currentNode.Value)) continue;
                if (MarkupLanguage.LanguageRules.Any(r => !r(currentNode))) continue;

                if ((!currentNode.Previous?.Value.EndsWith(" ") ?? false))
                    CloseTag(currentNode);
                else if ((!currentNode.Next?.Value.StartsWith(" ") ?? false))
                    CreateTag(currentNode);

            } while ((currentNode = currentNode.Next) != null);

            foreach (var token in tokens)
                result.Append(token);

            return result.ToString();
        }

        private void CreateTag(LinkedListNode<string> node)
        {
            var tag = Tag.CreateTagOnTextSeparator(new TextSeparator(node.Value, node));
            if (tag is IPairTag)
                openTags.Push(tag);
            else
                node.Value = tag.ToString();
        }

        private bool CloseTag(LinkedListNode<string> node)
        {
            if (openTags.Count == 0) return false;
            var lastOpenTag = openTags.Peek().StartSeparator;
            if (lastOpenTag.Separator != node.Value) return false;
            var tag = Tag.CreateTagOnTextSeparator(lastOpenTag);
            if (openTags.All(t => t is IPairTag pairTag && pairTag.CanIContainThisTagRule(tag))) // todo в текущем контексте это нормально, но если тег не может содержать сам себя работать не будет
            {
                lastOpenTag.Index.Value = ((IPairTag)tag).StartTag;
                node.Value = ((IPairTag)tag).EndTag;
            }
            openTags.Pop();
            return true;
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
