using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Token
    {
        public int StartPosition { get; set; }
        public int Length { get; set; }
        public List<Token> SubTokens { get; set; }
        public TokenType Type { get; set; }

        /*  Сущность, которая имеет начало (StartPosition) и конец (StartPosition + Length), а также тип (TokenType),
        по которому можно будет понять, как этот токен выделять.

            Token не имеет Value, то есть он не хранит непосредственно текст, а только его границы в исходной строке, 
        проще говоря, это просто разметка с одной особенностью, поторая заключается в SubTokens. 

            Каждый токен может иметь потомков, которые в свою очередь могут иметь своих потомков.
        Это позволит выстроить иерархическое дерево Токенов, которое сделает обработку вложенных тегов гораздо удобнее и понятнее.
        */
    }
}