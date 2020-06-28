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
        collectible1.interactable = false;
        collectible2.interactable = false;
    }

    public void SetCollectiblesPicked(int collectiblesPicked)
    {
        if (collectiblesPicked == 1)
        {
            LightCollectible1();
        }
        if (collectiblesPicked == 2)
        {
            LightCollectible1();
            collectible2.interactable = true;
            collectible2.enabled = false;
        }
    }

    private void LightCollectible1()
    {
        collectible1.interactable = true;
        collectible1.enabled = false;
    }
}
