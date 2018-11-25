namespace Markdown
{
    class StrongRegister : EmphasisRegister
    {
        public StrongRegister()
        {
            suffixLength = 2;
            suffixes = new string[] { "**", "__" };
            priority = 1;
            tags = new string[] { "<strong>", "</strong>" };
        }
    }
}
