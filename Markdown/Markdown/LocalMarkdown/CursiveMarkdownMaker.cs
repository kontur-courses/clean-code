namespace Markdown.LocalMarkdown
{
    internal class CursiveMarkdownMaker : LocalMarkdownMaker
    {
        public bool IsConsistent { get; private set; } = true;

        public CursiveMarkdownMaker(string line, int beginIndex, int endIndex)
        {
            BeginIndex = beginIndex;
            EndIndex = endIndex;
            Line = line;
            CheckConsistency();
        }

        public override void MakeSubstringMarkdown(MarkdownActionType[] actions)
        {
            if (!IsConsistent) return;

            actions[BeginIndex] = MarkdownActionType.OpenCursive;
            actions[EndIndex] = MarkdownActionType.CloseCursive;
        }

        private void CheckConsistency()
        {
            for (int i = BeginIndex + 1; i <= EndIndex; i++)
            {
                if (Line[i] == '_')
                {
                    EndIndex = i;
                    return;
                }
            }

            IsConsistent = false;
        }
    }
}