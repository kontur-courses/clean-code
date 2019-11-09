namespace Chess
{
    public struct Move
    {
        public Location locFrom;
        public Location locTo;

        private Move(Location locFrom, Location locTo)
        {
            this.locFrom = locFrom;
            this.locTo = locTo;
        }
        
        public static Move Create(Location locFrom, Location locTo) => new Move(locFrom, locTo);
    }
}