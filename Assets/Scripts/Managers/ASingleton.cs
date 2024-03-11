using UnityEngine;

public abstract class ASingleton<T> : MonoBehaviour where T : class
{
    public static T Instance { get; private set; } = null;

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
