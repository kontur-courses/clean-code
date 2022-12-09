using Markdown.Enums;
using Markdown.Extensions;
using Markdown.Tag;
using Markdown.TokenNamespace;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.HandlerNamespace
{
    public class Handler : IHandler
    {
        public IEnumerable<IToken> Handle(IEnumerable<IToken> tokens)
        {
            var handlers = new Func<IEnumerable<IToken>, IEnumerable<IToken>>[]
            {
                HandleEscapeTokens,
                HandleTagStates,
                HandleTagInteraction,
                HandleTagReplacement,
                HandleSelfClosingTags
            };

            return handlers.Aggregate(tokens.Clone(), (acc, next) => next(acc));
        }

        private static IEnumerable<IToken> HandleTagStates(IEnumerable<IToken> tokens)
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
                    
                    switch (tagState)
                    {
                        case TagState.Opening: stack.Push(newToken); break;
                        case TagState.Closing: stack.Pop(); break;
                    }

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
            return result.SelectWhere(t => new Token(TokenType.Text, t.Content),
                t => stack.Contains(t));
        }

        private static TagState GetTagState(IToken token, Stack<IToken> stack)
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

        private static IEnumerable<IToken> HandleTagReplacement(IEnumerable<IToken> tokens)
        {
            var stack = new Stack<string>();
            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Tag:
                    {
                        var tag = Tags.GetOrDefault<MarkdownTag>(token.Content);
                        var replacementTag = Rules.GetReplacement<MarkdownTag, HtmlTag>(tag);
                        var replacement = replacementTag.Opening;
                        switch (token.TagState)
                        {
                            case TagState.Closing: replacement = replacementTag.Closing; break;
                            case TagState.SelfClosing: stack.Push(replacementTag.Closing); break;
                        }
                        yield return new Token(token.Type, replacement, token.TagState);
                        continue;
                    }
                    case TokenType.LineBreak:
                    case TokenType.End:
                    {
                        if (stack.TryPop(out var replacement))
                            yield return new Token(token.Type, replacement, token.TagState);
                        break;
                    }
                }
                yield return (IToken)token.Clone();
            }
        }

        private static IEnumerable<IToken> HandleTagInteraction(IEnumerable<IToken> tokens)
        {
            var incorrectTokens = new List<IToken>();
            var tokensList = tokens.Clone().ToList();
            for (var i = 0; i < tokensList.Count; i++)
            {
                if (!tokensList[i].Content.IsTag<MarkdownTag>() ||
                    tokensList[i].TagState != TagState.Opening) continue;
                var tag = Tags.GetOrDefault<MarkdownTag>(tokensList[i].Content);
                var nesting = string.Empty;
                for (var j = i + 1; j < tokensList.Count; j++)
                {
                    if (tokensList[j].Content.IsTag<MarkdownTag>(TagState.Closing))
                    {
                        var prev = tokensList[i - 1].Content.LastOrDefault();
                        var next = tokensList[j + 1].Content.FirstOrDefault();
                        if (Rules.Interactions
                            .Any(interaction => interaction(nesting, prev, next)))
                        {
                            incorrectTokens.AddRange(new[] { tokensList[i], tokensList[j] });
                        }
                        break;
                    }
                    nesting += tokensList[j].Content;
                }
            }
            return tokensList.SelectWhere(t => new Token(TokenType.Text, t.Content),
                t => incorrectTokens.Contains(t));
        }

        private static IEnumerable<IToken> HandleEscapeTokens(IEnumerable<IToken> tokens)
        {
            var tokensList = tokens.Clone().ToList();
            var skip = false;
            for (var i = 0; i < tokensList.Count; i++)
            {
                var token = tokensList[i];
                var content = token.Content;
                if (skip)
                {
                    skip = false;
                    yield return new Token(TokenType.Text, content);
                    continue;
                }
                if (token.Type == TokenType.Escape)
                {
                    var nextToken = tokensList[i + 1];
                    if (nextToken.Type == TokenType.Tag || nextToken.Type == TokenType.Escape)
                    {
                        skip = true;
                        content = string.Empty;
                    }
                    yield return new Token(TokenType.Text, content);
                    continue;
                }
                yield return (IToken)token.Clone();
            }
        }

        private static IEnumerable<IToken> HandleSelfClosingTags(IEnumerable<IToken> tokens)
        {
            var prevToken = default(IToken);
            foreach (var token in tokens)
            {
                var content = token.Content;
                if (prevToken is { TagState: TagState.SelfClosing })
                    content = token.Content.Trim(' ');
                prevToken = token;
                yield return new Token(token.Type, content, token.TagState);
            }
        }
    }
}