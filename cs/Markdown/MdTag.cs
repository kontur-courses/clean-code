namespace Markdown
{
    public class MdTag : ITag
    {
        public string Name { get; }

        public MdTag(string name)
        {
            Name = name;
        }

        public string GetClosing()
        {
            throw new System.NotImplementedException();
        }

        public string GetOpener()
        {
            throw new System.NotImplementedException();
        }

        public string EncloseInTags(string line)
        {
            throw new System.NotImplementedException();
        }
    }
}