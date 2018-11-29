namespace Markdown.TagRendering
{
    public class TagDetecter
    {
        public bool TryDetectOpening(string paragraph, int index)
        {
            return index + 1 < paragraph.Length
                   && paragraph[index + 1] != ' '
                   && !char.IsNumber(paragraph, index + 1);
        }

        public bool TryDetectClosing(string paragraph, int index)
        {
            return index > 0
                   && paragraph[index - 1] != ' '
                   && !char.IsNumber(paragraph, index - 1);
        }

        public bool EmTagDetectedIn(string paragraph, int index) => paragraph[index] == '_';

        public bool StrongTagDetectedIn(string paragraph, int index)
        {
            return index < paragraph.Length - 1
                   && paragraph[index] == '_'
                   && paragraph[index + 1] == '_';
        }

        public bool ShieldedTagDetected(string paragraph, int index)
        {
            return index < paragraph.Length - 1
                   && paragraph[index] == '\\'
                   && paragraph[index + 1] == '_';
        }
    }
}
