using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerBehaviour : MonoBehaviour
{
    public GameObject Confetti;

    void Start()
    {
        Confetti.SetActive(false);
    }

    public void ShowConfetti()
    {
        Confetti.SetActive(true);
    }
}
