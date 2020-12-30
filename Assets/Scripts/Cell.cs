using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Button cellButton = default;
    [SerializeField] private Image displayImage = default;
    [SerializeField] private Sprite poopSprite = default;
    [SerializeField] private Sprite stickSprite = default;


    public Game GameManager { get; set; }
    public int Index { get; set; }
    public GameObject CoverPanel { get; set; }

    private void Awake()
    {
        Assert.IsNotNull(cellButton, "No reference to the Button");
        Assert.IsNotNull(displayImage, "No reference to the DisplayImage");
        Assert.IsNotNull(poopSprite, "No reference to the PoopSprite");
        Assert.IsNotNull(stickSprite, "No reference to the StickSprite");

    }


    // Called by button click and GameManager
    public void Fill()
    {
        // no more interactable once it's filled with text
        cellButton.interactable = false;
        CoverPanel.SetActive(true);

        if (GameManager.IsUserTurn)
        {
            displayImage.sprite = stickSprite;
            ChangeImageAlpha(1.0f);
            GameManager.Mainboard[Index].CellState = CellStatus.UserMarked;

        }
        else
        {
            displayImage.sprite = poopSprite;
            ChangeImageAlpha(1.0f);
            GameManager.Mainboard[Index].CellState = CellStatus.AIMarked;
            CoverPanel.SetActive(false);
        }

        GameManager.Switch();
    }

    public void ClearFill()
    {
        cellButton.interactable = true;
        displayImage.sprite = null;
        ChangeImageAlpha(0);
        GameManager.Mainboard.First(c => c.Index == Index).CellState = CellStatus.Empty;
    }

    private void ChangeImageAlpha(float alpha)
    {
        if (displayImage)
        {
            var imageColor = displayImage.color;
            imageColor.a = alpha;
            displayImage.color = imageColor;
        }
    }
}

