namespace Chess
{
    public class ChessProblem
    {
        private readonly Board board;

        public ChessProblem(Board board)
        {
            this.board = board;
        }

        // Определяет мат, шах или пат белым.
        public ChessStatus CalculateChessStatus()
        {
            var isCheckOrMate = IsCheckForWhite();
            var hasMoves = false;

            foreach (var whitePiece in board.GetPieces(PieceColor.White))
            {
                var pieceMoves = board.GetPiece(whitePiece).GetMoves(whitePiece, board);
                
                foreach (var whitePieceMove in pieceMoves)
                {
                    var temporaryMove = board.PerformTemporaryMove(whitePiece, whitePieceMove);
                    
                    if (!IsCheckForWhite())
                        hasMoves = true;
                    
                    temporaryMove.Undo();
                }
            }


            if (isCheckOrMate && !hasMoves)
            {
                return ChessStatus.Mate;
            }

            if (isCheckOrMate && hasMoves)
            {
                return ChessStatus.Check;
            }

            return hasMoves ? ChessStatus.Ok : ChessStatus.Stalemate;
        }
        
        // check — это шах
        private bool IsCheckForWhite()
        {
            foreach (var blackPiece in board.GetPieces(PieceColor.Black))
            {
                var piece = board.GetPiece(blackPiece);
                var moves = piece.GetMoves(blackPiece, board);
                
                foreach (var location in moves)
                {
                    if (Piece.Is(board.GetPiece(location), PieceColor.White, PieceType.King))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}