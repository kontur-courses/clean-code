using ConsoleApplication1.ConnectionHandlers.DataHandlers;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Extensions
{
    public static class ItemsConnecterExtensions
    {
        public static void AddStrengthsWithSameDirection(this ItemsConnecter connecter, int[] strenghts, Direction direction)
        {
            foreach (var strength in strenghts)
                connecter.AddItem(strength, direction);
        }
    }
}
