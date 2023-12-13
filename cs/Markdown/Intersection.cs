namespace Markdown
{
    public class Intersection
    {
        private string openParentTag;

        public Intersection(string openParentTag)
        {
            this.openParentTag = openParentTag;
        }

        public bool HaveIntersection(string markdownText, string currentOpenTag, int index)
        {
            return false;
        }
    }
}