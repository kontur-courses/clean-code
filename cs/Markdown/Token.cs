namespace Markdown
{
    public class Token
    {
        public int startInd;
        public int endInd;
        public Mod modType;
        public bool IsOpen;

        public Token(int startInd, int endInd, Mod modType = Mod.Common, bool IsOpen = true)
        {
            this.startInd = startInd;
            this.endInd = endInd;
            this.modType = modType;
            this.IsOpen = IsOpen;
        }

        public void Close()
        {
            IsOpen = false;
        }
    }
}
