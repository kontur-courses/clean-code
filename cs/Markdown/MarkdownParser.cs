using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkdownParser : IMarkdownParser
    {
        private static readonly Dictionary<string, string> MdToHtmlTags = new Dictionary<string, string>
        {
            {"_", "em"},
            {"__", "strong"}
        };

        private static readonly Dictionary<string, List<string>> MdAcceptedNestedTags =
            new Dictionary<string, List<string>>
            {
                {"", new List<string> {"_", "__"}},
                {"_", new List<string> {"_"}},
                {"__", new List<string> {"_", "__"}}
            };

        private bool IsCorrectNestedTag(string parentTag, string nestedTag)
        {
            if (MdAcceptedNestedTags.ContainsKey(parentTag))
                return MdAcceptedNestedTags[parentTag].Contains(nestedTag);
            return false;
        }

        private int GetTagsStartWithWordCount(string word)
        {
            return MdToHtmlTags.Keys.Count(k => k.StartsWith(word));
        }

        public static bool IsTagPart(char symbol)
        {
            return MdToHtmlTags.Keys.Any(k => k.StartsWith(symbol.ToString()));
        }

        private Stack<Token> tokensStack = new Stack<Token>();
        private int depth;
        private RootToken rootToken = new RootToken();
        private List<int> currentPositions = new List<int> {0}; // position skips tags length!

        public RootToken Parse(string markdown)
        {
            ResetResources();
            var tagBuffer = "";
            var isEscaped = false;
            for (var i = 0; i < markdown.Length; i++)
            {
                var processedToken = GetProcessedToken(rootToken);
                var current = markdown[i];
                isEscaped = CheckIsEscaped(isEscaped, current);
                if (!isEscaped && IsTagPart(current))
                {
                    //if (GetTagsStartWithWordCount(tagBuffer + current) != 0)
                    tagBuffer += current;
                    //if (GetTagsStartWithWordCount(tagBuffer) < 1 || !MdToHtmlTags.Keys.Contains(tagBuffer)) 
                    //    continue;
                    if (i != markdown.Length - 1 && GetTagsStartWithWordCount(tagBuffer + markdown[i + 1]) != 0)
                       continue;
                        
                    AddTagToStack(processedToken, currentPositions[depth], tagBuffer);
                    tagBuffer = "";
                }
                else
                {
                    if (isEscaped && current == '\\')
                        continue;
                    currentPositions[depth]++;
                    processedToken.AppendData(current.ToString());
                }
            }

            return rootToken;
        }

        private static bool CheckIsEscaped(bool isEscaped, char current)
        {
            if (current == '\\')
                return !isEscaped;
            if (char.IsWhiteSpace(current) || !IsTagPart(current))
                return false;
            return isEscaped;
        }

        private void ResetResources()
        {
            tokensStack = new Stack<Token>();
            depth = 0;
            rootToken = new RootToken();
            currentPositions = new List<int> {0}; //position skips tags!
        }

        private Token GetProcessedToken(Token rootToken)
        {
            var processedToken = rootToken;
            var d = 0;
            while (d < depth) //todo possible null pointer
            {
                processedToken = processedToken.GetLastNestedToken();
                d++;
            }

            return processedToken;
        }

        private void AddTagToStack(Token processedToken, int position, string tagBuffer)
        {
            if (tokensStack.Count == 0 || (tokensStack.Count != 0 && tokensStack.Peek().MdTag != tagBuffer))
            {
                // opening tag here
                var tokenToAdd = new Token(position, tagBuffer, MdToHtmlTags[tagBuffer]);
                tokensStack.Push(tokenToAdd);
                processedToken.AddNestedToken(tokenToAdd);
                depth++;
                AddNewPositionIfNeeded();
            }
            else if (tokensStack.Count != 0 && tokensStack.Peek().MdTag == tagBuffer)
            {
                // closing tag here
                var tokenToClose = tokensStack.Pop();
                currentPositions[depth] = 0;
                depth--;
                var parentToken = GetProcessedToken(rootToken);
                tokenToClose.IsClosed = true;
                if (IsCorrectNestedTag(parentToken.MdTag, tokenToClose.MdTag))
                    tokenToClose.IsValid = true;
            }
        }

        private void AddNewPositionIfNeeded()
        {
            while (currentPositions.Count - 1 < depth)
                currentPositions.Add(0);
        }
    }
}