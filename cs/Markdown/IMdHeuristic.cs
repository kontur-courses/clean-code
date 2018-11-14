namespace Markdown
{
    public interface IMdHeuristic
    {
        int OpenHeuristic(int index);
        int CloseHeuristic(int index);
    }
}