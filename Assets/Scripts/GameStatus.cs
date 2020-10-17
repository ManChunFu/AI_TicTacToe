using System.Linq;

public enum GameOverTypes
{
    UserWon,
    AIWon,
    Tie,
    Unknown,
}

public class GameStatus
{
    public bool IsGameOver { get; set; }
    public int Score { get; set; }
    public GameOverTypes GameOverType { get; set; } = GameOverTypes.Unknown;

    public void SetType(GameOverTypes type)
    {
        IsGameOver = true;
        GameOverType = type;
        Score = (GameOverType == GameOverTypes.UserWon) ? -1 : 1;
    }

    public GameStatus(BoardCell[] mainboard)
    {
        for (int rowIndex = 0; rowIndex < 3; rowIndex++)
        {
            if (mainboard.Count(c => c.RowIndex == rowIndex && c.CellState == CellStatus.UserMarked) == 3)
            {
                SetType(GameOverTypes.UserWon);
                return;
            }
            if (mainboard.Count(c => c.RowIndex == rowIndex && c.CellState == CellStatus.AIMarked) == 3)
            {
                SetType(GameOverTypes.AIWon);
                return;
            }
        }

        for (int columnIndex = 0; columnIndex < 3; columnIndex++)
        {
            if (mainboard.Count(c => c.ColumnIndex == columnIndex && c.CellState == CellStatus.UserMarked) == 3)
            {
                SetType(GameOverTypes.UserWon);
                return;
            }
            if (mainboard.Count(c => c.ColumnIndex == columnIndex && c.CellState == CellStatus.AIMarked) == 3)
            {
                SetType(GameOverTypes.AIWon);
                return;
            }
        }

        // Check diagonals
        if (mainboard.Count(c => c.ColumnIndex == c.RowIndex && c.CellState == CellStatus.UserMarked) == 3)
        {
            SetType(GameOverTypes.UserWon);
            return;
        }

        if (mainboard.Count(c => c.ColumnIndex == c.RowIndex && c.CellState == CellStatus.AIMarked) == 3)
        {
            SetType(GameOverTypes.AIWon);
            return;
        }

        if (mainboard[2].CellState == CellStatus.UserMarked && mainboard[4].CellState == CellStatus.UserMarked && mainboard[6].CellState == CellStatus.UserMarked)
        {
            SetType(GameOverTypes.UserWon);
            return;
        }

        if (mainboard[2].CellState == CellStatus.AIMarked && mainboard[4].CellState == CellStatus.AIMarked && mainboard[6].CellState == CellStatus.AIMarked)
        {
            SetType(GameOverTypes.AIWon);
            return;
        }

        if (!mainboard.Any(c => c.CellState == CellStatus.Empty))
        {
            SetType(GameOverTypes.Tie);
            return;
        }
    }
}
