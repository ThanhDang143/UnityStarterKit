using ThanhDV.Utilities.DebugExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace ThanhDV.Utilities.UIAdaptation
{
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class UIBackground : MonoBehaviour
    {
        [Space]
        [SerializeField] private bool setupOnAwake = true;

        private void Start()
        {
            if (setupOnAwake) Setup();
        }

        private void Setup()
        {
            if (!transform.parent.TryGetComponent<RectTransform>(out var parentRectTransform)) parentRectTransform = transform.parent.GetComponentInParent<RectTransform>();
            if (parentRectTransform == null)
            {
                DebugExtension.Log("Parent RectTransform not found!!!", Color.red);
                return;
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
}
