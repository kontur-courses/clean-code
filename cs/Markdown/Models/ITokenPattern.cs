namespace Markdown.Models
{
    public interface ITokenPattern
    {
        public bool IsStart(Context context);
        public bool IsEnd(Context context);
    }
}