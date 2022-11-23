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

        public void Close()
        {
            isOpen = false;
        }
    }
}
