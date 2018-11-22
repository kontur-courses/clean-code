namespace Markdown
{
    class ParagraphRegister : BaseRegister
    {
        public override Token tryGetToken(ref string input, int startPos)
        {
            string strValue;
            int index = input.IndexOf('\n', startPos);
            
            if (index >= 0)
            {
                strValue = input.Substring(startPos, index - startPos);
            }
            else
            {
                strValue = input.Substring(startPos, input.Length - startPos);
            }

            return new Token(strValue, "<p>", "</p>", 1, strValue.Length + 1, true); 
        }
    }
}
