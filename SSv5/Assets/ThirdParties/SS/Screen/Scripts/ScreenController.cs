using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public GameObject shield
    {
        get;
        set;
    }

    public string showAnimation
    {
        get;
        set;
    }

    public string hideAnimation
    {
        get;
        set;
    }

    private void OnDestroy()
    {
        if (shield != null)
        {
            var anim = shield.GetComponent<Animation>();
            anim.Play("ShieldHide");
            Destroy(shield, anim["ShieldHide"].length);
        }    
    }
}