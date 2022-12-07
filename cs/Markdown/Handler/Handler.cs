using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Handler : IHandler
    {
        public IEnumerable<IToken> Handle(IEnumerable<IToken> tokens)
        {
            var result = HandleTagStates(tokens);
            result = HandleTagInteraction(result);
            result = HandleTagReplacement(result);
            return result;
        }

        private IEnumerable<IToken> HandleTagStates(IEnumerable<IToken> tokens)
        {
            var result = new List<IToken>();
            var stack = new Stack<IToken>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Tag)
                {
                    var tag = Tags.GetOrDefault<MarkdownTag>(token.Content);
                    var tagState = GetTagState(token, stack);
                    var newToken = new Token(token.Type, token.Content, tagState);

                    stack.TryPeek(out var tokenPeek);
                    var tagPeek = Tags.GetOrDefault<MarkdownTag>(tokenPeek?.Content);

                    if (tagState == TagState.Opening)
                        stack.Push(newToken);
                    if (tagState == TagState.Closing)
                        stack.Pop();

                    stack.TryPeek(out var currentTokenPeek);
                    var currentTagPeek = Tags.GetOrDefault<MarkdownTag>(currentTokenPeek?.Content);

                    if (tagPeek != null && tag != tagPeek && !tagPeek.CanNesting ||
                        currentTagPeek != null && tag != currentTagPeek && !currentTagPeek.CanNesting)
                    {
                        result.Add(new Token(TokenType.Text, token.Content));
                        continue;
                    }
                    result.Add(newToken);
                    continue;
                }
                result.Add((IToken)token.Clone());
            }

            foreach (var token in result)
            {
                if (stack.Contains(token))
                    yield return new Token(TokenType.Text, token.Content);
                else yield return token;
            }
        }

        public TagState GetTagState(IToken token, Stack<IToken> stack)
        {
            var tag = Tags.GetOrDefault<MarkdownTag>(token.Content);
            var result = TagState.Undefined;

            if (token.Content.IsTag<MarkdownTag>(TagState.Opening))
                result = TagState.Opening;
            if (stack.TryPeek(out var t) && t.Content.IsTag(tag))
                result = TagState.Closing;
            if (tag.IsSelfClosing)
                result = TagState.SelfClosing;

            return result;
        }

        private IEnumerable<IToken> HandleTagReplacement(IEnumerable<IToken> tokens)
        {
            var stack = new Stack<string>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Tag)
                {
                    var tag = Tags.GetOrDefault<MarkdownTag>(token.Content);
                    var replacementTag = Rules.GetReplacement<MarkdownTag, HTMLTag>(tag);
                    var replacement = replacementTag.Opening;

                    if (token.TagState == TagState.Closing)
                        replacement = replacementTag.Closing;
                    if (token.TagState == TagState.SelfClosing)
                        stack.Push(replacementTag.Closing);

                    yield return new Token(token.Type, replacement, token.TagState);
                    continue;
                }
                if (token.Type == TokenType.LineBreak || token.Type == TokenType.End)
                {
                    if (stack.TryPop(out var replacement))
                        yield return new Token(token.Type, replacement, token.TagState);
                }
                yield return (IToken)token.Clone();
            }
        }

        private IEnumerable<IToken> HandleTagInteraction(IEnumerable<IToken> tokens)
        {
            var incorrectTokens = new List<IToken>();
            var tokensList = tokens.Clone().ToList();
            for (var i = 0; i < tokensList.Count; i++)
            {
                if (tokensList[i].Type == TokenType.Tag && tokensList[i].TagState == TagState.Opening)
                {
                    var tag = Tags.GetOrDefault<MarkdownTag>(tokensList[i].Content);
                    var nesting = string.Empty;
                    for (var j = i + 1; j < tokensList.Count; j++)
                    {
                        if (tokensList[j].Content.IsTag(tag) &&
                            tokensList[j].TagState == TagState.Closing)
                        {
                            var left = tokensList[i - 1].Type == TokenType.Text
                                ? tokensList[i - 1].Content.Last() : default;
                            var right = tokensList[j + 1].Type == TokenType.Text
                                ? tokensList[j + 1].Content.First() : default;
                            if (nesting != string.Empty)
                            if (nesting.All(ch => char.IsDigit(ch)) ||
                                char.IsWhiteSpace(nesting.First()) ||
                                char.IsWhiteSpace(nesting.Last()) ||
                                ((char.IsLetter(left) || 
                                char.IsLetter(right)) && 
                                nesting.Contains(' ')))
                            {
                                incorrectTokens.AddRange(new[] { tokensList[i], tokensList[j] });
                            }
                            break;
                        }
                        nesting += tokensList[j].Content;
                    }
                }
            }

            foreach (var token in tokensList)
            {
                if (incorrectTokens.Contains(token))
                    yield return new Token(TokenType.Text, token.Content);
                else yield return token;
            }
        }
    }
}