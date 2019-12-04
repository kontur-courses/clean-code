namespace MarkdownProcessor.Wraps
{
    public abstract class WrapType : IWrapType
    {
        public abstract string OpenWrapMarker { get; }
        public abstract string CloseWrapMarker { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is WrapType wrapType && Equals(wrapType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((OpenWrapMarker != null ? OpenWrapMarker.GetHashCode() : 0) * 397) ^
                       (CloseWrapMarker != null ? CloseWrapMarker.GetHashCode() : 0);
            }
        }

        private bool Equals(IWrapType other) => other.OpenWrapMarker == OpenWrapMarker &&
                                                other.CloseWrapMarker == CloseWrapMarker;
    }
}