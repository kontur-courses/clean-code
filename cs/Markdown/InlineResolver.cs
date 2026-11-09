namespace Markdown
{
    public class InlineResolver
    {
        public static InlinePiece[] Resolve(Token[] tokens)
        {
            if (tokens.Length == 0)
                return Array.Empty<InlinePiece>();

            var root = new OpenContainer { Kind = ContainerKind.Root };
            var stack = new List<OpenContainer> { root };

            foreach (var token in tokens)
            {
                switch (token)
                {
                    case TextToken t:
                        AddText(stack, t.Text);
                        break;
                    case EscapedToken e:
                        AddText(stack, e.EscapedChar.ToString());
                        break;
                    case UnderscoreToken u:
                        ProcessUnderscore(stack, u);
                        break;
                    case BracketOpenToken:
                        stack.Add(new OpenContainer { Kind = ContainerKind.Link, OpenerText = "[" });
                        break;
                    case LinkTailToken lt:
                        ProcessLinkTail(stack, lt);
                        break;
                }
            }

            while (stack.Count > 1)
                PopAsLiteral(stack);

            return MergeText(root.Children.ToArray());
        }

        private static void AddText(List<OpenContainer> stack, string text)
        {
            var top = stack[^1];
            top.Children.Add(new TextInlinePiece(text));
            if (text.Any(char.IsWhiteSpace))
                top.HasWhitespace = true;
        }

        private static void ProcessUnderscore(List<OpenContainer> stack, UnderscoreToken u)
        {
            var targetKind = u.Length == 1 ? ContainerKind.Em : ContainerKind.Strong;
            var text = u.Length == 1 ? "_" : "__";
            var top = stack[^1];

            var canOpen = u.Right.HasValue && !char.IsWhiteSpace(u.Right.Value);
            var canClose = u.Left.HasValue && !char.IsWhiteSpace(u.Left.Value);

            // 1) Есть открытый тег и его можно попробовать закрыть
            if (canClose && top.Kind == targetKind)
            {
                if (CanClosePair(top, u))
                {
                    CloseContainer(stack, targetKind);
                    return;
                }

                PopAsLiteral(stack);
                AddText(stack, text);
                return;
            }

            // 2) Непарное закрытие подчёркиваний
            if (canClose && (top.Kind == ContainerKind.Em || top.Kind == ContainerKind.Strong)
                         && top.Kind != targetKind)
            {
                var matchIdx = FindKindInStack(stack, targetKind);
                if (matchIdx >= 0)
                {
                    while (stack.Count > matchIdx)
                        PopAsLiteral(stack);
                    AddText(stack, text);
                    return;
                }
            }

            // 3) Вдруг мы открывашка
            if (canOpen)
            {
                // Внутри одинарного двойное не работает
                if (targetKind == ContainerKind.Strong && ContainsKind(stack, ContainerKind.Em))
                {
                    AddText(stack, text);
                    return;
                }

                stack.Add(new OpenContainer
                {
                    Kind = targetKind,
                    OpenerText = text,
                    OpenerLeft = u.Left,
                    OpenerLeftEscaped = u.LeftEscaped,
                });
                return;
            }

            // 4) Мы никто
            AddText(stack, text);
        }

        private static bool CanClosePair(OpenContainer top, UnderscoreToken closer)
        {
            if (!top.HasWhitespace) return true;
            var openerIsIntraword = IsIntrawordChar(top.OpenerLeft, top.OpenerLeftEscaped);
            var closerIsIntraword = IsIntrawordChar(closer.Right, closer.RightEscaped);
            return !(openerIsIntraword || closerIsIntraword);
        }

        private static bool IsIntrawordChar(char? c, bool isEscaped)
        {
            if (isEscaped) return false;
            return c.HasValue && !char.IsWhiteSpace(c.Value);
        }

        private static void CloseContainer(List<OpenContainer> stack, ContainerKind kind)
        {
            var c = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            var parent = stack[^1];
            var children = MergeText(c.Children.ToArray());

            InlinePiece piece = kind switch
            {
                ContainerKind.Em => new EmInlinePiece(children),
                ContainerKind.Strong => new StrongInlinePiece(children),
                ContainerKind.Link => new LinkInlinePiece(c.Url, children),
                _ => throw new InvalidOperationException(),
            };
            parent.Children.Add(piece);
            if (c.HasWhitespace) parent.HasWhitespace = true;
        }

        private static InlinePiece[] MergeText(InlinePiece[] pieces)
        {
            var result = new List<InlinePiece>();
            foreach (var p in pieces)
            {
                if (p is TextInlinePiece t && result.Count > 0 && result[^1] is TextInlinePiece last)
                    result[^1] = new TextInlinePiece(last.Text + t.Text);
                else
                    result.Add(p);
            }
            return result.ToArray();
        }

        private static void PopAsLiteral(List<OpenContainer> stack)
        {
            var c = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            var parent = stack[^1];
            parent.Children.Add(new TextInlinePiece(c.OpenerText));
            parent.Children.AddRange(c.Children);
            if (c.HasWhitespace || c.OpenerText.Any(char.IsWhiteSpace))
                parent.HasWhitespace = true;
        }

        private static int FindKindInStack(List<OpenContainer> stack, ContainerKind kind)
        {
            for (int i = stack.Count - 1; i > 0; i--)
                if (stack[i].Kind == kind) return i;
            return -1;
        }

        private static bool ContainsKind(List<OpenContainer> stack, ContainerKind kind)
        {
            foreach (var c in stack)
                if (c.Kind == kind) return true;
            return false;
        }

        private static void ProcessLinkTail(List<OpenContainer> stack, LinkTailToken lt)
        {
            int matchIdx = FindKindInStack(stack, ContainerKind.Link);

            // Ссылка оказалась не ссылкой
            if (!lt.Closed || matchIdx < 0)
            {
                if (matchIdx >= 0)
                    while (stack.Count > matchIdx)
                        PopAsLiteral(stack);
                AddText(stack, "](" + lt.Url + (lt.Closed ? ")" : ""));
                return;
            }

            while (stack.Count - 1 > matchIdx)
                PopAsLiteral(stack);

            stack[^1].Url = lt.Url;
            CloseContainer(stack, ContainerKind.Link);
        }
    }

    internal enum ContainerKind { Root, Em, Strong, Link }

    internal sealed class OpenContainer
    {
        public ContainerKind Kind;
        public string OpenerText = "";
        public List<InlinePiece> Children = new();
        public bool HasWhitespace;
        public char? OpenerLeft;
        public bool OpenerLeftEscaped;
        public string Url;
    }
}
