using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;
using Markdown.Exstensions;
using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown
{
    public class TokenBuilder:ITokenBuilder
    {
        private readonly ITagCondition<TokenType> condition;

        public TokenBuilder(ITagCondition<TokenType> condition)
        {
            this.condition = condition;
        }
        public Tag GetTag(int start, int end, TokenType type)
        {
            TagStatus status;
            if (condition.GetTagOpenStatus(type))
            {
                status = TagStatus.Close;
                condition.CloseTag(type);
            }
            else
            {
                status = TagStatus.Open;
                condition.OpenTag(type, start);
            }

            return new Tag(start, end, type, status);
        }

        public Text GetText(int start, string value)
        {
            return new Text(start, start + value.Length - 1, TokenType.Text, value);
        }
    }
}
