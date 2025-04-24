using UnityEngine;

namespace SSManager.Manager.Template
{
    public class XXXController : MonoBehaviour, IBtnBack
    {
        public const string NAME = "XXX";

        public void OnBtnBack()
        {
            ScreenManager.Close();
        }
    }
}