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

        private List<int> currentPositionsByDepth = new List<int> {0}; // position skips tags length!
        private int depth;
        private RootToken rootToken = new RootToken();
        private Stack<Token> tokensStack = new Stack<Token>();

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
                    tagBuffer += current;
                    if (i != markdown.Length - 1 && GetTagsStartWithWordCount(tagBuffer + markdown[i + 1]) != 0)
                        continue;

                    AddTagToStack(processedToken, currentPositionsByDepth[depth], tagBuffer, i, markdown);
                    tagBuffer = "";
                }
                else
                {
                    if (isEscaped && current == '\\')
                        continue;
                    currentPositionsByDepth[depth]++;
                    processedToken.AppendData(current.ToString());
                }
            }

            return rootToken;
        }

        private static bool IsTagPart(char symbol)
        {
            return MdToHtmlTags.Keys.Any(k => k.StartsWith(symbol.ToString()));
        }

        private static bool IsCorrectNestedTag(string parentTag, string nestedTag)
        {
            return MdAcceptedNestedTags.ContainsKey(parentTag) && MdAcceptedNestedTags[parentTag].Contains(nestedTag);
        }

        private static int GetTagsStartWithWordCount(string word)
        {
            return MdToHtmlTags.Keys.Count(k => k.StartsWith(word));
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
            currentPositionsByDepth = new List<int> {0}; //position skips tags!
        }

        private Token GetProcessedToken(Token rootToken)
        {
            var processedToken = rootToken;
            var depthCounter = 0;
            while (depthCounter < depth)
            {
                processedToken = processedToken.GetLastNestedToken();
                depthCounter++;
            }

            return processedToken;
        }

        private void AddTagToStack(Token processedToken, int tokenPosition, string tagBuffer, int markdownPosition,
            string markdown)
        {
            if (tokensStack.Count == 0 || tokensStack.Count != 0 && tokensStack.Peek().MdTag != tagBuffer)
                ProcessOpenTag(processedToken, tokenPosition, tagBuffer, markdownPosition, markdown);
            else if (tokensStack.Count != 0 && tokensStack.Peek().MdTag == tagBuffer)
                ProcessCloseTag(tagBuffer, markdownPosition, markdown);
        }

        private void ProcessCloseTag(string tagBuffer, int tagEndPosition, string markdown)
        {
            var tokenToClose = tokensStack.Pop();
            currentPositionsByDepth[depth] = 0;
            depth--;
            var parentToken = GetProcessedToken(rootToken);
            if (IsCorrectCloseTag(tagBuffer, tagEndPosition, markdown))
            {
                tokenToClose.IsClosed = true;
                if (IsCorrectNestedTag(parentToken.MdTag, tokenToClose.MdTag))
                    tokenToClose.IsValid = true;
            }
            else
            {
                parentToken.RemoveLastNestedToken();
                parentToken.AppendData(tokenToClose.MdTag + tokenToClose.Data + tagBuffer);
            }
        }

        private void ProcessOpenTag(Token processedToken, int tokenPosition, string tagBuffer, int tagEndPosition,
            string markdown)
        {
            if (IsCorrectOpenTag(tagBuffer, tagEndPosition, markdown))
            {
                var tokenToAdd = new Token(tokenPosition, tagBuffer, MdToHtmlTags[tagBuffer]);
                tokensStack.Push(tokenToAdd);
                processedToken.AddNestedToken(tokenToAdd);
                depth++;
                AddNewPositionIfNeeded();
            }
            else
            {
                processedToken.AppendData(tagBuffer);
            }
        }

        private static bool IsCorrectOpenTag(string tagBuffer, int tagEndPosition, string markdown)
        {
            var previous = GetElementBeforeTag(tagBuffer, tagEndPosition, markdown);
            var next = GetElementAfterTag(tagEndPosition, markdown);
            return previous == ' ' && next != ' ';
        }

        private static bool IsCorrectCloseTag(string tagBuffer, int tagEndPosition, string markdown)
        {
            var previous = GetElementBeforeTag(tagBuffer, tagEndPosition, markdown);
            var next = GetElementAfterTag(tagEndPosition, markdown);
            return previous != ' ' && next == ' ';
        }

        private void AddNewPositionIfNeeded()
        {
            while (currentPositionsByDepth.Count - 1 < depth)
                currentPositionsByDepth.Add(0);
        }

        private static char GetElementAfterTag(int tagEndIndex, string str)
        {
            return tagEndIndex >= str.Length - 1
                ? ' '
                : str[tagEndIndex + 1];
        }

        private static char GetElementBeforeTag(string tagBuffer, int tagEndIndex, string str)
        {
            return tagEndIndex - tagBuffer.Length + 1 <= 0
                ? ' '
                : str[tagEndIndex - tagBuffer.Length];
        }
    }
}