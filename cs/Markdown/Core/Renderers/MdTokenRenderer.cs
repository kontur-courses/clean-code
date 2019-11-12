using System;

namespace Markdown
{
    public class MdTokenRenderer
    {        
        public string RenderField(IMdToken field)
        {
            // Пример использования IHTMLTagToken
            if (field.TokenType == MdTokenType.HTMLTag)
            {
                var tag = field as IHTMLTagTokenToken;
                // ...render tag
            }
            throw new NotImplementedException();
        }
    }
}