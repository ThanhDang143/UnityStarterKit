/**
 * @author Anh Pham (Zenga Inc)
 * @email anhpt.csit@gmail.com
 * @date 2024/03/29
 */

using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public Component screen
    {
        get;
        set;
    }

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
        if (screen != null)
        {
            ScreenManager.RemoveScreen(screen);
        }

        if (shield != null)
        {
            var anim = shield.GetComponent<Animation>();
            anim.Play("ShieldHide");
            Destroy(shield, anim["ShieldHide"].length);
        }    
    }
}