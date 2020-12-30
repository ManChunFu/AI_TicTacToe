using System;

public enum CellStatus
{
    UserMarked,
    AIMarked,
    Empty
}

public class BoardCell
{
    public int Index { get; set; }
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public CellStatus CellState { get; set; } = CellStatus.Empty;

    public override string ToString()
    {
        return $"{{\"index\":{Index},\"rowIndex\":{RowIndex},\"columnIndex\":{ColumnIndex},\"cellState\":{(int)CellState}}}";
    }
    public BoardCell(int index)
    {
        this.RowIndex = index / 3;
        this.ColumnIndex = index % 3;
        this.Index = index;
    }

}

