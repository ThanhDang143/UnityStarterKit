using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    instance = new GameObject().AddComponent<T>();
                    instance.gameObject.name = instance.GetType().Name;
                }
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public void Reset()
    {
        instance = null;
    }

    public static bool IsExists()
    {
        return (instance != null);
    }
}

public class SSSingleton<T> : SSController where T : SSController
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    instance = new GameObject().AddComponent<T>();
                    instance.gameObject.name = instance.GetType().Name;
                }
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public void Reset()
    {
        instance = null;
    }

    public static bool IsExists()
    {
        return (instance != null);
    }
}