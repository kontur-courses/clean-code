namespace Markdown
{
    public interface IMdHeuristic
    {
        int OpenHeuristicLength { get; }
        bool OpenHeuristic(char[] str);
        int CloseHeuristicLength { get; }
        bool CloseHeuristic(char[] str);
    }
}