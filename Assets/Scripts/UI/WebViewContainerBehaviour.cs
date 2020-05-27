using UnityEngine;

public class WebViewContainerBehaviour : MonoBehaviour
{
    public UniWebView webView;
    private bool WebViewVisible = false;

    private void Start()
    {
        UniWebView.SetWebContentsDebuggingEnabled(true);

        UpdateContainerVisibility();

        webView.SetSupportMultipleWindows(true);
    }

    /** 
     * Method to be used on HUD button
     */
    public void ToggleWebView()
    {
        SetWebViewVisibility(!WebViewVisible);
    }

    public void SetWebViewVisibility(bool toggle)
    {
        WebViewVisible = toggle;
        UpdateContainerVisibility();
    }

    private void UpdateContainerVisibility()
    {
        // RectTransform container = GetComponent<RectTransform>();
        gameObject.SetActive(WebViewVisible);
    }
}
