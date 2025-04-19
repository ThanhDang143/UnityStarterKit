using UnityEngine;

namespace SSManager.Manager.Template
{
    public class ScreenTemplateController : MonoBehaviour, IBtnBack
    {
        public const string NAME = "ScreenTemplate";

        public void OnBtnBack()
        {
            ScreenManager.Close();
        }
    }
}