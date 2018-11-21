namespace Markdown
{
    public class MarkerPair
    {
        public Marker Opener { get; set; }
        public Marker Closer { get; set; }

        public MarkerPair(Marker opener, Marker closer)
        {
            Opener = opener;
            Closer = closer;
        }
    }
}
