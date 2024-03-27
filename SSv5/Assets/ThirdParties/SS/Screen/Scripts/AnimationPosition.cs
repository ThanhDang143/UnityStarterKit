using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPosition : MonoBehaviour
{
    [SerializeField] float m_BaseWidth = 720;
    [SerializeField] float m_BaseHeight = 1600;

    float m_Width;
    float m_Height;
    RectTransform m_Rect;
    Animation m_Animation;

    private void Start()
    {
        m_Animation = GetComponent<Animation>();
        m_Rect = GetComponent<RectTransform>();
        m_Height = m_BaseHeight;
        m_Width = m_Height * Screen.width / Screen.height;
    }

    private void LateUpdate()
    {
        if (m_Animation != null && m_Animation.isPlaying)
        {
            m_Rect.anchoredPosition = new Vector2(m_Rect.anchoredPosition.x * m_Width / m_BaseWidth, m_Rect.anchoredPosition.y * m_Height / m_BaseHeight);
        }
    }
}