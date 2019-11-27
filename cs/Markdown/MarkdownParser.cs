using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownParser : IMarkdownParser
    {
        private readonly TokenInfo tokenInfo;
        private List<int> currentPositionsByDepth = new List<int> {0}; // position skips tags length!
        private int depth;
        private RootToken rootToken = new RootToken();
        private Stack<Token> tokensStack = new Stack<Token>();

        public MarkdownParser(TokenInfo tokenInfo)
        {
            this.tokenInfo = tokenInfo;
        }

        public RootToken Parse(string markdown)
        {
            ResetResources();
            var tagBuffer = "";
            var isEscaped = false;
            for (var markdownPosition = 0; markdownPosition < markdown.Length; markdownPosition++)
            {
                var processedToken = GetProcessedToken(rootToken);
                var processedSymbol = markdown[markdownPosition];
                isEscaped = CheckIsEscaped(isEscaped, processedSymbol);
                if (!isEscaped && tokenInfo.IsTagPart(processedSymbol))
                {
                    if (processedToken.IsClosed && tokenInfo.IsOnlyCloseTagPart(processedSymbol))
                        continue;

                    tagBuffer += processedSymbol;

                    if (markdownPosition != markdown.Length - 1 &&
                        tokenInfo.HasTagsStartingWith(tagBuffer + markdown[markdownPosition + 1]))
                        continue;

                    AddTagToStack(processedToken, currentPositionsByDepth[depth], tagBuffer, markdownPosition, markdown);
                    tagBuffer = "";
                }
                else
                {
                    if (isEscaped && processedSymbol == '\\')
                        continue;

                    if (FirstSpacesShouldBeIgnored(processedSymbol, processedToken))
                        continue;

                    processedToken.AppendData(processedSymbol.ToString());
                    currentPositionsByDepth[depth]++;
                }
            }

            return rootToken;
        }

        private bool FirstSpacesShouldBeIgnored(char symbol, Token processedToken)
        {
            return symbol == ' ' && currentPositionsByDepth[depth] == 0
                                 && tokenInfo.IsSpaceAfterOpenTagRequired(processedToken.MdTag);
        }

        private bool CheckIsEscaped(bool isEscaped, char current)
        {
            if (current == '\\')
                return !isEscaped;
            if (char.IsWhiteSpace(current) || !tokenInfo.IsTagPart(current))
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
            if (tokensStack.Count == 0 ||
                tokensStack.Count != 0 && tokenInfo.GetCloseTag(tokensStack.Peek().MdTag) != tagBuffer)
                ProcessOpenTag(processedToken, tokenPosition, tagBuffer, markdownPosition, markdown);
            else if (tokensStack.Count != 0 && tokenInfo.GetCloseTag(tokensStack.Peek().MdTag) == tagBuffer)
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
                if (tokenInfo.IsCorrectNestedTag(parentToken.MdTag, tokenToClose.MdTag))
                    tokenToClose.IsValid = true;

                if (tokenInfo.IsExtraWrappingRequired(tokenToClose.MdTag))
                {
                    currentPositionsByDepth[depth] = 0;
                    depth--;
                }
            }
            else
            {
                parentToken.RemoveLastNestedToken();
                var dataToAppend = tokenToClose.MdTag + tokenToClose.Data + tagBuffer;
                currentPositionsByDepth[depth] += dataToAppend.Length;
                parentToken.AppendData(dataToAppend);
            }
        }

        private void ProcessOpenTag(Token processedToken, int tokenPosition, string tagBuffer, int tagEndPosition,
            string markdown)
        {
            if (IsCorrectOpenTag(tagBuffer, tagEndPosition, markdown))
            {
                var tokenToAdd = new TokenBuilder()
                    .SetPosition(tokenPosition)
                    .SetMdTag(tagBuffer)
                    .SetHtmlTagName(tokenInfo.MdToHtmlTags[tagBuffer])
                    .Build();
                if (tokenInfo.IsExtraWrappingRequired(tagBuffer))
                {
                    tokenToAdd = new TokenBuilder()
                        .SetPosition(0)
                        .SetMdTag(tagBuffer)
                        .SetHtmlTagName(tokenInfo.MdToHtmlTags[tagBuffer])
                        .Build();
                    var wrappingTag = tokenInfo.GetExtraWrappingHtmlTagName(tagBuffer);
                    var wrappingToken = new TokenBuilder()
                        .SetPosition(tokenPosition)
                        .SetMdTag(wrappingTag)
                        .SetHtmlTagName(wrappingTag)
                        .SetIsClosed(true)
                        .SetIsValid(true)
                        .Build();
                    depth++;
                    processedToken.AddNestedToken(wrappingToken);
                    processedToken = wrappingToken;
                }

                tokensStack.Push(tokenToAdd);
                processedToken.AddNestedToken(tokenToAdd);
                depth++;
                AddNewPositionIfNeeded();
            }
            else
            {
                currentPositionsByDepth[depth]++;
                processedToken.AppendData(tagBuffer);
            }
        }

        private bool IsCorrectOpenTag(string tagBuffer, int tagEndPosition, string markdown)
        {
            var previous = GetElementBeforeTag(tagBuffer, tagEndPosition, markdown);
            var next = GetElementAfterTag(tagEndPosition, markdown);
            var correctPrevious = ' ';
            if (tokenInfo.IsNewLineBeforeOpenTagRequired(tagEndPosition,tagBuffer))
                correctPrevious = '\n';
            if (tokenInfo.IsSpaceAfterOpenTagRequired(tagBuffer))
                return previous == correctPrevious && next == ' ';
            return previous == correctPrevious && next != ' ';
        }

        private bool IsCorrectCloseTag(string tagBuffer, int tagEndPosition, string markdown)
        {
            var previous = GetElementBeforeTag(tagBuffer, tagEndPosition, markdown);
            var next = GetElementAfterTag(tagEndPosition, markdown);
            if (tokenInfo.IsSpaceAfterCloseTagRequired(tagBuffer))
                return previous != ' ' && next == ' ';
            return previous != ' ';
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