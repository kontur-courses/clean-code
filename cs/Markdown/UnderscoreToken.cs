using System.Collections.Generic;

namespace Markdown
{
    /// <summary>
    /// Cейчас данный класс хранит функциональность для "_" и "__" одновременно. Ошибка, да, сейчас уже не успеваю поправить
    /// </summary>
    public sealed class UnderscoreToken : Token
    {
        public UnderscoreToken(int position, int length, string value)
        {
            Position = position;
            Length = length;
            Value = value;
        }

        public override int Position { get; set; }
        public override int Length { get; set; }
        public override string Value { get; set; }
        public override List<Token> InnerTokens { get; set; }
        public override Token ParentToken { get; set; }
        public override string ToHtml()
        {

            if (InnerTokens == null || InnerTokens.Count == 0)
                return Value[1] == '_' ? $"<strong>{Value.Substring(2, Value.Length - 4)}</strong>" : $"<em>{Value.Substring(1, Value.Length - 2)}</em>";
            var text = Value;
            var single = Value[1] != '_';
            foreach (var innerToken in InnerTokens)
            {
                var htmlToken = innerToken.ToHtml();
                text = text.Substring(0, innerToken.Position) +
                       htmlToken +
                       text.Substring(innerToken.Position + innerToken.Length);

            }
            text = single ? text.Substring(1, text.Length - 2) : text.Substring(2, text.Length - 3);
            text = single ? $"<em>{text}</em>" : $"<strong>{text}</strong>";
            return text;
        }
    }
}