using UnityEngine;

public class WebViewContainerBehaviour : MonoBehaviour
{
    public UniWebView webView;
    private bool WebViewVisible = false;

    private void Start()
    {
        UniWebView.SetWebContentsDebuggingEnabled(true);

        webView.OnMessageReceived += (view, message) => {
            Debug.Log("holis");
            Debug.Log(message.RawMessage);
        };

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
