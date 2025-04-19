using UnityEngine;

namespace SSManager.Manager
{
    public class AnimationPosition : MonoBehaviour
    {
        [SerializeField] float m_BaseWidth = 720;
        [SerializeField] float m_BaseHeight = 1600;

        float m_Width;
        float m_Height;
        RectTransform m_Rect;
        UnscaledAnimation m_Animation;

        private void Awake()
        {
            m_Animation = GetComponent<UnscaledAnimation>();
            m_Rect = GetComponent<RectTransform>();
            m_Height = m_BaseHeight;
            m_Width = m_Height * UnityEngine.Screen.width / UnityEngine.Screen.height;
        }

        private void LateUpdate()
        {
            if (m_Animation != null && m_Animation.isPlaying)
            {
                Reposition();
            }
        }

        public void Reposition()
        {
            m_Rect.anchoredPosition = new Vector2(m_Rect.anchoredPosition.x * m_Width / m_BaseWidth, m_Rect.anchoredPosition.y * m_Height / m_BaseHeight);
        }
    }
}