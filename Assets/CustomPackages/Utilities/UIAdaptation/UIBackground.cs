using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class UIBackground : MonoBehaviour
{
    [Space]
    [SerializeField] private bool isSetupOnAwake = true;

    private void Start()
    {
        if (isSetupOnAwake) StartCoroutine(IESetup());
    }

    private IEnumerator IESetup()
    {
        yield return new WaitForEndOfFrame();
        RectTransform parentRectTransform = transform.parent.GetComponent<RectTransform>();
        if (parentRectTransform == null) parentRectTransform = transform.parent.GetComponentInParent<RectTransform>();
        if (parentRectTransform == null)
        {
            Debug.Log("<color=red> Parent RectTransform not found </color>");
            yield break;
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        Image image = GetComponent<Image>();
        Vector3 imageSize = image.sprite.bounds.size;
        Vector2 bgSize = Vector2.zero;

        bgSize.x = parentRectTransform.rect.width;
        bgSize.y = bgSize.x * imageSize.y / imageSize.x;

        if (bgSize.y < parentRectTransform.rect.height)
        {
            bgSize.y = parentRectTransform.rect.height;
            bgSize.x = bgSize.y * imageSize.x / imageSize.y;
        }

        rectTransform.sizeDelta = bgSize;
    }
}
