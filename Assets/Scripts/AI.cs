using System.Linq;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System;

public class AI 
{
    public int BestMove { get; set; } = -1; // invalid index as start

    private const string baseUrl = "https://apiaiminimax.azurewebsites.net";

    // use delegate to notify the ienumerator has finished
    public IEnumerator BestMoveCoroutine(BoardCell[] mainboard, int depth, Action onComplete = null)
    {
        string board = $"[{string.Join(",", mainboard.Select(c => c.ToString()))}]";

        using (UnityWebRequest webService = UnityWebRequest.Get($"{baseUrl}/api/ai/getbestmove?mainboardJson={board}&depth={depth}"))
        {
            yield return webService.SendWebRequest();

            if (webService.isNetworkError || webService.isHttpError)
                Debug.Log(webService.error);
            else
            {
                // Show results as text
                //Debug.Log(webService.downloadHandler.text);
                BestMove = int.Parse(webService.downloadHandler.text);
            }
        }

        onComplete?.Invoke();
        #region local AI function calling bestMove
        //int score = 0, bestScore = -100, bestMoveIndex = -1;
        //for (int index = 0; index < depth; index++)
        //{
        //    if (mainboard[index].CellState == CellStatus.Empty)
        //    {
        //        mainboard[index].CellState = CellStatus.AIMarked;
        //        GameStatus status = new GameStatus(mainboard);
        //        if (status.IsGameOver)
        //            return index;

        //        score = Minimax(false, mainboard, int.MinValue, int.MaxValue);
        //        mainboard[index].CellState = CellStatus.Empty;

        //        if (score > bestScore)
        //        {
        //            bestScore = score;
        //            bestMoveIndex = index;
        //        }
        //    }
        //}
        //return bestMoveIndex;
        #endregion
    }

    #region Local funciton Minimax
    //public int Minimax(bool isAIPlaying, BoardCell[] board, int alpha, int beta)
    //{
    //    GameStatus status = new GameStatus(board);
    //    if (status.IsGameOver)
    //        return status.Score;

    //    int score;
    //    if (isAIPlaying)
    //    {
    //        for (int index = 0; index < 9; index++)
    //        {
    //            if (board[index].CellState == CellStatus.Empty)
    //            {
    //                board[index].CellState = CellStatus.AIMarked;
    //                score = Minimax(false, board, alpha, beta);
    //                board[index].CellState = CellStatus.Empty;

    //                if (score > alpha)
    //                    alpha = score;

    //                if (alpha > beta)
    //                    break;
    //            }
    //        }
    //        return alpha;
    //    }
    //    else
    //    {
    //        for (int index = 0; index < 9; index++)
    //        {
    //            if (board[index].CellState == CellStatus.Empty)
    //            {
    //                board[index].CellState = CellStatus.UserMarked;
    //                score = Minimax(true, board, alpha, beta);
    //                board[index].CellState = CellStatus.Empty;

    //                if (score < beta)
    //                    beta = score;

    //                if (alpha > beta)
    //                    break;
    //            }
    //        }
    //        return beta;
    //    }
    //}
    #endregion
    public int RandomMove()
    {
        BestMove =new System.Random().Next(0, 9);
        return BestMove;
    }

    public IEnumerator WakeUpAPI()
    {
        using (UnityWebRequest webService = UnityWebRequest.Get($"{baseUrl}/api/ai/wakemeup"))
        {
            yield return webService.SendWebRequest();
        }
    }
       
}
