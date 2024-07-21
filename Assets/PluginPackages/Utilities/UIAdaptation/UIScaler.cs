using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class UIScaler : MonoBehaviour
{
#if AUTO_UIADAPTATION_VERTICAL
    [SerializeField] private float baseWidth = 1080f;
    [SerializeField] private float baseHeight = 1920f;
#elif AUTO_UIADAPTATION_HORIZONTAL
    [SerializeField] private float baseWidth = 1920f;
    [SerializeField] private float baseHeight = 1080f;
#else
    [SerializeField] private float baseWidth = 1080f;
    [SerializeField] private float baseHeight = 1920f;
#endif

    public void Setup()
    {
        if (!TryGetComponent(out CanvasScaler scaler))
        {
            Debug.Log("<color=red> CanvasScaler not found </color>");
            return;
        }

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(baseWidth, baseHeight);

        float w = baseWidth / Screen.width;
        float h = baseHeight / Screen.height;
        float ratio = h / w;
        ratio = ratio >= 1 ? 1 : 0;
        scaler.matchWidthOrHeight = ratio;
    }
}
