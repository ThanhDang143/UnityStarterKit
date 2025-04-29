using UnityEngine;
using UnityEngine.UI;
using SSManager.Manager;

public class Screen3 : MonoBehaviour, IBtnBack
{
    public Text label;

    public void OnBtnBackClicked()
    {
        ScreenManager.Close(hideAnimation: ScreenAnimation.RightHide);
    }
}
