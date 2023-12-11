using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UI CurrentUI { get; set; }

    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            instance = FindAnyObjectByType<UIManager>();
            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<UIManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
