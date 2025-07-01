using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool applicationIsQuitting = false;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(GameManager) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (instance == null)
                {
                    instance = (T)FindFirstObjectByType(typeof(T));
                    var objects = FindObjectsOfType(typeof(T));
                    if (objects.Length > 1)
                    {
                        foreach (var obj in objects)
                        {
                            if (!ReferenceEquals(instance, (T)obj))
                            {
                                Destroy(obj);
                            }
                        }
                        return instance;
                    }

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        //DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            instance.gameObject.name);
                    }
                }

                return instance;
            }
        }
        set
        {
            if (instance != null)
                Destroy(instance);
            instance = value;
        }
    }
}
