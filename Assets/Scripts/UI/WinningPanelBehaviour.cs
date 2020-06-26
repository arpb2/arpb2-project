using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinningPanelBehaviour : MonoBehaviour
{
    public Button collectible1;
    public Button collectible2;
    public Button collectible3;
    private List<Button> collectibles;

    // Start is called before the first frame update
    void Start()
    {
        collectibles = new List<Button>
        {
            collectible1,
            collectible2,
            collectible3
        };
    }

    public void LightCollectible()
    {
        foreach (Button button in collectibles)
        {
            if (!button.enabled)
            {
                button.enabled = true;
                break;
            }
        }
    }
}
