using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvManager : MonoBehaviour
{
    private Inventory<Equipment> equipment = new Inventory<Equipment>();


    private static InvManager instance;

    public InvManager Instance
    {
        get
        {
            instance = FindAnyObjectByType<InvManager>();   //Test FindAnyObjectByType
            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<InvManager>();
                DontDestroyOnLoad(gameObject);
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
