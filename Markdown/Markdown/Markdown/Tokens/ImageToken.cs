namespace Markdown.Tokens
{
    public class ImageToken : Token
    {
        public int Position { get; set; }
        public int Length { get; set; }
        public TokenType Type { get; set; }

        public string DescriptionForImage;
        public string PathForImage;

        public ImageToken(int position, string description, string path) : base(position, description, path)
        {
            Position = position;
            Length = description.Length + path.Length;
            PathForImage = path;
            DescriptionForImage = description;
            Type = TokenType.Image;
        }
    }
}
