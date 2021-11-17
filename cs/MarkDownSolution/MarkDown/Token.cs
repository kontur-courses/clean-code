namespace MarkDown
{
    public class Token
    {
        bool isItalic;
        bool isBold;
        CharType charType;
        string symbol;
        public void MakeItalic()
        {
            isItalic = true;
        }

        public void MakeBold()
        {
            isBold = true;
        }
    }
}
