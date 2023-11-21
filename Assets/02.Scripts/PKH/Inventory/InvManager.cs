using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class InvManager : MonoBehaviour
{
    public Inventory<Equipment> equipmentInv = new Inventory<Equipment>();


    private static InvManager instance;

    public static InvManager Instance
    {
        get
        {
            instance = FindAnyObjectByType<InvManager>();   //Test FindAnyObjectByType
            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<InvManager>();
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


    public void AddItem(Item item)
    {
        switch(item.GetType())
        {
            case Type type when type == typeof(Equipment):
                break;
        }
        
    }

    public void RemoveItem(Item item)
    {
        switch (item.GetType())
        {
            case Type type when type == typeof(Equipment):
                break;
        }
    }
}
