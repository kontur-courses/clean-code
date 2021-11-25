namespace Markdown.TokenizerLogic
{
    internal class PairedTokenInfo
    {
        public readonly PairedToken Token;
        public bool CanOpen { get; private set; }
        public bool CanClose { get; private set; }

        public PairedTokenInfo(PairedToken token)
        {
            Token = token;
        }

        public bool IsSameType(PairedToken other)
        {
            return Token.GetType() == other.GetType();
        }

        public void Open() => CanOpen = true;

        public void DisableOpen() => CanOpen = false;

        public void Close() => CanClose = true;

        public void DisableClose() => CanClose = false;
    }
}
