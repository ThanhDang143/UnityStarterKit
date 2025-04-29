using UnityEngine;

namespace SSManager.Manager.Template
{
    public class XXXYController : MonoBehaviour, IBtnBack
    {
        public const string NAME = "XXXY";

        public void OnBtnBackClicked()
        {
            ScreenManager.Close();
        }
    }
}