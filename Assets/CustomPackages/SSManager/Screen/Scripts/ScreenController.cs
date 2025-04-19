using UnityEngine;

namespace SSManager.Manager
{
    public class ScreenController : MonoBehaviour
    {
        public Component Screen { get; set; }
        public string ShowAnimation { get; set; }
        public string HideAnimation { get; set; }
        public string AnimationObjectName { get; set; }
        public bool HasShield { get; set; }

        private void OnDestroy()
        {
            if (Screen != null)
            {
                ScreenManager.RemoveScreen(Screen);
                ScreenManager.HideShieldOrShowTop(name);
            }
        }
    }
}