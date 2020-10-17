using System;

public class AI
{
    public int BestMove(BoardCell[] mainboard, int depth) 
    {
        int score = 0, bestScore = -100, bestMoveIndex = -1;
        for (int index = 0; index < depth; index++)
        {
            if (mainboard[index].CellState == CellStatus.Empty)
            {
                mainboard[index].CellState = CellStatus.AIMarked;
                GameStatus status = new GameStatus(mainboard);
                if (status.IsGameOver)
                    return index;

                score = Minimax(false, mainboard, int.MinValue, int.MaxValue);
                mainboard[index].CellState = CellStatus.Empty;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMoveIndex = index;
                }
            }
        }
        return bestMoveIndex;
    }


    public int Minimax(bool isAIPlaying, BoardCell[] board, int alpha, int beta)
    {
        GameStatus status = new GameStatus(board);
        if (status.IsGameOver)
            return status.Score;

        int score;
        if (isAIPlaying)
        {
            for (int index = 0; index < 9; index++)
            {
                if (board[index].CellState == CellStatus.Empty)
                {
                    board[index].CellState = CellStatus.AIMarked;
                    score = Minimax(false, board, alpha, beta);
                    board[index].CellState = CellStatus.Empty;

                    if (score > alpha)
                        alpha = score;

                    if (alpha > beta)
                        break;
                }
            }
            return alpha;
        }
        else
        {
            for (int index = 0; index < 9; index++)
            {
                if (board[index].CellState == CellStatus.Empty)
                {
                    board[index].CellState = CellStatus.UserMarked;
                    score = Minimax(true, board, alpha, beta);
                    board[index].CellState = CellStatus.Empty;

                    if (score < beta)
                        beta = score;

                    if (alpha > beta)
                        break;
                }
            }
            return beta;
        }
    }

    public int RandomMove()
    {
        return new Random().Next(0, 9);
    }
}
