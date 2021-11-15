namespace Markdown.Models
{
    public interface ITokenQuery
    {
        public bool IsStart(Context context);
        public bool IsEnd(Context context);
    }
}