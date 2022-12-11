using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    internal class AutomatonPartStarter : ITransitionFunctionValue
    {
        public string PartName { get; }
        public string ParentPartName { get; }
        public int ParentPartStateIndexToReturn { get; }
        public string[] NewStackElements { get; }

        public AutomatonPartStarter(
            string partName, 
            string parentPartName, 
            int parentPartStateIndexToReturn,
            string[] newStackElements)
        {
            PartName = partName;
            ParentPartName = parentPartName;
            ParentPartStateIndexToReturn = parentPartStateIndexToReturn;
            NewStackElements = newStackElements;
        }

        public AutomatonPartStarter(
            string partName,
            string parentPartName,
            int parentPartStateIndexToReturn,
            string newStackElement)
        {
            PartName = partName;
            ParentPartName = parentPartName;
            ParentPartStateIndexToReturn = parentPartStateIndexToReturn;
            NewStackElements = new[] { newStackElement };
        }
    }
}
