

public class PopupPause : SSController
{
    public void OnBtnHomeClicked()
    {
        SSSceneManager.Instance.Close();
        SSSceneManager.Instance.Screen(ScreenNames.HOME);
    }
}
