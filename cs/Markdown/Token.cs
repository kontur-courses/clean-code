namespace Markdown
{
    public class Token
    {
        public readonly int StartInd;
        public readonly int EndInd;
        public Mod modType;
        public bool isOpen;

        public Token(int startInd, int endInd, Mod modType = Mod.Common, bool IsOpen = true)
        {
            this.StartInd = startInd;
            this.EndInd = endInd;
            this.modType = modType;
            this.isOpen = IsOpen;
        }

        public Token(){}

        public void Close()
        {
            isOpen = false;
        }

        public void ChangeModType(Mod modType)
        {
            this.modType = modType;
        }

        public void CastToCommonType()
        {
            Close();
            ChangeModType(Mod.Common);
        }
    }
}
