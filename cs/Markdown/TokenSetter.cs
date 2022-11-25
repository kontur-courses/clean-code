using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;
using Markdown.Exstensions;
using Markdown.Interfaces;
using Markdown.Interfacess;
using Markdown.Tokens;

namespace Markdown
{
    public class TokenSetter:ITokenSetter<TokenType>
    {
        private ITokenBuilder tokenBuilder;
        private ITagCondition<TokenType> tagCondition;
        public TokenSetter(TokenBuilder tokenBuilder, TagCondition tagCondition)
        {
            this.tokenBuilder = tokenBuilder;
            this.tagCondition = tagCondition;
        }

        public void SetToken(List<Token> tokens, TokenType type, ref int index, string line, StringBuilder builder)
        {
            switch (type)
            {
                case TokenType.Text:
                    builder.Append(line[index]);
                    if(index == line.Length-1)
                        ClearStringBuilder(tokens, builder, index);
                    break;
                case TokenType.Slash:
                    if (line[index + 1] != '\\')
                        builder.Append(line[index + 1]);
                    index++;
                    if (index == line.Length - 1)
                        ClearStringBuilder(tokens, builder, index);
                    break;
                case TokenType.Strong:
                    ClearStringBuilder(tokens, builder, index);
                    AddTag(tokens, type, index, index+1);
                    index++;
                    break;
                default:
                    ClearStringBuilder(tokens, builder, index);
                    AddTag(tokens, type, index, index);
                    break;
            }
        }

        public void AddTag(List<Token> tokens, TokenType type, int start, int end)
        {
            var tag = tokenBuilder.GetTag(start, end, type);
            tokens.Add(tag);
        }
        private void ClearStringBuilder(List<Token> tokens, StringBuilder builder, int index)
        {
            if (builder.Length <= 0)
                return;

            var value = builder.ToString();
            builder.Clear();
            var textToken = tokenBuilder.GetText(index - value.Length + 1, value);
            tokens.Add(textToken);
        }

        public void CloseTags(List<Token> tokens)
        {
            CloseTag(tokens, TokenType.Italic);
            CloseTag(tokens, TokenType.Strong);
        }

        private void CloseTag(List<Token> tokens, TokenType type)
        {
            if (tagCondition.GetTagOpenStatus(type))
            {
                var token = tokens.First(x => x.Start == tagCondition.GetOpenIndex(TokenType.Italic));
                var index = tokens.IndexOf(token);
                tokens[index] = new Text(token.Start, token.End, TokenType.Text, tagCondition.GetTag(type));
            }
        }

        public void DeleteEmptyTags(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (i + 1 != tokens.Count && tokens[i] is Tag && tokens[i + 1] is Tag)
                {
                    var tokenFirst = (tokens[i] as Tag);
                    var tokenSecond = (tokens[i + 1] as Tag);
                    if (tokenFirst.Type == tokenSecond.Type && tokenFirst.Status != tokenSecond.Status && tokenFirst.Status == TagStatus.Open)
                    {
                        tokens[i] = TagToText(tokens[i] as Tag);
                        tokens[i + 1] = TagToText(tokens[i + 1] as Tag);
                    }
                }
            }
        }

        public Text TagToText(Tag tag)
        {
            return tokenBuilder.GetText(tag.Start, tagCondition.GetTag(tag.Type));
        }
    }
}
