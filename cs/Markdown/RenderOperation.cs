using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class RenderOperation
    {
        private readonly Dictionary<int, string> buffer = new Dictionary<int, string>();
        private readonly HashSet<string> openedTags = new HashSet<string>();

        private readonly string text;
        private int currentIndex;

        public RenderOperation(string text)
        {
            this.text = text;
        }

        public string Result { get; private set; }

        public void Process()
        {
            var resultBuilder = new StringBuilder();
            var screened = false;
            while (currentIndex < text.Length || buffer.Count > 0)
                switch (GetRenderStatus())
                {
                    case RenderStatus.CheckScreening:
                        var slashCount = GetSlashesCount();
                        screened = slashCount % 2 != 0;
                        resultBuilder.Append(new string('\\', slashCount - slashCount % 2));
                        currentIndex += slashCount;
                        break;

                    case RenderStatus.AppendClosingTag:
                        AppendFromBuffer(currentIndex, resultBuilder);
                        break;

                    case RenderStatus.CheckMarkSymbol:
                        var tag = TagBuilder.BuildTag(text, currentIndex);
                        switch (GetTagStatus(tag, screened))
                        {
                            case TagStatus.Screened:
                                screened = false;
                                buffer[tag.OpenPosition] = tag.Mark;
                                buffer[tag.ClosePosition - tag.Mark.Length + 1] = tag.Mark;
                                break;

                            case TagStatus.Correct:
                                buffer[tag.OpenPosition] = $"<{tag.TagName}>";
                                buffer[tag.ClosePosition - tag.Mark.Length + 1] = $"</{tag.TagName}>";
                                openedTags.Add(tag.TagName);
                                break;

                            case TagStatus.Incorrect:
                                var mark = Marks.GetMarkFromText(currentIndex, text);
                                resultBuilder.Append(mark);
                                currentIndex += mark.Length;
                                break;
                        }

                        break;

                    case RenderStatus.PlainText:
                        resultBuilder.Append(text[currentIndex]);
                        currentIndex++;
                        break;
                }

            Result = resultBuilder.ToString();
        }

        private RenderStatus GetRenderStatus()
        {
            if (buffer.ContainsKey(currentIndex))
                return RenderStatus.AppendClosingTag;

            if (Marks.ExpectedToBeMark(currentIndex, text))
                return RenderStatus.CheckMarkSymbol;

            if (currentIndex < text.Length && text[currentIndex] == '\\'
                                           && Marks.ExpectedToBeMark(currentIndex + 1, text))
                return RenderStatus.CheckScreening;

            return RenderStatus.PlainText;
        }

        private TagStatus GetTagStatus(Tag tag, bool isScreened)
        {
            if (isScreened)
                return TagStatus.Screened;
            if (tag.isCorrect && tag.TagName == "strong" && openedTags.Contains("em"))
                return TagStatus.Incorrect;

            return tag.isCorrect ? TagStatus.Correct : TagStatus.Incorrect;
        }

        private void AppendFromBuffer(int position, StringBuilder builderToAppend)
        {
            var value = buffer[position];
            builderToAppend.Append(value);
            buffer.Remove(position);

            currentIndex += Marks.IsHtmlTag(value) ? Marks.GetMarkByHtmlTag(value).Length : value.Length;
        }

        private int GetSlashesCount()
        {
            var count = 0;
            var i = currentIndex;
            while (i < text.Length && text[i] == '\\')
            {
                count++;
                i++;
            }

            return count;
        }

        private enum RenderStatus
        {
            AppendClosingTag,
            CheckMarkSymbol,
            CheckScreening,
            PlainText
        }

        private enum TagStatus
        {
            Screened,
            Incorrect,
            Correct
        }
    }
}