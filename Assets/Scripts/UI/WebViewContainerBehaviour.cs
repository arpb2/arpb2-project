using UnityEngine;

public class WebViewContainerBehaviour : MonoBehaviour
{
    private bool WebViewVisible = false;

    private void Start()
    {
        UpdateContainerVisibility();
    }

    public void ToggleWebView()
    {
        WebViewVisible = !WebViewVisible;

        UpdateContainerVisibility();
    }

    private void UpdateContainerVisibility()
    {
        // RectTransform container = GetComponent<RectTransform>();
        gameObject.SetActive(WebViewVisible);
    }
}
