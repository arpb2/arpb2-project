using UnityEngine;

public class OnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "EnergyCell_1_1(Clone)")
        {
            Destroy(collision.gameObject);
            // TODO: Maybe add points?
        }
        if (collision.gameObject.name == "Banner(Clone)")
        {
            GameObject.Find("GameController").GetComponent<GameControllerBehaviour>().wonLevel = true;
        }
    }
}
