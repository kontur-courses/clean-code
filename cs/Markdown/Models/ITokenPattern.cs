namespace Markdown.Models
{
    public interface ITokenPattern
    {
        public int TagLength { get; }
        public bool LastCloseSucceed { get; }
        public bool TrySetStart(Context context);
        public bool CanContinue(Context context);
    }
}