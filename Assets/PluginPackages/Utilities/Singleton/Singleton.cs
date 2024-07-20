using UnityEngine;


public class Singleton<T> where T : class, new()
{
    private static T _instance;
    private static readonly object _lock = new object();

    private Singleton() { } // Private constructor to prevent object creation from outside the class

    public static T Instance
    {
        get
        {
            lock (_lock) // Lock to ensure thread safety
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;

                    if (_instance == null)
                    {
                        _instance = new GameObject().AddComponent<T>();
                        _instance.gameObject.name = _instance.GetType().Name;
                    }
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }
    }
}