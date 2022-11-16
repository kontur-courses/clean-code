namespace Markdown
{
    public class Marker
    {
        private TagFider finder = new();
        private UnpairedTagsRemover remover = new();
        private TagSwitcher switcher = new();

        public string Mark(String text)
        {
            string[] paragraphs = text.Split(
                new string[] { "\r\n" },
                StringSplitOptions.None
            );
            var result = new string[paragraphs.Length];
            for (int i = 0; i<paragraphs.Length-1; i++)
            {
                var tags = finder.CreateTagList(paragraphs[i]);
                tags = remover.FilterTags(tags, paragraphs[i]);
                result[i] = switcher.Switch(tags, paragraphs[i]);
            };
            return string.Join("\r\n", result);
        }
    }
}