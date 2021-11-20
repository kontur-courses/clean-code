using System;
using Markdown.Nodes;

namespace Markdown
{
    public class MarkingToNodeParser
    {
        public bool TryGetOpenedNode(Marking marking, out TaggedNode node, out Marking trimmedMarking)
        {
            return TryGetFirstHeaderNode(marking, out node, out trimmedMarking) ||
                   TryGetStrongTaggedNode(marking, out node, out trimmedMarking) ||
                   TryGetEmphasizedTaggedNode(marking, out node, out trimmedMarking);
        }

        private bool TryGetFirstHeaderNode(Marking marking, out TaggedNode node, out Marking trimmedMarking)
        {
            trimmedMarking = null;
            node = null;
            if (!marking.SymbolBeforeMarking.HasValue &&
                marking.StringMarking.StartsWith("#") &&
                marking.SymbolAfterMarking == ' ')
            {
                trimmedMarking = new Marking(
                    marking.SymbolBeforeMarking,
                    String.Empty, 
                    marking.SymbolAfterMarking);
                node = new FirstHeaderTaggedNode();
                return true;
            }

            return false;
        }

        private bool TryGetStrongTaggedNode(Marking marking, out TaggedNode node, out Marking trimmedMarking)
        {
            trimmedMarking = null;
            node = null;
            if (marking.SymbolAfterMarking != ' ' &&
                marking.StringMarking.StartsWith("__"))
            {
                trimmedMarking = new Marking(
                    marking.SymbolBeforeMarking,
                    marking.StringMarking[2..],
                    marking.SymbolAfterMarking);
                node = new StrongTaggedNode();
                return true;
            }

            return false;
        }

        private bool TryGetEmphasizedTaggedNode(Marking marking, out TaggedNode node, out Marking trimmedMarking)
        {
            trimmedMarking = null;
            node = null;
            if (marking.SymbolAfterMarking != ' ' &&
                marking.StringMarking.StartsWith("_"))
            {
                trimmedMarking = new Marking(
                    marking.SymbolBeforeMarking,
                    marking.StringMarking[1..],
                    marking.SymbolAfterMarking);
                node = new EmphasizedTaggedNode();
                return true;
            }

            return false;
        }
    }
}