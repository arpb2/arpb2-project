using UnityEngine;

public class OnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "EnergyCell_1_1(Clone)")
        {
            Destroy(collision.gameObject);
            GetComponent<MainCharacterBehaviour>().AddPoint();
        }
        if (collision.gameObject.name == "Banner")
        {
            GameObject.Find("GameController").GetComponent<GameControllerBehaviour>().SetAsWon();
            collision.gameObject.GetComponentInParent<BannerBehaviour>().ShowConfetti();
        }
    }
}
