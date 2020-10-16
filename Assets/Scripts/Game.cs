using UnityEngine;
using UnityEngine.Assertions;

public class Game : MonoBehaviour
{
    [SerializeField] private Board board = default;
    [SerializeField] private UI uiManager = default;

    public BoardCell[] Mainboard { get; set; }
    public bool IsUserTurn;
    private AI ai = new AI();
    private float delayForAiMove = 0.1f; // add a little delay to let the player feels like it's ai's turn  

    private bool playerStart;
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
        board.Cells[ai.BestMove(Mainboard)].Fill();
    }

    private void GameOver(GameOverTypes type)
    {
        uiManager.DisplayResult(type);
        uiManager.AddScore(type);

        if (type == GameOverTypes.Tie)
            playerStart = !playerStart;
        else
            playerStart = true;
    }

    // called by a button click
    public void Restart()
    {
        foreach (Cell cell in board.Cells)
        {
            cell.ClearFill();
        }
        uiManager.Restart();

        if (!playerStart)
            board.Cells[ai.RandomMove()].Fill();
    }

    // called by a button click
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
