using System;
using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;

public record Node(TypeOfNode Type, int Consumed, List<Node>? Descendants = null, string? Value = null);