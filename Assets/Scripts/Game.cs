using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private Board board = default;
    [SerializeField] private UI uiManager = default;

    public BoardCell[] Mainboard { get; set; }
    public bool IsUserTurn;
    private AI ai = new AI();
    private float delayForAiMove = 0.1f; // add a little delay to let the player feels like it's ai's turn  

    private bool playerStart;
    private bool firstMove;
    private void Awake()
    {
        Assert.IsNotNull(board, "No reference to the Board");
        Assert.IsNotNull(uiManager, "No reference to the UI");
    }

    private void Start()
    {
        if (board)
            board.Build(this);

        IsUserTurn = true;
        playerStart = true;
    }


    public void Switch()
    {
        IsUserTurn = !IsUserTurn;

        GameStatus status = new GameStatus(Mainboard);

        if (status.IsGameOver)
        {
            GameOver(status.GameOverType);
            return;
        }

        if (!IsUserTurn)
            Invoke("AiTurn", delayForAiMove);
    }

    private void AiTurn()
    {
        if (firstMove)
        {
            firstMove = false;
            board.Cells[ai.BestMove(Mainboard, 5)].Fill(); // less check ONLY for the first move (reducing waiting time for ai response)
        }
        else
            board.Cells[ai.BestMove(Mainboard, 9)].Fill();
    }

    private void GameOver(GameOverTypes type)
    {
        uiManager.DisplayResult(type);
        uiManager.AddScore(type);
   }

    /// <summary>
    /// Game rule: Always change the starting side (ai starts or user starts). No changes Only if the one starts and also wins the game.  
    /// </summary>
    /// <param name="type">Type of GameOver</param>
    /// <returns>if player should starts the game next round</returns>
    private bool ShouldPlayerStarts(GameOverTypes type)
    {
        if (playerStart)
        {
            switch (type)
            {
                case GameOverTypes.UserWon:
                    return true;
                case GameOverTypes.AIWon:
                    return false;
                case GameOverTypes.Tie:
                    return false;
                case GameOverTypes.Unknown:
                    return true;
                default:
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case GameOverTypes.UserWon:
                    return true;
                case GameOverTypes.AIWon:
                    return false;
                case GameOverTypes.Tie:
                    return true;
                case GameOverTypes.Unknown:
                    return false;
                default:
                    break;
            }
        }
        return true;
    }

    // called by a button click
    public void Restart()
    {
        GameStatus status = new GameStatus(Mainboard);
        // Check who should start next round
        playerStart = ShouldPlayerStarts(status.GameOverType);

        foreach (Cell cell in board.Cells)
        {
            cell.ClearFill();
        }
        uiManager.Restart();

        if (!playerStart)
        {
            IsUserTurn = false;
            board.Cells[ai.RandomMove()].Fill();
            firstMove = false;
        }
    }

    // called by a button click
    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
