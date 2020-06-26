using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public WinningPanelBehaviour WinningPanel;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "EnergyCell_1_1(Clone)")
        {
            Destroy(collision.gameObject);
            WinningPanel.LightCollectible();
        }
        if (collision.gameObject.name == "Banner(Clone)")
        {
            GameObject.Find("GameController").GetComponent<GameControllerBehaviour>().wonLevel = true;
        }
    }
}
