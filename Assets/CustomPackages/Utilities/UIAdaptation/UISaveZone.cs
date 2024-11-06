using UnityEngine;

public class UISaveZone : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private bool ignoreSaveZone = false;
    [SerializeField] private RectTransform[] ignoreObjs;

    public void Setup()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Rect saveZone = Screen.safeArea;
        Vector2 anchorMin = saveZone.position;
        Vector2 anchorMax = anchorMin + saveZone.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        if (ignoreSaveZone)
        {
            foreach (RectTransform ignoreObj in ignoreObjs)
            {
                Vector2 tempAnchorMin = ignoreObj.anchorMin;
                Vector2 tempAnchorMax = ignoreObj.anchorMax;

                tempAnchorMin.x = 2 * ignoreObj.anchorMin.x - anchorMin.x;
                tempAnchorMin.y = 2 * ignoreObj.anchorMin.y - anchorMin.y;
                tempAnchorMax.x = 2 * ignoreObj.anchorMax.x - anchorMax.x;
                tempAnchorMax.y = 2 * ignoreObj.anchorMax.y - anchorMax.y;

                ignoreObj.anchorMin = tempAnchorMin;
                ignoreObj.anchorMax = tempAnchorMax;

                ignoreObj.offsetMin = Vector2.zero;
                ignoreObj.offsetMax = Vector2.zero;
            }
        }
    }
}
