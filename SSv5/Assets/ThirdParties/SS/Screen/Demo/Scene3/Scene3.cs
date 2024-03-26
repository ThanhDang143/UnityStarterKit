using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene3 : MonoBehaviour
{
    [SerializeField] Transform m_Cube;

    public Transform cube
    {
        get
        {
            return m_Cube;
        }
    }
}
