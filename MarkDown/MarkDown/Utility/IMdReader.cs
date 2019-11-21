namespace MarkDown
{
    public interface IMdReader
    {
        char LookAhead(int offset = 0);
        char LookBehind(int offset = 1);
        void ShiftPointer(int amountOfChars = 1);
        char[] LookNextChars(int n);
        char[] LookPreviousChars(int n);
        bool HasNext();
        bool HasNextChars(int n);
        bool HasPrevious();
        bool HasPreviousChars(int n);
    }
}