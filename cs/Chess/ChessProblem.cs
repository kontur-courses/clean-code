using System.Linq;

namespace Chess
{
    public class ChessProblem
    {
        private Board board;
        public  ChessStatus ChessStatus;

        
        public void LoadFrom(string[] lines)
        {
            board = new BoardParser().ParseBoard(lines);
        }
        

        // Определяет мат, шах или пат разным.
        public void CalculateChessStatus(PieceColor color)
        {
            //board = newBoard;
            var isCheck = IsCheckForKing(color);
            var hasMoves = HasMoves(color);
            
            if (isCheck)
            {
                if (hasMoves)
                    ChessStatus = ChessStatus.Check;
                else ChessStatus = ChessStatus.Mate;
            }
            else if (hasMoves) ChessStatus = ChessStatus.Ok;
            else ChessStatus = ChessStatus.Stalemate;
        }

        private bool HasMoves(PieceColor color)
        {    
            
            foreach (var locFrom in board.GetPieces(color))
            {
                foreach (var locTo in board.GetPiece(locFrom).GetMoves(locFrom, board))
                {
                    var old = board.GetPiece(locTo);
                    bool result;
                    //var temp = new TemporaryPieceMove(board, locFrom, locTo, old);

                    using (var tmp = new TemporaryPieceMove(board, locFrom, locTo, old))
                    {
                        tmp.
                        result = !IsCheckForKing(color);
                    }
                    if (result)
                        return true;
                }
            }
            return false;
            
        }

        // check — это шах
        private bool IsCheckForKing(PieceColor color)
        {
            var enemyColor = 1 - color;

            return board.GetPieces(enemyColor).Select(loc =>
            {
                var piece = board.GetPiece(loc);
                var moves = piece.GetMoves(loc, board);
                return moves.Any(dest => Piece.Is(board.GetPiece(dest), color, PieceType.King));
            }).Any(x => x);

            //foreach (var loc in board.GetPieces(enemyColor))
            //{
            //    var piece = board.GetPiece(loc);
            //    var moves = piece.GetMoves(loc, board);
            //    foreach (var destination in moves)
            //    {
            //        if (Piece.Is(board.GetPiece(destination),
            //                     color, PieceType.King))
            //            return true;
            //    }
            //}           
            //return false;
        }
    }
}