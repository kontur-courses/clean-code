namespace Markdown.LocalMarkdown
{
    internal class BoldMarkdownMaker : LocalMarkdownMaker
    {
        public bool IsConsistent { get; private set; } = true;

        public BoldMarkdownMaker(string line, int beginIndex, int endIndex)
        {
            BeginIndex = beginIndex;
            EndIndex = endIndex;
            Line = line;
            CheckConsistency();
        }

        public override void MakeSubstringMarkdown(MarkdownActionType[] actions)
        {
            if (!IsConsistent) return;

            actions[BeginIndex] = MarkdownActionType.OpenBold;
            actions[BeginIndex + 1] = MarkdownActionType.NotRendered;
            actions[EndIndex - 1] = MarkdownActionType.NotRendered;
            actions[EndIndex] = MarkdownActionType.CloseBold;
        }

        private void CheckConsistency()
        {
            for (int i = BeginIndex + 3; i <= EndIndex; i++)
            {
                if (Line[i] == '_' && Line[i - 1] == '_')
                {
                    EndIndex = i;
                    return;
                }
            }

            IsConsistent = false;
        }
    }
}