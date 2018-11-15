namespace Markdown
{
    public class TegStrong : Teg
    {
        public TegStrong() => TegRule = new TegRule((t) => true);
        public override string ToString() => "strong";
    }
}
