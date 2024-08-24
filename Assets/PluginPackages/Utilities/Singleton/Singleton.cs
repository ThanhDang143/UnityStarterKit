using UnityEngine;

public class Singleton<T> where T : class, new()
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
                    Debug.Log($"<color=yellow>{_instance.GetType().Name} instance is null!!! Auto create new instance</color>");

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
                    _instance = FindFirstObjectByType(typeof(T)) as T;

                    if (_instance == null)
                    {
                        Debug.Log($"<color=yellow>{_instance.GetType().Name} instance is null!!! Auto create new instance</color>");

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