using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab = default;
    [SerializeField] private GameObject coverPanel = default;
    public Cell[] Cells { get; set; }

    private void Awake()
    {
        Assert.IsNotNull(cellPrefab, "No reference to the CellPrefab");
        Assert.IsNotNull(coverPanel, "No reference to the CoverPanel");
    }

    private void Start()
    {
        if (coverPanel.activeSelf)
            coverPanel.SetActive(false);
    }

    public void Build(Game game)
    {
        var cells = new HashSet<Cell>();
        var myBoard = new HashSet<BoardCell>();
        if (cellPrefab)
        {
            for (int index = 0; index < 9; index++)
            {
                Cell newCell = Instantiate(cellPrefab, transform).GetComponent<Cell>();
                newCell.GameManager = game;
                newCell.Index = index;
                newCell.CoverPanel = coverPanel;
                cells.Add(newCell);
                myBoard.Add(new BoardCell(index));
            }
        }
        Cells = cells.ToArray();
        game.Mainboard = myBoard.ToArray();
    }

    public void DisableCoverPanel()
    {
        coverPanel.SetActive(false);
    }
}
