using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Markdown.Md;
using Markdown.Md.TagHandlers;

namespace Markdown
{
    public class Parser : IParser
    {
        private readonly TagHandler tagHandler;
        private readonly ITagConverter tagConverter;
        private int position;
        private string str;

        public Parser(TagHandler tagHandler, ITagConverter tagConverter)
        {
            this.tagHandler = tagHandler;
            this.tagConverter = tagConverter;
        }

        public Tag Parse(string str)
        {
            if (str == null)
            {
                throw new ArgumentException("Given string can't be null", nameof(str));
            }

            this.str = str;
            position = 0;
            return BuildTree(ImmutableStack<TokenNode>.Empty);
        }

        private Tag BuildTree(ImmutableStack<TokenNode> openedTokens)
        {
            var nestedTags = new List<Tag>();

            while (position < str.Length)
            {
                var currentToken = tagHandler.Handle(str, position, openedTokens);
                position += currentToken.Value.Length;

                switch (currentToken.PairType)
                {
                    case TokenPairType.Open:
                        nestedTags.Add(BuildTree(openedTokens.Push(currentToken)));

                        break;
                    case TokenPairType.NotPair:
                        nestedTags.Add(new Tag {Type = currentToken.Type, Value = currentToken.Value});

                        break;
                    case TokenPairType.Close:

                        if (openedTokens.IsEmpty)
                        {
                            return tagConverter.Convert(new Tag
                            {
                                Type = currentToken.Type,
                                Tags = nestedTags,
                                Value = currentToken.Value
                            });
                        }

                        if (openedTokens.Peek()
                            .Type == currentToken.Type)
                        {
                            openedTokens.Pop(out var token);

                            return new Tag
                            {
                                Type = token.Type,
                                Tags = nestedTags,
                                Value = token.Value
                            };
                        }
                        else
                        {
                            nestedTags.Add(
                                tagConverter.Convert(new Tag
                                {
                                    Type = currentToken.Type,
                                    Value = currentToken.Value
                                })
                            );
                        }

                        break;
                }
            }

            return new Tag {Type = "root", Tags = nestedTags};
        }
    }
}