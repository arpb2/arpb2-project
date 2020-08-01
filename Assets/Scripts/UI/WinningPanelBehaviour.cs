using System.Collections.Generic;
using UnityEngine;

public class WinningPanelBehaviour : MonoBehaviour
{
    public List<UnityEngine.UI.Image> PointsObjects;

    private Color PointOnColor = Color.white;
    private Color PointOffColor = Color.black;

    public void ShowWithPoints(int points)
    {
        Debug.Log(string.Format("Showing winning panel with {0} points", points));

        for (int i = 0; i < PointsObjects.Count; ++i)
        {
            PointsObjects[i].color = i < points ? PointOnColor : PointOffColor;
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
