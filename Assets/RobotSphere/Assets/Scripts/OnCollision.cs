using UnityEngine;

public class OnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(">>> Collided with: " + collision.gameObject.name);
        if (collision.gameObject.name == "EnergyCell_1_1(Clone)")
        {
            Destroy(collision.gameObject);
            // TODO: Maybe add points?
        }
        if (collision.gameObject.name == "Banner")
        {
            GameObject.Find("GameController").GetComponent<GameControllerBehaviour>().WonLevel = true;
            collision.gameObject.GetComponentInParent<BannerBehaviour>().ShowConfetti();
        }
    }
}
