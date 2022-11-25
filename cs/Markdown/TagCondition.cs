using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;
using Markdown.Exstensions;

namespace Markdown
{
    public class TagCondition:ITagCondition<TokenType>
    {
        private readonly Dictionary<TokenType, bool> tagIsOpen = new Dictionary<TokenType, bool>()
        {
            [TokenType.Strong] = false,
            [TokenType.Italic] = false,
            [TokenType.Header] = false
        };

        private readonly Dictionary<TokenType, int> tagOpenIndex = new Dictionary<TokenType, int>()
        {
            [TokenType.Italic] = 0,
            [TokenType.Strong] = 0,
            [TokenType.Header] = 0
        };

        private readonly Dictionary<TokenType, string> tagStrings = new Dictionary<TokenType, string>()
        {
            [TokenType.Italic] = "_",
            [TokenType.Strong] = "__"
        };


        public void SetOpenIndex(TokenType type, int openIndex) => tagOpenIndex[type] = openIndex;

        public void SetTagOpenStatus(TokenType type, bool value) => tagIsOpen[type] = value;

        public int GetOpenIndex(TokenType type) => tagOpenIndex[type];

        public bool GetTagOpenStatus(TokenType type) => tagIsOpen[type];
        public void OpenTag(TokenType type, int index)
        {
            tagIsOpen[type] = true;
            tagOpenIndex[type] = index;
        }
        public void CloseTag(TokenType type)
        {
            tagIsOpen[type] = false;
        }

        public string GetTag(TokenType type)
        {
            return tagStrings[type];
        }
    }
}
