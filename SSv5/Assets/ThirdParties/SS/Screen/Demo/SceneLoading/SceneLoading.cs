using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] RectTransform m_Progress;

    private void Update()
    {
        m_Progress.sizeDelta = new Vector2(ScreenManager.asyncOperation.progress * 500, 100);
    }
}