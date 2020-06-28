using UnityEngine;
using UnityEngine.UI;

public class WinningPanelBehaviour : MonoBehaviour
{
    public Button wonGame;
    public Button collectible1;
    public Button collectible2;

    public int CollectiblesPicked { set; get; }

    private void Start()
    {
        wonGame.enabled = false;
        collectible1.enabled = false;
        collectible2.enabled = false;
    }

    public void SetCollectiblesPicked(int collectiblesPicked)
    {
        if (collectiblesPicked == 1)
        {
            collectible1.interactable = true;
        }
        if (collectiblesPicked == 2)
        {
            collectible2.interactable = true;
        }
    }
}
