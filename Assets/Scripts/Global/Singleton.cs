using UnityEngine;

public class Singleton<T> where T : class, new()
{
    private static T _instance;
    private static readonly object _lock = new object();

    private Singleton() { } // Constructor riêng tư để ngăn chặn việc tạo đối tượng từ bên ngoài

    public static T Instance
    {
        get
        {
            lock (_lock) // Khóa để đảm bảo thread-safe
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

public class SSSingleton<T> : SSController where T : SSController
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